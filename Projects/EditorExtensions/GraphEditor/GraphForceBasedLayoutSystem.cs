using System;
using System.Collections.Generic;
using System.Linq;
using Graphs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EditorExtensions.GraphEditor
{
    public class GraphForceBasedLayoutSystem : IGraphLayoutSystem
    {
        private class NodeLayoutInfo 
        {

            public NodeDrawInfo NodeDrawInfo; // reference to draw inforamtion about node
            public Node Node;			// reference to the node in the simulation
            public Vector2 Velocity;		// the node's current velocity, expressed in vector form
            public Vector2 NextPosition;	// the node's position after the next iteration
            public NodeLayoutInfo[] linkedNodes;

            /// <summary>
            /// Initialises a new instance of the Diagram.NodeLayoutInfo class, using the specified parameters.
            /// </summary>
            /// <param name="node"></param>
            /// <param name="nodeDrawInfo"></param>
            /// <param name="velocity"></param>
            /// <param name="nextPosition"></param>
            public NodeLayoutInfo(Node node, NodeDrawInfo nodeDrawInfo, Vector2 velocity, Vector2 nextPosition) 
            {
                Node = node;
                NodeDrawInfo = nodeDrawInfo;
                Velocity = velocity;
                NextPosition = nextPosition;
            }

            public void InitializeLinks(Dictionary<Node, NodeLayoutInfo> nodeLayoutInfos)
            {
                linkedNodes = Node.ArcsOut.Where(a => a.To != Node).Select(a => nodeLayoutInfos[a.To]).ToArray();
            }
        }
	    
        private const float ATTRACTION_CONSTANT = 0.1f;		// spring constant
        private const float REPULSION_CONSTANT = 10000f;	// charge constant

        private const float DEFAULT_DAMPING = 0.5f;
        private const int DEFAULT_SPRING_LENGTH = 100;
        private const int DEFAULT_MAX_ITERATIONS = 500;
	    
        public void Layout(Dictionary<Node, NodeDrawInfo> nodes)
        {
            Arrange(nodes, DEFAULT_DAMPING, DEFAULT_SPRING_LENGTH, DEFAULT_MAX_ITERATIONS, true);
        }
        
        public void Arrange(Dictionary<Node, NodeDrawInfo> nodes, float damping, int springLength, int maxIterations, bool deterministic) 
        {
		
            // random starting positions can be made deterministic by seeding System.Random with a constant
            Random.InitState(0);

            // copy nodes into an array of metadata and randomise initial coordinates for each node
            NodeLayoutInfo[] layout = new NodeLayoutInfo[nodes.Count];
            Dictionary<Node, NodeLayoutInfo> layoutDictionary = new Dictionary<Node, NodeLayoutInfo>();

            int index = 0;
            foreach (var node in nodes)
            {
                layout[index] = new NodeLayoutInfo(node.Key, node.Value, new Vector2(), Vector2.zero)
                {
                    NodeDrawInfo = {Position = new Vector2(Random.value * 500, Random.value * 500)}
                };
                layoutDictionary.Add(node.Key, layout[index]);
                index++;
            }

            foreach (var nodeLayoutInfo in layout)
            {
                nodeLayoutInfo.InitializeLinks(layoutDictionary);
            }

            int stopCount = 0;
            int iterations = 0;

            while (true) {
                double totalDisplacement = 0;

                for (int i=0; i<layout.Length; i++) {
                    NodeLayoutInfo current = layout[i];

                    // express the node's current position as a vector, relative to the origin
                    Vector2 currentPosition = new Vector2(CalcDistance(Vector2.zero, current.NodeDrawInfo.Position), GetBearingAngle(Vector2.zero, current.NodeDrawInfo.Position));
                    Vector2 netForce = new Vector2(0, 0);

                    // determine repulsion between nodes
                    foreach (var other in layout) {
                        if (other != current) netForce += CalcRepulsionForce(current, other);
                    }

                    // determine attraction caused by connections
                    foreach (var child in current.linkedNodes) {
                        netForce += CalcAttractionForce(current.NodeDrawInfo, child.NodeDrawInfo, springLength);
                    }
                    foreach (var parent in layout) {
                        if (parent.linkedNodes.Any(n => n == current)) netForce += CalcAttractionForce(current.NodeDrawInfo, parent.NodeDrawInfo, springLength);
                    }
				
                    // apply net force to node velocity
                    current.Velocity = (current.Velocity + netForce) * damping;

                    // apply velocity to node position
                    current.NextPosition = (currentPosition + current.Velocity);
                }

                // move nodes to resultant positions (and calculate total displacement)
                for (int i = 0; i < layout.Length; i++) {
                    NodeLayoutInfo current = layout[i];

                    totalDisplacement += CalcDistance(current.NodeDrawInfo.Position, current.NextPosition);
                    current.NodeDrawInfo.Position = current.NextPosition;
                }

                iterations++;
                if (totalDisplacement < 10) stopCount++;
                if (stopCount > 15) break;
                if (iterations > maxIterations) break;
            }
	        
	        

            // center the diagram around the origin
            Rect logicalBounds = GetDiagramBounds(layout);
            Vector2 midPoint = logicalBounds.center;
            
            
            Debug.Log(midPoint);
            foreach (var node in layout) {
            	node.NodeDrawInfo.Position -= midPoint;
                Debug.Log(node.NodeDrawInfo.Position);
            }
        }
        
        
        private Rect GetDiagramBounds(NodeLayoutInfo[] layout) {
            float minX = float.MaxValue, minY = float.MaxValue;
            float maxX = float.MinValue, maxY = float.MinValue;
            foreach (var node in layout) {
                if (node.NodeDrawInfo.Position.x < minX) minX = node.NodeDrawInfo.Position.x;
                if (node.NodeDrawInfo.Position.x  > maxX) maxX = node.NodeDrawInfo.Position.x;
                if (node.NodeDrawInfo.Position.y < minY) minY = node.NodeDrawInfo.Position.y;
                if (node.NodeDrawInfo.Position.y > maxY) maxY = node.NodeDrawInfo.Position.y;
            }

            return new Rect(minX, minY, maxX - minX, maxY - minY);
        }
	    
        /// <summary>
        /// Calculates the bearing angle from one point to another.
        /// </summary>
        /// <param name="start">The node that the angle is measured from.</param>
        /// <param name="end">The node that creates the angle.</param>
        /// <returns>The bearing angle, in degrees.</returns>
        private float GetBearingAngle(Vector2 start, Vector2 end) {
            Vector2 half = new Vector2(start.x + ((end.x - start.x) / 2), start.y + ((end.y - start.y) / 2));

            float diffX = half.x - start.x;
            float diffY = half.y - start.y;

            if (diffX == 0) diffX = 0.001f;
            if (diffY == 0) diffY = 0.001f;

            double angle;
            if (Mathf.Abs(diffX) > Mathf.Abs(diffY)) {
                angle = System.Math.Tanh(diffY / diffX) * (180.0 / System.Math.PI);
                if (((diffX < 0) && (diffY > 0)) || ((diffX < 0) && (diffY < 0))) angle += 180;
            }
            else {
                angle = System.Math.Tanh(diffX / diffY) * (180.0 / System.Math.PI);
                if (((diffY < 0) && (diffX > 0)) || ((diffY < 0) && (diffX < 0))) angle += 180;
                angle = (180 - (angle + 90));
            }

            return (float)angle;
        }
	    
        /// <summary>
        /// Calculates the attraction force between two connected nodes, using the specified spring length.
        /// </summary>
        /// <param name="x">The node that the force is acting on.</param>
        /// <param name="y">The node creating the force.</param>
        /// <param name="springLength">The length of the spring, in pixels.</param>
        /// <returns>A Vector representing the attraction force.</returns>
        private Vector2 CalcAttractionForce(NodeDrawInfo x, NodeDrawInfo y, float springLength) {
            int proximity = Mathf.Max(CalcDistance(x.Position, y.Position), 1);

            // Hooke's Law: F = -kx
            float force = ATTRACTION_CONSTANT * Mathf.Max(proximity - springLength, 0);
            float angle = GetBearingAngle(x.Position, y.Position);

            return new Vector2(force, angle);
        }

        /// <summary>
        /// Calculates the distance between two points.
        /// </summary>
        /// <param name="a">The first point.</param>
        /// <param name="b">The second point.</param>
        /// <returns>The pixel distance between the two points.</returns>
        public static int CalcDistance(Vector2 a, Vector2 b) {
            float xDist = (a.x - b.x);
            float yDist = (a.y - b.y);
            return (int)Mathf.Sqrt(Mathf.Pow(xDist, 2) + Mathf.Pow(yDist, 2));
        }

        /// <summary>
        /// Calculates the repulsion force between any two nodes in the diagram space.
        /// </summary>
        /// <param name="x">The node that the force is acting on.</param>
        /// <param name="y">The node creating the force.</param>
        /// <returns>A Vector representing the repulsion force.</returns>
        private Vector2 CalcRepulsionForce(NodeLayoutInfo x, NodeLayoutInfo y) {
            int proximity = Mathf.Max(CalcDistance(x.NodeDrawInfo.Position, y.NodeDrawInfo.Position), 1);

            // Coulomb's Law: F = k(Qq/r^2)
            float force = -(REPULSION_CONSTANT / Mathf.Pow(proximity, 2));
            float angle = GetBearingAngle(x.NodeDrawInfo.Position, y.NodeDrawInfo.Position);

            return new Vector2(force, angle);
        }
    }
}