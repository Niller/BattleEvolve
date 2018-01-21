using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerDragAction : IGraphViewerAction
    {
        private bool _isDragging;
        private Vector2 _dragDelta;
        //private List<EditorSpawnPoint> _draggedPoints = new List<EditorSpawnPoint>();
        private List<Vector2> _draggedPointsPositions = new List<Vector2>();

        public bool TryExecute()
        {
            /*
            var context = FormationsEditorContext.Instance;

            var evt = Event.current;
            if (evt.control || evt.alt)
            {
                _draggedPoints = null;
                _isDragging = false;
                return false;
            }

            if (evt.type == EventType.MouseUp)
            {
                _dragDelta = Vector2.zero;
                _draggedPointsPositions.Clear();
                _draggedPoints = null;
                _isDragging = false;
            }

            if (!evt.alt && evt.button == 0 && evt.type == EventType.MouseDown)
            {
                _dragDelta = Vector2.zero;
                _draggedPointsPositions.Clear();
                _isDragging = true;
                var spawnPointByPosition = context.CurrentFormation.FindSpawnPointByPosition(evt.mousePosition);
                if (spawnPointByPosition == null)
                {
                    var selectedDrawersCount = context.CurrentFormation.SelectedSpawnPoints.Count;
                    if (selectedDrawersCount > 0)
                    {
                        context.CurrentFormation.CleanUpSelection();
                        HandleUtility.Repaint();
                    }

                    _draggedPoints = null;
                    _isDragging = false;
                    return false;
                }
            }

            if (evt.button == 0 && evt.type == EventType.MouseDrag)
            {

                if (!_isDragging)
                {
                    var spawnPointByPosition = context.CurrentFormation.FindSpawnPointByPosition(evt.mousePosition);
                    if (spawnPointByPosition == null)
                    {
                        return false;
                    }
                    _isDragging = true;
                }

                if (_draggedPoints == null)
                {
                    _draggedPoints = context.CurrentFormation.SelectedSpawnPoints.ToList();
                }

                if (_draggedPointsPositions == null || _draggedPointsPositions.Count != _draggedPoints.Count)
                {
                    _draggedPointsPositions = _draggedPoints
                        .Select(drawer => new Vector2(drawer.Rect.x, drawer.Rect.y)).ToList();
                }

                var len = _draggedPoints.Count;
                if (len > 0)
                {
                    _dragDelta += evt.delta;

                    var delta = _dragDelta;

                    for (var i = 0; i < len; ++i)
                    {
                        
                        if (evt.shift)
                        {
                            _draggedPoints[i].ScreenPoint += evt.delta;
                            _dragDelta = Vector2.zero;
                            _draggedPointsPositions.Clear();
                        }
                        else
                        {
                            var pos = new Vector2(_draggedPointsPositions[i].x + delta.x,
                                _draggedPointsPositions[i].y + delta.y);
                            _draggedPoints[i].Align(pos);
                        }
                        
                    }

                    HandleUtility.Repaint();
                    return true;
                }
            }

            if (_dragDelta != Vector2.zero)
            {
                EditorGUIUtility.AddCursorRect(context.Viewport, MouseCursor.MoveArrow);
            }

            if (evt.button == 0 && evt.type == EventType.DragExited)
            {
                _dragDelta = Vector2.zero;
                _draggedPoints = context.CurrentFormation.SelectedSpawnPoints.ToList();
            }

            _draggedPoints = null;
            */
            return false;
        }

        /*
        public static Rect CalculateDragArea(List<EditorSpawnPoint> drawers, List<Vector2> drawersPositions)
        {
            var min = new Vector2(float.MaxValue, float.MaxValue);
            var max = Vector2.zero;

            var len = drawers.Count;
            for (var i = 0; i < len; i++)
            {
                var nodeDrawer = drawers[i];
                var drawersPosition = drawersPositions[i];
                min = Vector2.Min(min, new Vector2(drawersPosition.x, drawersPosition.y));
                max = Vector2.Max(max, new Vector2(drawersPosition.x + nodeDrawer.Rect.width
                    , drawersPosition.y + nodeDrawer.Rect.height));
            }

            return new Rect(min.x, min.y, max.x - min.x, max.y - min.y);
        }*/
    }
}