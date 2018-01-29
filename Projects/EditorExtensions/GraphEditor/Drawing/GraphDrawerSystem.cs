using System.Collections.Generic;
using System.Linq;
using EditorExtensions.GraphEditor.Utilities;
using Graphs;
using UnityEditor;
using UnityEngine;
using Utilities.Extensions;

namespace EditorExtensions.GraphEditor.Drawing
{
    public class GraphDrawerSystem : IGraphDrawerSystem
    {
        private const int SelectionRadius = 6;
        private const int SelectionArcDistanceThreshold = 15;

        private Dictionary<Node, NodeDrawInfo> _nodeDrawInfos = new Dictionary<Node, NodeDrawInfo>();

        public HashSet<INodeDrawInfo> SelectedNodes
        {
            get;
            private set;
        }

        public IEnumerable<INodeDrawInfo> Nodes => _nodeDrawInfos.Values.Cast<INodeDrawInfo>();

        private readonly IGraphLayoutSystem _graphLayoutSystem;

        public Arc SelectedArc
        {
            get;
            private set;
        }

        public GraphDrawerSystem()
        {
            _graphLayoutSystem = new GraphForceBasedLayoutSystem();
            SelectedNodes = new HashSet<INodeDrawInfo>();
        }

        public void DrawNodes()
        {
            var defaultColor = Handles.color;
            foreach (var drawInfo in _nodeDrawInfos.Values)
            {
                var drawingPosition = DrawingContext.Current.ApplyScroll(drawInfo.Position);
                if (SelectedNodes.Contains(drawInfo))
                {
                    Handles.color = Color.yellow;;
                    Handles.DrawSolidDisc(drawingPosition, Vector3.forward, drawInfo.Radius + SelectionRadius * DrawingContext.Current.Zoom);
                }
                Handles.color = Color.red;
                Handles.DrawSolidDisc(drawingPosition, Vector3.forward, drawInfo.Radius);
                
            }
            Handles.color = defaultColor;
        }
        
        public void UpdateDrawInfos(Graph graph)
        {
            _nodeDrawInfos = new Dictionary<Node, NodeDrawInfo>();
            foreach (var node in graph.Nodes)
            {
                _nodeDrawInfos[node] = new NodeDrawInfo(node.Id, Vector2.zero);
            }
            Layout();
        }

        public void AddNode(Node node, Vector2 position)
        {
            _nodeDrawInfos.Add(node, new NodeDrawInfo(node.Id, position));
        }

        public void RemoveNode(Node node)
        {
            _nodeDrawInfos.Remove(node);
        }

        //TODO Optimize
        public Node GetNode(INodeDrawInfo nodeDrawInfo)
        {
            return _nodeDrawInfos.FirstOrDefault(n => n.Value == nodeDrawInfo).Key;
        }
        
        //TODO Optimize search
        public bool GetNodeDrawInfoByPosition(Vector2 position, out INodeDrawInfo drawInfo)
        {
            foreach (var nodeDrawInfo in _nodeDrawInfos.Values)
            {
                if (Vector2.Distance(nodeDrawInfo.Position, position) <= nodeDrawInfo.Radius)
                {
                    drawInfo = nodeDrawInfo;
                    return true;
                }
            }
            drawInfo = default(NodeDrawInfo);
            return false;
        }
        
        public HashSet<INodeDrawInfo> GetNodeDrawInfoByRect(Rect rect)
        {
            return _nodeDrawInfos.Values.Where(n => rect.Contains(n.Position, true)).Cast<INodeDrawInfo>().ToHashSet();
        }

        //TODO Optimize search
        public bool SelectArcByPostion(Vector2 position)
        {
            foreach (var nodePair in _nodeDrawInfos)
            {
                foreach (var arc in nodePair.Key.ArcsOut)
                {
                    bool needSelectArc = arc.IsLoop()
                        ? CheckDistanceFromPointToLoop(position, nodePair.Value)
                        : ChectDistanceFromPointToArc(position, arc);
                    if (needSelectArc)
                    {
                        SelectedArc = arc;
                        return true;
                    }
                }
            }
            return false;
        }

