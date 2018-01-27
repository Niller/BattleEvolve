using System.Collections.Generic;
using System.Linq;
using Graphs;
using UnityEditor;
using UnityEngine;
using Utilities.Extensions;

namespace EditorExtensions.GraphEditor
{
    public class GraphDrawerSystem
    {
        private const int SelectionRadius = 6;
        
        private Dictionary<int, NodeDrawInfo> _nodeDrawInfos = new Dictionary<int, NodeDrawInfo>();
        
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
            _nodeDrawInfos = new Dictionary<int, NodeDrawInfo>();
            for (var i = 0; i < graph.Nodes.Count; i++)
            {
                var node = graph.Nodes[i];
                _nodeDrawInfos[node.Id] = new NodeDrawInfo(node.Id, 50 * i * Vector2.one);
            }
        }

        public void AddNode(int id, Vector2 position)
        {
            _nodeDrawInfos.Add(id, new NodeDrawInfo(id, position));
        }

        public void RemoveNode(int id)
        {
            _nodeDrawInfos.Remove(id);
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

        public void DrawArcs()
        {
            
        }
        
    }
}