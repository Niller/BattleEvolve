using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerScrollAction : IGraphViewerAction
    {
        private bool _isScrolling;
        private const float Speed = 1f;

        public bool TryExecute()
        {            
            var context = DrawingContext.Current;

            var evt = Event.current;
            if (evt.type == EventType.MouseDrag)
            {
                if (!_isScrolling)
                {
                    _isScrolling = evt.alt && evt.button == 0 || evt.button == 2;
                    //DragAndDrop.PrepareStartDrag();
                    return true;
                }

                context.Scroll += evt.delta*Speed;
                GraphEditorWindow.NeedHandlesRepaint = true;
                return true;
            }
                        
            if (Event.current.type == EventType.MouseUp || Event.current.type == EventType.MouseLeaveWindow)
            {
                _isScrolling = false;
            }

            if (_isScrolling)
            {
                return true;
            }

            return false;
        }      
    }
}