using EditorExtensions.Controls;
using EditorExtensions.GraphEditor.Actions;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class GraphViewer
    {

        private readonly GridDrawer _gridDrawer;

        private readonly IGraphViewerAction[] _actions = 
        {
            new GraphViewerContextMenuAction(),
            new GraphViewerKeyboardAction(),
            new GraphViewerCreateArcAction(), 
            new GraphViewerNodeSelectionAction(),
            new GraphViewerArcSelectionAction(), 
            new GraphViewerDragAction(), 
            new GraphViewerScrollAction()             
        };
        
        public GraphViewer()
        {
            _gridDrawer = new GridDrawer
            {
                CellSize = 15
            };
        }
        
        public void DoLayout(Rect rect, GraphContext graphContext)
        {
            _gridDrawer.Draw(rect, -DrawingContext.Current.Scroll);

            graphContext.Draw();
            
            foreach (var action in _actions)
            {
                if (action.TryExecute())
                {
                    break;
                }
            }
        }
    }
}