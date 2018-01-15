using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.Controls
{
    public class GridDrawer
    {
        private readonly List<Vector3> _gridLines = new List<Vector3>();
        private readonly List<Vector3> _gridGuideLines = new List<Vector3>();
        private Vector2 _lastScroll = new Vector2(float.MinValue, float.MinValue);
        private Rect _lastViewport = Rect.zero;
        private bool _dirty;
        private float _cellSize;
        private readonly Color _colorGridLines;
        private readonly Color _colorGuideLines;

        public float CellSize
        {
            get
            {
                return _cellSize;
            }
            set
            {
                _cellSize = value;
                _dirty = true;
            }
        }

        public GridDrawer()
        {
            CellSize = 5;
            _colorGridLines = new Color(0.5f, 0.5f, 0.5f, 0.2f);
            _colorGuideLines = new Color(0.5f, 0.5f, 0.5f, 0.5f);
        }

        public void Draw(Rect viewport, Vector2 scroll)
        {
            Draw(viewport, scroll, Vector2.zero);
        }
    
        public void Draw(Rect viewport, Vector2 scroll, Vector2 center)
        {
            if (Event.current.type != EventType.Repaint)
            {
                return;
            }

            if(scroll != _lastScroll || _lastViewport != viewport || _dirty)
            {
                _gridLines.Clear();
                _gridGuideLines.Clear();

                const int ge = 20;

                var gx = 0;
                var gy = 0;

                for (var x = -scroll.x + center.x; x < viewport.xMax; x += CellSize)
                {
                    _gridLines.Add(new Vector3(x, viewport.yMin, 0));
                    _gridLines.Add(new Vector3(x, viewport.yMax, 0));

                    if (gx++ % ge != 0)
                    {
                        continue;
                    }

                    _gridGuideLines.Add(new Vector3(x, viewport.yMin, 0));
                    _gridGuideLines.Add(new Vector3(x, viewport.yMax, 0));
                }       
                gx = 1;
                for (var x = -scroll.x - CellSize + center.x; x > viewport.xMin; x -= CellSize)
                {
                    _gridLines.Add(new Vector3(x, viewport.yMin, 0));
                    _gridLines.Add(new Vector3(x, viewport.yMax, 0));

                    if (gx++ % ge != 0)
                    {
                        continue;
                    }

                    _gridGuideLines.Add(new Vector3(x, viewport.yMin, 0));
                    _gridGuideLines.Add(new Vector3(x, viewport.yMax, 0));
                }

                for (var y = -scroll.y + center.y; y < viewport.yMax; y += CellSize)
                {
                    _gridLines.Add(new Vector3(viewport.xMin, y, 0));
                    _gridLines.Add(new Vector3(viewport.xMax, y, 0));

                    if (gy++ % ge != 0)
                    {
                        continue;
                    }

                    _gridLines.Add(new Vector3(viewport.xMin, y, 0));
                    _gridLines.Add(new Vector3(viewport.xMax, y, 0));
                }           
                gy = 1;
                for (var y = -scroll.y - CellSize + center.y; y > viewport.yMin; y -= CellSize)
                {
                    _gridLines.Add(new Vector3(viewport.xMin, y, 0));
                    _gridLines.Add(new Vector3(viewport.xMax, y, 0));

                    if (gy++ % ge != 0)
                    {
                        continue;
                    }

                    _gridLines.Add(new Vector3(viewport.xMin, y, 0));
                    _gridLines.Add(new Vector3(viewport.xMax, y, 0));
                }
                _lastScroll = scroll;
                _lastViewport = viewport;
                _dirty = false;
            }

            var color = Handles.color;
            Handles.color = _colorGridLines;
            Handles.DrawLines(_gridLines.ToArray());

            Handles.color = _colorGuideLines;
            Handles.DrawLines(_gridGuideLines.ToArray());
            Handles.color = color; 
        }
    }
}