using EditorExtensions.Controls;
using EditorExtensions.GraphEditor.Actions;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class GraphViewer
    {
        private const int BaseGridCellSize = 30;

        private readonly GridDrawer _gridDrawer;

        private readonly IGraphViewerAction[] _actions = 
        {
            new GraphViewerContextMenuAction(),
            new GraphViewerKeyboardAction(),
            new GraphViewerZoomAction(),
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
        
        public void DoLayout(Rect rect)
        {  
            _gridDrawer.CellSize = (rect.height/(BaseGridCellSize)*(2f*DrawingContext.Current.Zoom));
            _gridDrawer.Draw(rect, -DrawingContext.Current.Scroll, rect.center);
            
            DrawingContext.Current.Draw();
            
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