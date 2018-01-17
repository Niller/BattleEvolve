using EditorExtensions.Controls;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class GraphViewer
    {

        private readonly GridDrawer _gridDrawer;
        
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
        }
    }
}