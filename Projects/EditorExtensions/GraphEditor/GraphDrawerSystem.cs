using System.Collections.Generic;
using System.Linq;
using EditorExtensions.GraphEditor.Utilities;
using Graphs;
using UnityEditor;
using UnityEngine;
using Utilities.Extensions;

namespace EditorExtensions.GraphEditor
{
    public class GraphDrawerSystem
    {
        private const int SelectionRadius = 6;
        
        private Dictionary<Node, NodeDrawInfo> _nodeDrawInfos = new Dictionary<Node, NodeDrawInfo>();
        
        public HashSet<NodeDrawInfo> SelectedNodes = new HashSet<NodeDrawInfo>();

        public void DrawNodes()
        {
            var defaultColor = Handles.color;
            foreach (var drawInfo in _nodeDrawInfos.Values)
            {
                var drawingPosition = DrawingContext.Current.ApplyScroll(drawInfo.Position);
                if (SelectedNodes.Contains(drawInfo))
                {
                    Handles.color = Color.yellow;;
                    Handles.DrawSolidDisc(drawingPosition, Vector3.forward, drawInfo.Radius + SelectionRadius);
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
        public Node GetNode(NodeDrawInfo nodeDrawInfo)
        {
            return _nodeDrawInfos.FirstOrDefault(n => n.Value == nodeDrawInfo).Key;
        }
        
        //TODO Optimize search
        public bool GetNodeDrawInfoByPosition(Vector2 position, out NodeDrawInfo drawInfo)
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
        
        public HashSet<NodeDrawInfo> GetNodeDrawInfoByRect(Rect rect)
        {
            return _nodeDrawInfos.Values.Where(n => rect.Contains(n.Position, true)).ToHashSet();
        }

        #region selection
        
        public void Select(NodeDrawInfo node)
        {
            SelectedNodes.Add(node);
        }
        
        public void Select(HashSet<NodeDrawInfo> nodes)
        {
            SelectedNodes = new HashSet<NodeDrawInfo>(nodes);
        }

        public void ClearSelection(NodeDrawInfo node)
        {
            SelectedNodes.Remove(node);
        }

        public void CleanUpSelection()
        {
            SelectedNodes.Clear();
        }
        
        #endregion

        public void DrawArcs(IEnumerable<Arc> arcs)
        {
            var drawingContext = DrawingContext.Current;
            var passedArcs = new HashSet<Arc>();
            foreach (var arc in arcs)
            {
                var nodeFrom = _nodeDrawInfos[arc.From];
                var nodeTo = _nodeDrawInfos[arc.To];
                
                var from = drawingContext.ApplyScroll(nodeFrom.Position);
                var to = drawingContext.ApplyScroll(nodeTo.Position);
                
                var oppositeDirectionArc = arc.To.GetArcTo(arc.From);
                if (oppositeDirectionArc != null)
                {
                    if (arc.From == arc.To)
                    {
                        DrawUtilities.DrawLoop(from, nodeTo.Radius);   
                    }
                    else
                    {
                        DrawUtilities.DrawDirectionalLine(from, to, nodeTo.Radius, true);
                        passedArcs.Add(oppositeDirectionArc);
                    }
                }
                else
                {
                    DrawUtilities.DrawDirectionalLine(from, to, nodeTo.Radius, false);
                }
            }
        }
    }
}