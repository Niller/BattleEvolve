using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;
using UnityEngine.VR.WSA;

namespace EditorExtensions.GraphEditor
{
    public class DrawingContext
    {
        #region static
        
        private static Dictionary<GraphContext, DrawingContext> _drawingContexts;

        private static DrawingContext _currentContext;
        
        public static DrawingContext Current => _currentContext;

        public static void SwitchContext()
        {
            var graphContext = GraphContext.Current;
            if (graphContext == null)
            {
                throw new Exception("Try to create drawing context while graph context is not created");
            }
            if (!_drawingContexts.TryGetValue(graphContext, out _currentContext))
            {
                _currentContext = new DrawingContext();
                _drawingContexts[graphContext] = _currentContext;
            }
        }

        public static void Create()
        {
            _drawingContexts = new Dictionary<GraphContext, DrawingContext>();
        }
        
        #endregion

        private Vector2 _scroll;
        private float _zoom = 1f;
        
        public Rect Viewport
        {
            get;
            set;
        }

        public Vector2 Scroll
        {
            get
            {
                return _scroll;
            }
            set
            {
                _scroll = value;
            }
        }

        public float Zoom
        {
            get
            {
                return _zoom;
            }
            set
            {
                var oldValue = _zoom;
                _zoom = value;
                ApplyZoom(oldValue, value);
                GraphEditorWindow.NeedHandlesRepaint = true;
            }
        }

        private void ApplyZoom(float oldZoom, float newZoom)
        {
            var deltaZoom = newZoom - oldZoom;
            var graphContext = GraphContext.Current;
            foreach (var node in graphContext.GraphDrawerSystem.Nodes)
            {
                node.Position += deltaZoom * (node.Position - (Viewport.center - _scroll)) / oldZoom;
            }
        }

        public Vector2 ApplyScroll(Vector2 postion)
        {
            return postion + Scroll;
        }
        
        /// <summary>
        /// Current mouse position with scroll
        /// </summary>
        /// <returns></returns>
        public Vector2 GetMousePosition()
        {
            return Event.current.mousePosition - Scroll;
        }

        public void Draw()
        {
            GraphContext.Current.Draw();
        }     
    }
}