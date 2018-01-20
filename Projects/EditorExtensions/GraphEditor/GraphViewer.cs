using EditorExtensions.Controls;
using EditorExtensions.GraphEditor.Actions;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class GraphViewer
    {

        private readonly GridDrawer _gridDrawer;

        private readonly IGraphViewerAction[] _actions = new IGraphViewerAction[]
        {

        };
        
        public GraphViewer()
        {
            _gridDrawer = new GridDrawer
            {
                CellSize = 15
            };
        }
        
        public void DoLayout(Rect rect)
        {
            _gridDrawer.Draw(rect, DrawingContext.Instance.Scroll);

            foreach (var action in _actions)
            {
                if (action.Execute())
                {
                    break;
                }
            }
        }
    }
}