using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerSelectionAction : IGraphViewerAction
    {
        private bool _selecting;
        private Rect _selectionRect;
        private bool _wasMouseDown;

        public bool TryExecute()
        {
            var drawingContext = DrawingContext.Current;
            var graphContext = GraphContext.Current;

            var evt = Event.current;

            if (evt.alt || evt.button != 0)
            {
                return false;
            }

            var hasNodeUnderPoint = false;
            if (evt.type == EventType.MouseDown)
            {
                NodeDrawInfo nodeUnderPoint;
                if (!graphContext.GetNodeDrawInfoByPosition(drawingContext.ApplyScroll(evt.mousePosition), out nodeUnderPoint))
                {
                    HandleUtility.Repaint();
                }
                else
                {
                    hasNodeUnderPoint = true;
                    if (nodeUnderPoint.LastEventType == EventType.MouseDown)
                    {
                        if (evt.control)
                        {
                            graphContext.Select(nodeUnderPoint);
                            
                            return true;
                        }

                        /*
                        var selectedDrawersCount = drawingContext.CurrentFormation.SelectedSpawnPoints.Count;
                        if (selectedDrawersCount < 1 || !nodeUnderPoint.IsSelected)
                        {
                            drawingContext.CurrentFormation.CleanUpSelection();
                            drawingContext.CurrentFormation.SelectSpawnPoint(nodeUnderPoint);
                            HandleUtility.Repaint();
                            return true;
                        }
                        */
                        
                        graphContext.CleanUpSelection();
                        graphContext.Select(nodeUnderPoint);
                        GraphEditorWindow.NeedHandlesRepaint = true;
                        return true;

                    }
                }
            }

            if (evt.type == EventType.MouseUp)
            {
                _selecting = false;
                _wasMouseDown = false;
                _selectionRect = new Rect();
                return false;
            }

            if (evt.type == EventType.MouseDown && !hasNodeUnderPoint)
            {
                _selecting = false;
                _wasMouseDown = true;
                _selectionRect.x = evt.mousePosition.x;
                _selectionRect.y = evt.mousePosition.y;
                return false;
            }           

            if (evt.type == EventType.MouseDrag && _wasMouseDown)
            {
                _selecting = true;
            }

            if (!_selecting)
            {
                return false;
            }

            _selectionRect.width = evt.mousePosition.x - _selectionRect.x;
            _selectionRect.height = evt.mousePosition.y - _selectionRect.y;
            DrawRectBorders(_selectionRect, EditorGUIUtility.isProSkin ? Color.white : Color.black);

            SelectNodesByRect(graphContext, _selectionRect, drawingContext.Scroll);

            HandleUtility.Repaint();
            return true;
        }
        
        public void SelectNodesByRect(GraphContext context, Rect selection, Vector2 scroll)
        {
            selection.x += scroll.x;
            selection.y += scroll.y;
            var nodeUnderRect = context.GetNodeDrawInfoByRect(selection);
            context.Select(nodeUnderRect);
        }

        private static void DrawRectBorders(Rect rect, Color color)
        {
            Handles.color = color;
            Handles.DrawLine(rect.min, new Vector2(rect.xMax, rect.yMin));
            Handles.DrawLine(rect.max, new Vector2(rect.xMin, rect.yMax));
            Handles.DrawLine(rect.min, new Vector2(rect.xMin, rect.yMax));
            Handles.DrawLine(rect.max, new Vector2(rect.xMax, rect.yMin));
        }
        
        
    }
}