using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using Graphs;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public struct NodeDrawInfo
    {
        public Vector2 Position;
        public float Radius => 20;

        public NodeDrawInfo(Vector2 postion)
        {
            Position = postion;
        }
    }
    
    public class NodeDrawerSystem
    {
        public void Draw(IEnumerable<NodeDrawInfo> drawInfos, IEnumerable<NodeDrawInfo> selectedNodes)
        {
            foreach (var drawInfo in drawInfos)
            {
                Handles.DrawSolidDisc(DrawingContext.Current.ApplyScroll(drawInfo.Position), Vector3.forward, drawInfo.Radius);
            }
        }
    }
    
    public class GraphContext
    {
        #region static

        public static GraphContext Current 
        { 
            get; 
            set; 
        }

        #endregion
        
        
        private readonly Graph _graph;
        
        private Dictionary<int, NodeDrawInfo> _nodeDrawInfos;

        private NodeDrawerSystem _nodeDrawerSystem;

        public string Name
        {
            get; 
            set;
        }

        public GraphContext(Graph graph)
        {
            _graph = graph;
            Initialize();
        }
        
        public GraphContext()
        {
            _graph = new Graph();
            Initialize();
        }

        private void Initialize()
        {
            UpdateDrawInfos();
            _nodeDrawerSystem = new NodeDrawerSystem();
        }

        public void AddNode(Vector2 position)
        {
            var node = _graph.AddNode();
            _nodeDrawInfos.Add(node.Id, new NodeDrawInfo(position));            
        }

        public void Draw()
        {
            _nodeDrawerSystem.Draw(_nodeDrawInfos.Values, null);
        }

        private void UpdateDrawInfos()
        {
            _nodeDrawInfos = new Dictionary<int, NodeDrawInfo>();
            for (var i = 0; i < _graph.Nodes.Count; i++)
            {
                var node = _graph.Nodes[i];
                _nodeDrawInfos[node.Id] = new NodeDrawInfo(50 * i * Vector2.one);
            }
        }
    }
}