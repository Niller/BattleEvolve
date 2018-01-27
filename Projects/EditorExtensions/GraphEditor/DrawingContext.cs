using System;
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

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

        /*public DrawingContext()
        {
            
        }*/

        private Vector2 _scroll;
        
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
                //LimitScroll();
                //HandleUtility.Repaint();
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
            
        }
        
        private void LimitScroll()
        {
            _scroll.x = Mathf.Min(_scroll.x, 0f);
            _scroll.y = Mathf.Min(_scroll.y, 0f);
        }
        
        
    }
}