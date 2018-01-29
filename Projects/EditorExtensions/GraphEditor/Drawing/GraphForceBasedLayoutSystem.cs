using System.Collections.Generic;
using System.Linq;
using Graphs;
using UnityEngine;
using Random = UnityEngine.Random;

namespace EditorExtensions.GraphEditor.Drawing
{
    public class GraphForceBasedLayoutSystem : IGraphLayoutSystem {
        
        private struct NodeLayoutInfo 
        {
            public readonly NodeDrawInfo NodeDrawInfo;
            public readonly Node Node;		
            public Vector2 Velocity;	
            public readonly int Mass;  

            public NodeLayoutInfo(Node node, NodeDrawInfo nodeDrawInfo, int mass = 1) 
            {
                Node = node;
                NodeDrawInfo = nodeDrawInfo;
                Velocity = Vector2.zero;
                Mass = mass;
            }        
        }
        
        public void Layout(Dictionary<Node, NodeDrawInfo> nodes)
        {
            ApplyForces(nodes, 1000, 1000, 100, 800, 0.5f);
        }

        private void ApplyForces(Dictionary<Node, NodeDrawInfo> nodes, int pbHeight, int pbWidth, float spring, float charge, float damping)
        {
            // random starting positions can be made deterministic by seeding System.Random with a constant
            Random.InitState(0);

            // copy nodes into an array of metadata and randomise initial coordinates for each node
            NodeLayoutInfo[] layoutInfos = new NodeLayoutInfo[nodes.Count];

            int index = 0;
            foreach (var node in nodes)
            {
                layoutInfos[index] = new NodeLayoutInfo(node.Key, node.Value)
                {
                    NodeDrawInfo =
                    {
                        Position = new Vector2(Random.value*pbHeight, Random.value*pbWidth)
                    }
                };
                index++;
            }

            float diff = 0;
            float diff1 = 1;
            var nodesCount = layoutInfos.Length;
            
            while (CheckDisplacement(diff, diff1))
            {
                diff1 = diff;
                diff = 0;
                
                for (int i = 0; i < nodesCount; i++)
                {
                    var node = layoutInfos[i];
                    for (int j = 0; j < nodesCount; j++)
                    {
                        var otherNode = layoutInfos[j];
                        if (node.Node != otherNode.Node)
                        {
                            
                            float dx = otherNode.NodeDrawInfo.Position.x - node.NodeDrawInfo.Position.x;
                            float dy = otherNode.NodeDrawInfo.Position.y - node.NodeDrawInfo.Position.y;
                            var velocity = new Vector2(dx, dy);
                            
                            float hypotenuse = Mathf.Sqrt(Mathf.Pow(dx, 2) + Mathf.Pow(dy, 2));
                            float force = 0;
                            if (node.Node.GetArcTo(otherNode.Node) != null || otherNode.Node.GetArcTo(node.Node) != null)
                            {
                                force = (hypotenuse - spring)/2.0f;
                            }
                            else
                            {
                                force = -((node.Mass*otherNode.Mass)/Mathf.Pow(hypotenuse, 2))*charge;
                            }

                            velocity /= hypotenuse;
                            velocity *= force;
                            node.Velocity += velocity;
                        }
                    }
                    node.NodeDrawInfo.Position += node.Velocity;
                    node.Velocity *= damping;
                    diff += Mathf.Abs(node.Velocity.x) + Mathf.Abs(node.Velocity.y);
                }
            }
            
            AlignCenterNodes(layoutInfos);
        }

        private void AlignCenterNodes(NodeLayoutInfo[] layoutInfos)
        {
            var nodeDrawInfos = layoutInfos.Select(l => l.NodeDrawInfo).ToArray();

            if (nodeDrawInfos.Length == 0)
            {
                return;
            }
            
            var minX = (int) nodeDrawInfos[0].Position.x;
            var minY = (int) nodeDrawInfos[0].Position.y;
            var maxX = (int) nodeDrawInfos[0].Position.x;
            var maxY = (int) nodeDrawInfos[0].Position.y;
            
            foreach (var n in nodeDrawInfos)
            {
                if (n.Position.x < minX) minX = (int) n.Position.x;
                if (n.Position.y < minY) minY = (int) n.Position.y;
                if (n.Position.x > maxX) maxX = (int) n.Position.x;
                if (n.Position.y > maxY) maxY = (int) n.Position.y;
            }

            var rect = new Rect(minX, minY, maxX - minX, maxY - minY);
            foreach (var nodeLayoutInfo in nodeDrawInfos)
            {
                nodeLayoutInfo.Position -= rect.center;
                nodeLayoutInfo.Position += DrawingContext.Current.Viewport.center;
            }
        }

        private bool CheckDisplacement(float diff, float diff1)
        {
            return (Mathf.Abs(diff - diff1) > 0.0001 && Mathf.Abs(diff - diff1) < 10000) || diff > 1;
        }
    }
}