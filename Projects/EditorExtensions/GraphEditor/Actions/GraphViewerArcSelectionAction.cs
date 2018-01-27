using System.Net;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerArcSelectionAction : IGraphViewerAction
    {
        public bool TryExecute()
        {
            var drawingContext = DrawingContext.Current;
            var graphContext = GraphContext.Current;
            var graphDrawerSystem = graphContext.GraphDrawerSystem;

            var evt = Event.current;

            if (evt.type == EventType.MouseUp)
            {
                
                if (graphDrawerSystem.SelectedNodes.Count == 0 && graphDrawerSystem.SelectArcByPostion(drawingContext.GetMousePosition()))
                {
                    GraphEditorWindow.NeedHandlesRepaint = true;
                    return true;
                }
                else
                {
                    graphDrawerSystem.DeselectArc();
                }
            }

            return false;
        }
    }
}