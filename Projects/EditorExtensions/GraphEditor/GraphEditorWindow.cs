using System;
using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using System.Runtime.InteropServices;
using System.Security.Cryptography.X509Certificates;
using EditorExtensions.Controls;
using EditorExtensions.Utilities;
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
        public static void Open()
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

        #endregion static

        private const int TopPanelHeightConst = 17;
        private const int TopTabsPanelHeightConst = 27;
        
        private readonly List<GraphContext> _openedGraphs = new List<GraphContext>();

        private int _currentGraphIndex;

        private GraphViewer _graphViewer;
        private Tabs _graphTabs;

        public GraphEditorWindow()
        {
            _openedGraphs.Add(new GraphContext()
            {
                Name = "Graph1"
            });
            _openedGraphs.Add(new GraphContext()
            {
                Name = "Graph2"
            });
            _openedGraphs.Add(new GraphContext()
            {
                Name = "Graph3"
            });
            _openedGraphs.Add(new GraphContext()
            {
                Name = "Graph4"
            });

            _currentGraphIndex = 0;
        }
        
        [UsedImplicitly]
        private void OnEnable()
        {
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
            _openedGraphs.Add(new GraphContext()
            {
                Name = "new Graph" + _openedGraphs.Count
            });
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
            EditorGUILayout.BeginHorizontal(EditorStyles.toolbar);
            {
                GUILayout.FlexibleSpace();
                if (GUILayout.Button("Create", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    
                    if (EditorUtility.DisplayDialog("Unsaved changes", "You may lost your unsaved changes. Do you want to save them?",
                        "Yes", "No"))
                    {
                    }
                    Repaint();
                }

                if (GUILayout.Button("Open", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                    Repaint();
                }

                if (GUILayout.Button("Save", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                }

                if (GUILayout.Button("Save As", EditorStyles.toolbarButton, GUILayout.Width(50)))
                {
                }
            
                EditorGUILayout.EndHorizontal();
            }
        }
    }
}