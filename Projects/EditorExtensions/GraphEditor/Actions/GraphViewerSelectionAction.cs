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
           /* var context = FormationsEditorContext.Instance;

            var evt = Event.current;

            if (evt.alt || evt.button != 0)
            {
                return false;
            }

            EditorSpawnPoint spawnPointByPosition = null;
            if (evt.type == EventType.MouseDown)
            {
                spawnPointByPosition = context.CurrentFormation.FindSpawnPointByPosition(evt.mousePosition);
                if (spawnPointByPosition == null)
                {
                    HandleUtility.Repaint();
                }
                else
                {
                    if (spawnPointByPosition.LastEventType == EventType.MouseDown)
                    {
                        if (evt.control)
                        {
                            context.CurrentFormation.SelectSpawnPoint(spawnPointByPosition);
                            HandleUtility.Repaint();
                            return true;
                        }

                        var selectedDrawersCount = context.CurrentFormation.SelectedSpawnPoints.Count;
                        if (selectedDrawersCount < 1 || !spawnPointByPosition.IsSelected)
                        {
                            context.CurrentFormation.CleanUpSelection();
                            context.CurrentFormation.SelectSpawnPoint(spawnPointByPosition);
                            HandleUtility.Repaint();
                            return true;
                        }
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

            if (evt.type == EventType.MouseDown && spawnPointByPosition == null)
            {
                _selecting = false;
                _wasMouseDown = true;
                _selectionRect.x = evt.mousePosition.x;
                _selectionRect.y = evt.mousePosition.y;
                return false;
            }

            bool isContainInBattleRect = false;
            if (context.IsArenaMap)
            {
                var arenaBattleSizeRect = ((ArenaMapEditorContext) context).ArenaBattleSizeRect;
                var rect = new Rect(arenaBattleSizeRect.MapScreenRect.x - context.Scroll.x - ArenaMapResizeAction.ResizeStartZone,
                    arenaBattleSizeRect.MapScreenRect.y - ArenaMapResizeAction.ResizeStartZone - context.Scroll.y,
                    arenaBattleSizeRect.MapScreenRect.width + ArenaMapResizeAction.ResizeStartZone * 2,
                    arenaBattleSizeRect.MapScreenRect.height + ArenaMapResizeAction.ResizeStartZone * 2);
                isContainInBattleRect = rect.Contains(evt.mousePosition);
            }
            

            if (evt.type == EventType.MouseDrag && _wasMouseDown && !isContainInBattleRect)
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

            SelectSpawnPoints(context, _selectionRect, context.Scroll);

            HandleUtility.Repaint();
            return true;*/
            return false;
        }
        
/*
        public void SelectSpawnPoints(FormationsEditorContext context, Rect selection, Vector2 scroll)
        {
            selection.x += scroll.x;
            selection.y += scroll.y;
            var selectedSpawnPoints = new HashSet<EditorSpawnPoint>();

            foreach (var spawnPoint in context.CurrentFormation.SpawnPoints)
            {
                if (selection.Overlaps(spawnPoint.Rect, true))
                {
                    selectedSpawnPoints.Add(spawnPoint);
                }   
            }

            context.CurrentFormation.SelectSpawnPoints(selectedSpawnPoints);
        }

        private static void DrawRectBorders(Rect rect, Color color)
        {
            Handles.color = color;
            Handles.DrawLine(rect.min, new Vector2(rect.xMax, rect.yMin));
            Handles.DrawLine(rect.max, new Vector2(rect.xMin, rect.yMax));
            Handles.DrawLine(rect.min, new Vector2(rect.xMin, rect.yMax));
            Handles.DrawLine(rect.max, new Vector2(rect.xMax, rect.yMin));
        }*/
    }
}