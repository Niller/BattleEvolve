using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerSelectionAction : IGraphViewerAction
    {
        private bool _selecting;
        private Rect _selectionRect;
        private bool _canCreateSelectionBox;

        public bool TryExecute()
        {
            var drawingContext = DrawingContext.Current;
            var graphContext = GraphContext.Current;

            var evt = Event.current;

            if (evt.alt || evt.button != 0) return false;

            bool isNodeUnderPoint = false;
            if (evt.type == EventType.MouseDown)
            {
                NodeDrawInfo nodeUnderPoint;
                if (!graphContext.GetNodeDrawInfoByPosition(drawingContext.GetMousePosition(), out nodeUnderPoint))
                {
                    GraphEditorWindow.NeedHandlesRepaint = true;
                    
                }
                else
                {
                    if (evt.control)
                    {
                        graphContext.Select(nodeUnderPoint);
                        GraphEditorWindow.NeedHandlesRepaint = true;
                        return true;
                    }

                    if (!graphContext.SelectedNodes.Contains(nodeUnderPoint))
                    {
                        graphContext.CleanUpSelection();
                        graphContext.Select(nodeUnderPoint);
                        GraphEditorWindow.NeedHandlesRepaint = true;
                        return true;
                    }
                    
                    isNodeUnderPoint = true;
                }
            }

            if (evt.type == EventType.MouseUp)
            {
                _selecting = false;
                _canCreateSelectionBox = false;
                _selectionRect = new Rect();
                return false;
            }

            if (evt.type == EventType.MouseDown)
            {
                _selecting = false;
                _canCreateSelectionBox = !isNodeUnderPoint;
                _selectionRect.x = evt.mousePosition.x;
                _selectionRect.y = evt.mousePosition.y;
                return false;
            }

            if (evt.type == EventType.MouseDrag && _canCreateSelectionBox)
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

        private void SelectNodesByRect(GraphContext context, Rect selection, Vector2 scroll)
        {
            selection.position -= scroll;
            var nodesUnderRect = context.GetNodeDrawInfoByRect(selection);
            context.Select(nodesUnderRect);
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