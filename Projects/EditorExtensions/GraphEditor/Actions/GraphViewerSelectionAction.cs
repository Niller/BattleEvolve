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
            var graphDrawerSystem = GraphContext.Current.GraphDrawerSystem;

            var evt = Event.current;

            if (evt.alt || evt.button != 0) return false;

            bool isNodeUnderPoint = false;
            if (evt.type == EventType.MouseDown)
            {
                NodeDrawInfo nodeUnderPoint;
                if (!graphDrawerSystem.GetNodeDrawInfoByPosition(drawingContext.GetMousePosition(), out nodeUnderPoint))
                {
                    GraphEditorWindow.NeedHandlesRepaint = true;
                    
                }
                else
                {
                    if (evt.control)
                    {
                        graphDrawerSystem.Select(nodeUnderPoint);
                        GraphEditorWindow.NeedHandlesRepaint = true;
                        return true;
                    }

                    if (!graphDrawerSystem.SelectedNodes.Contains(nodeUnderPoint))
                    {
                        graphDrawerSystem.CleanUpSelection();
                        graphDrawerSystem.Select(nodeUnderPoint);
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

            SelectNodesByRect(graphDrawerSystem, _selectionRect, drawingContext.Scroll);

            HandleUtility.Repaint();
            return true;
        }

        private void SelectNodesByRect(GraphDrawerSystem graphDrawerSystem, Rect selection, Vector2 scroll)
        {
            selection.position -= scroll;
            var nodesUnderRect = graphDrawerSystem.GetNodeDrawInfoByRect(selection);
            graphDrawerSystem.Select(nodesUnderRect);
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