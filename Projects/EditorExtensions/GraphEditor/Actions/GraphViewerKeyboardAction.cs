using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerKeyboardAction : IGraphViewerAction
    {
        public bool TryExecute()
        {
            var graphContext = GraphContext.Current;

            var evt = Event.current;
            if (evt.type != EventType.KeyUp)
            {
                return false;
            }

            if (evt.keyCode == KeyCode.Delete)
            {
                graphContext.RemoveSelected();
                GraphEditorWindow.NeedHandlesRepaint = true;
                return true;
            }

            return false;
        }
    }
}