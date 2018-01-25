﻿using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.InteropServices;
using Graphs;
using UnityEngine;
using Utilities.Extensions;

namespace EditorExtensions.GraphEditor
{
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
        
        public HashSet<NodeDrawInfo> SelectedNodes = new HashSet<NodeDrawInfo>();

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
        
        private void UpdateDrawInfos()
        {
            _nodeDrawInfos = new Dictionary<int, NodeDrawInfo>();
            for (var i = 0; i < _graph.Nodes.Count; i++)
            {
                var node = _graph.Nodes[i];
                _nodeDrawInfos[node.Id] = new NodeDrawInfo(node.Id, 50 * i * Vector2.one);
            }
        }

        public void AddNode(Vector2 position)
        {
            var node = _graph.AddNode();
            _nodeDrawInfos.Add(node.Id, new NodeDrawInfo(node.Id, position));            
        }

        public void Draw()
        {
            _nodeDrawerSystem.Draw(_nodeDrawInfos.Values, SelectedNodes);
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
            return _nodeDrawInfos.Values.Where(n => rect.Contains(n.Position)).ToHashSet();
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
    }
}