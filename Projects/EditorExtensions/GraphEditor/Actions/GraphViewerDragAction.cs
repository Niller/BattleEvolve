﻿using System.Collections.Generic;
using System.Linq;
using EditorExtensions.GraphEditor.Drawing;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerDragAction : IGraphViewerAction
    {
        private bool _isDragging;
        private Vector2 _dragDelta;
        
        public bool TryExecute()
        {
            
            var drawingContext = DrawingContext.Current;
            var graphDrawerSystem = GraphContext.Current.GraphDrawerSystem;

            var evt = Event.current;
            if (evt.control || evt.alt)
            {
                _isDragging = false;
                return false;
            }

            if (evt.type == EventType.MouseUp)
            {
                _dragDelta = Vector2.zero;
                _isDragging = false;
            }

            if (!evt.alt && evt.button == 0 && evt.type == EventType.MouseDown)
            {
                _dragDelta = Vector2.zero;
                _isDragging = true;
                INodeDrawInfo nodeByPosition;
                if (!graphDrawerSystem.GetNodeDrawInfoByPosition(drawingContext.GetMousePosition(), out nodeByPosition))
                {
                    graphDrawerSystem.CleanUpSelection();
                    GraphEditorWindow.NeedHandlesRepaint = true;

                    _isDragging = false;
                    return false;
                }
            }

            if (evt.button == 0 && evt.type == EventType.MouseDrag)
            {

                if (!_isDragging)
                {
                    INodeDrawInfo nodeByPosition;
                    if (!graphDrawerSystem.GetNodeDrawInfoByPosition(drawingContext.GetMousePosition(), out nodeByPosition))
                    {
                        return false;
                    }
                    _isDragging = true;
                }

                foreach (var node in graphDrawerSystem.SelectedNodes)
                {

                    node.Position = node.Position + evt.delta;
                    _dragDelta = Vector2.zero;
                    GraphEditorWindow.NeedHandlesRepaint = true;
                }
                return true;
            }

            if (_dragDelta != Vector2.zero)
            {
                EditorGUIUtility.AddCursorRect(drawingContext.Viewport, MouseCursor.MoveArrow);
            }
            
            return false;
        }
    }
}