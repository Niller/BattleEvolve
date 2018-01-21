using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerContextMenuAction : IGraphViewerAction
    {
        private class OnNodeCreateClickArgs
        {
            public readonly Vector2 Position;

            public OnNodeCreateClickArgs(Vector2 position)
            {
                Position = position;
            }
        }
        
        public bool TryExecute()
        {
            var evt = Event.current;
            if (evt.type != EventType.ContextClick)
            {
                return false;
            }

            var menu = new GenericMenu();
            menu.AddItem(new GUIContent("Create node"), false, OnNodeCreateClick, new OnNodeCreateClickArgs(evt.mousePosition));
            
            menu.ShowAsContext();
            return true;
        }

        private void OnNodeCreateClick(object args)
        {
            GraphContext.Current.AddNode(((OnNodeCreateClickArgs)args).Position);
        }
    }
}