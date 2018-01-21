using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerKeyboardAction : IGraphViewerAction
    {
        public bool TryExecute()
        {
            //var context = FormationsEditorContext.Instance;

            var evt = Event.current;
            if (evt.type != EventType.KeyUp)
            {
                return false;
            }

            if (evt.keyCode == KeyCode.Delete)
            {
                //context.CurrentFormation.RemoveSelectedSpawnPoints();
                return true;
            }

            if (evt.control && evt.keyCode == KeyCode.D)
            {
                //context.CurrentFormation.DublicateSelectedSpawnPoints();
                return true;
            }

            return false;
        }
    }
}