        private bool CheckDistanceFromPointToLoop(Vector2 p, INodeDrawInfo nodeDrawInfo)
        {
            var drawInfo = nodeDrawInfo as NodeDrawInfo;
            
            Vector3 fromDirection = Vector3.down;
            Vector3 toDirection = Vector3.right;;

            var from = (Vector3)nodeDrawInfo.Position + fromDirection * drawInfo.Radius;
            var to = (Vector3)nodeDrawInfo.Position + toDirection * drawInfo.Radius;

            var tangentDistance = drawInfo.Radius * 4;
            
            var startTangent = from + fromDirection * tangentDistance;
            var endTangent = from + toDirection * tangentDistance;

            return HandleUtility.DistancePointBezier(p, from, to, startTangent, endTangent) < SelectionArcDistanceThreshold * DrawingContext.Current.Zoom;
        }

        private bool ChectDistanceFromPointToArc(Vector2 p, Arc arc)
        {
            
            var p0 = _nodeDrawInfos[arc.From].Position;
            var p1 = _nodeDrawInfos[arc.To].Position;
            var distance = HandleUtility.DistancePointToLineSegment(p, p0, p1);
            return distance < SelectionArcDistanceThreshold * DrawingContext.Current.Zoom;
        }

        #region selection
        
        public void Select(INodeDrawInfo node)
        {
            SelectedNodes.Add(node);
            DeselectArc();                        
        }
        
        public void Select(HashSet<INodeDrawInfo> nodes)
        {
            SelectedNodes = new HashSet<INodeDrawInfo>(nodes);
        }

        public void Deselect(INodeDrawInfo node)
        {
            SelectedNodes.Remove(node);
        }

        public void CleanUpSelection()
        {
            SelectedNodes.Clear();
        }
        
        public void DeselectArc()
        {
            SelectedArc = null;
        }
           
        #endregion

        public void DrawArcs(IEnumerable<Arc> arcs)
        {
            var drawingContext = DrawingContext.Current;
            var passedArcs = new HashSet<Arc>();
            foreach (var arc in arcs)
            {
                if (passedArcs.Contains(arc))
                {
                    continue;
                }

                var nodeFrom = _nodeDrawInfos[arc.From];
                var nodeTo = _nodeDrawInfos[arc.To];
                
                var from = drawingContext.ApplyScroll(nodeFrom.Position);
                var to = drawingContext.ApplyScroll(nodeTo.Position);
                
                var oppositeDirectionArc = arc.To.GetArcTo(arc.From);
                if (oppositeDirectionArc != null)
                {
                    if (arc.From == arc.To)
                    {
                        DrawLoop(nodeFrom, arc == SelectedArc);   
                    }
                    else
                    {
                        DrawArc(nodeFrom, nodeTo, arc == SelectedArc, true);
                        passedArcs.Add(oppositeDirectionArc);
                    }
                }
                else
                {
                    DrawArc(nodeFrom, nodeTo,arc == SelectedArc);
                }
            }
        }

        public void DrawLoop(INodeDrawInfo node, bool isSelected)
        {
            var drawInfo = node as NodeDrawInfo;
            DrawUtilities.DrawLoop(DrawingContext.Current.ApplyScroll(node.Position), drawInfo.Radius, GetArcColor(isSelected), DrawingContext.Current.Zoom);
        }

        public void DrawArc(INodeDrawInfo from, Vector2 to, bool isSelected, bool isTwoDirectional = false)
        {
            var fromNodeDrawInfo = from as NodeDrawInfo;
            DrawUtilities.DrawDirectionalLine(DrawingContext.Current.ApplyScroll(from.Position), to, fromNodeDrawInfo.Radius, GetArcColor(isSelected), 
                DrawingContext.Current.Zoom, isTwoDirectional);
        }
        
        public void DrawArc(INodeDrawInfo from, INodeDrawInfo to, bool isSelected, bool isTwoDirectional = false)
        {
            var toNodeDrawInfo = to as NodeDrawInfo;
            DrawUtilities.DrawDirectionalLine(DrawingContext.Current.ApplyScroll(from.Position), 
                DrawingContext.Current.ApplyScroll(to.Position), toNodeDrawInfo.Radius, GetArcColor(isSelected), DrawingContext.Current.Zoom, isTwoDirectional);
        }

        private Color GetArcColor(bool isSelected)
        {
            return isSelected ? Color.yellow : Color.white;
        }

        public void Layout()
        {
            _graphLayoutSystem.Layout(_nodeDrawInfos);
        }
    }
}