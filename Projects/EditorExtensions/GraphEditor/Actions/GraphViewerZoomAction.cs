using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerZoomAction : IGraphViewerAction
    {
        private float _zoomSpeed = 0.03f;

        public bool TryExecute()
        {
            var context = DrawingContext.Current;
            
            var evt = Event.current;

            if (evt.type == EventType.ScrollWheel)
            {
                context.Zoom = Mathf.Clamp(context.Zoom - _zoomSpeed*Event.current.delta.y, 0.3f, 3f);
                return true;
            }
            return false;
        }
    }
}