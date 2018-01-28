using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using EditorExtensions.Controls;
using EditorExtensions.Utilities;
using Graphs;
using JetBrains.Annotations;
using UnityEditor;
using UnityEditorInternal;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class GraphEditorWindow : EditorWindow
    {
        #region static

        private static GraphEditorWindow _window;
        
        [MenuItem("BattleEvolve/Graph editor")]
        public static void OpenWindow()
        {
            _window = GetWindow<GraphEditorWindow>();
            _window.titleContent = new GUIContent("Graph editor", BuiltInResources.FindIcon("d_PreTextureMipMapLow"));
            _window.minSize = new Vector2(500, 500);
            _window.Show();
        }

        public static bool NeedRepaint
        {
            get; 
            set;
        }
        
        public static bool NeedHandlesRepaint
        {
            get; 
            set;
        }

        #endregion static

        private const int TopPanelHeightConst = 17;
        private const int TopTabsPanelHeightConst = 27;
        private const string GraphsDefaultPath = "Assets/Resources/Data/Graphs/";

        private readonly List<GraphContext> _openedGraphs = new List<GraphContext>();

        private int _currentGraphIndex;

        private GraphViewer _graphViewer;
        private Tabs _graphTabs;
        
        [UsedImplicitly]
        private void OnEnable()
        {
            _openedGraphs.Add(new GraphContext()
            {
                Name = "New graph"
            });

            _currentGraphIndex = 0;
            
            wantsMouseEnterLeaveWindow = true;
            
            DrawingContext.Create();
            SetCurrentGraphContext(0);
            DrawingContext.SwitchContext();
            
            _graphViewer = new GraphViewer();
            
            _graphTabs = new Tabs(_openedGraphs.Count);
            _graphTabs.OnSelectionChange += OnTabSelectionChange;
            _graphTabs.OnTabRemove += OnTabRemove;
            _graphTabs.OnTabAddClick += OnTabAddClick;
        }

        private void OnTabAddClick()
        {
            AddGraphContext(new GraphContext()
            {
                Name = "new Graph" + _openedGraphs.Count
            });
        }

        private void AddGraphContext(GraphContext graphContext)
        {
            _openedGraphs.Add(graphContext);
            _graphTabs.AddTab();
            NeedRepaint = true;
        }

        private void OnTabRemove(int i)
        {
            _openedGraphs.RemoveAt(i);
            NeedRepaint = true;
        }

        private void OnTabSelectionChange(int i)
        {
            SetCurrentGraphContext(i);
            NeedRepaint = true;
        }

        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            DrawTopPanel();
            DrawTabs();
            DrawGraphViewer();

            if (NeedRepaint)
            {
                Repaint();
                NeedRepaint = false;
            }

            if (NeedHandlesRepaint)
            {
                HandleUtility.Repaint();
                NeedHandlesRepaint = false;
            }
        }

        private void DrawGraphViewer()
        {
            var mainRect = new Rect(0, TopPanelHeightConst+TopTabsPanelHeightConst, position.width, position.height - TopPanelHeightConst-TopTabsPanelHeightConst);
            GUILayout.BeginArea(mainRect);
            DrawingContext.Current.Viewport = mainRect;
            _graphViewer.DoLayout(new Rect(0, 0, position.width, position.height - TopPanelHeightConst-TopTabsPanelHeightConst), _openedGraphs[_currentGraphIndex]);
            GUILayout.EndArea();
        }

        private void DrawTabs()
        {
            var mainRect = new Rect(0, TopPanelHeightConst, position.width, TopTabsPanelHeightConst);
            var backgroundTexture = new Texture2D(1, 1);
            BuiltInResources.FillUsingDefaultColor(backgroundTexture, EditorGUIUtility.isProSkin);
            GUI.DrawTexture(mainRect, backgroundTexture);
            
            _graphTabs.DoLayout(mainRect, _openedGraphs.Select(g => g.Name).ToList());
        }

        private void SetCurrentGraphContext(int index)
        {
            _currentGraphIndex = index;
            GraphContext.Current = _openedGraphs[index];
            DrawingContext.SwitchContext();
        }

        private void DrawTopPanel()
        {
            var graphContext = GraphContext.Current;
            
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Open", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    OpenGraph();
                    NeedRepaint = true;
                }

                if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    if (string.IsNullOrEmpty(graphContext.CurrentPath))
                    {
                        SaveGraphAs(graphContext);
                    }
                    else
                    {
                        SaveGraph(graphContext);
                    }
                }

                if (GUILayout.Button("Save As", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    SaveGraphAs(graphContext);
                }
            
                EditorGUILayout.EndHorizontal();
            }
        }

        private void OpenGraph()
        {
            var path = GraphsDefaultPath;

            path = EditorUtility.OpenFilePanel("Open Graph", Directory.GetParent(path).FullName, "bytes");
            if (string.IsNullOrEmpty(path))
                return;
            if (!File.Exists(path))
            {
                EditorUtility.DisplayDialog("File not exists",
                    "Selected file not exists, you must select Graph binary file.", "Ok");
                return;
            }

            var graph = IoUtilities.LoadFromFile<Graph>(path, Graph.Signature);
            AddGraphContext(new GraphContext(graph)
            {
                Name = Path.GetFileNameWithoutExtension(path),
                CurrentPath = path,
            });     
        }

        private void SaveGraphAs(GraphContext graphContext)
        {
            var path = GraphsDefaultPath;
            path = EditorUtility.SaveFilePanel("Save Graph",
                Directory.GetParent(path).FullName, "Graph", "bytes");

            if (string.IsNullOrEmpty(path))
            {
                return;
            }

            graphContext.CurrentPath = path;
            
            SaveGraph(graphContext);
        }

        private void SaveGraph(GraphContext graphContext)
        {
            graphContext.Save();
            
            AssetDatabase.Refresh();
        }
    }
}