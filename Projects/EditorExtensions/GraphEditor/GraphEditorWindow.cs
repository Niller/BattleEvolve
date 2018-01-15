using System.Collections.Generic;
using System.Linq;
using System.Linq.Expressions;
using EditorExtensions.Utilities;
using JetBrains.Annotations;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class GraphContext
    {
        public string Name
        {
            get; 
            set;
        }
    }
    
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

        #endregion static

        private const int TopPanelHeightConst = 17;
        
        private readonly List<GraphContext> _openedGraphs = new List<GraphContext>();

        private int _currentGraphIndex;

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
        }
        
        [UsedImplicitly]
        // ReSharper disable once InconsistentNaming
        private void OnGUI()
        {
            DrawTopPanel();
            DrawBackground();
            DrawTabs();
        }

        private void DrawBackground()
        {
            var mainRect = new Rect(0, TopPanelHeightConst, position.width, position.height - TopPanelHeightConst);
            var backgroundTexture = new Texture2D(1, 1);
            BuiltInResources.FillUsingDefaultColor(backgroundTexture, EditorGUIUtility.isProSkin);
            GUI.DrawTexture(mainRect, backgroundTexture);
        }

        private void DrawTabs()
        {
            GUILayout.BeginHorizontal();
            _currentGraphIndex = GUILayout.Toolbar(_currentGraphIndex, _openedGraphs.Select(g => g.Name).ToArray());
            if (GUILayout.Button("+", GUILayout.Width(20f)))
            {
                
            }
            GUILayout.FlexibleSpace();
            GUILayout.EndHorizontal();
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