using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using EditorExtensions.GraphEditor.Drawing;
using EditorExtensions.Utilities;
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
        private string _currentPath;
        
        public IGraphDrawerSystem GraphDrawerSystem
        {
            get;
            private set;
        }
        
        public string Name
        {
            get; 
            set;
        }

        public string CurrentPath
        {
            get
            {
                return _currentPath; 
                
            }
            set
            {
                _currentPath = value;
                Name = Path.GetFileNameWithoutExtension(_currentPath);
            }
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
            GraphDrawerSystem = new GraphDrawerSystem();
            GraphDrawerSystem.UpdateDrawInfos(_graph);
        }
        
        public void AddNode(Vector2 position)
        {
            var node = _graph.AddNode();
            GraphDrawerSystem.AddNode(node, position);            
        }

        public void AddArc(Node nodeFrom, Node nodeTo)
        {
            _graph.AddArc(nodeFrom, nodeTo);
        }

        public void Draw()
        {
            GraphDrawerSystem.DrawArcs(_graph.Arcs);
            GraphDrawerSystem.DrawNodes();
        }

        public void RemoveSelected()
        {
            if (GraphDrawerSystem.SelectedArc != null)
            {
                _graph.RemoveArc(GraphDrawerSystem.SelectedArc);
                var oppositeArc = GraphDrawerSystem.SelectedArc.To.GetArcTo(GraphDrawerSystem.SelectedArc.From);
                if (oppositeArc != null)
                {
                    _graph.RemoveArc(oppositeArc);
                }
                GraphDrawerSystem.DeselectArc();
            }

            if (GraphDrawerSystem.SelectedNodes.Count > 0)
            {
                foreach (var selectedNode in GraphDrawerSystem.SelectedNodes)
                {
                    var node = GraphDrawerSystem.GetNode(selectedNode);
                    _graph.RemoveNode(node);
                    GraphDrawerSystem.RemoveNode(node);
                }
                GraphDrawerSystem.CleanUpSelection();
            } 
        }

        public void Save()
        {
            if (string.IsNullOrEmpty(CurrentPath))
            {
                throw new ArgumentException("Cannot save. Path is unknown");
            }
            
            IoUtilities.SaveToFile(CurrentPath, _graph);
        }
    }
}