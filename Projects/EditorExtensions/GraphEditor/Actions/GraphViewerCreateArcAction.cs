using EditorExtensions.GraphEditor.Utilities;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerCreateArcAction : IGraphViewerAction
    {
        private bool _creatingArc;
        private NodeDrawInfo _nodeFrom;

        public bool TryExecute()
        {
            var drawingContext = DrawingContext.Current;
            var graphDrawerSystem = GraphContext.Current.GraphDrawerSystem;
            var evt = Event.current;

            if (evt.shift)
            {
                if (evt.type == EventType.mouseDown)
                {
                    NodeDrawInfo nodeDrawInfo;
                    if (graphDrawerSystem.GetNodeDrawInfoByPosition(drawingContext.GetMousePosition(), out nodeDrawInfo))
                    {
                        _creatingArc = true;
                        _nodeFrom = nodeDrawInfo;
                        return true;
                    }
                }
                
                if (evt.type == EventType.MouseUp && _creatingArc)
                {
                    TryCreateArc(graphDrawerSystem, drawingContext);
                    _creatingArc = false;
                    return true;
                }

                if (_creatingArc)
                {
                    NodeDrawInfo nodeDrawInfo;
                    if (graphDrawerSystem.GetNodeDrawInfoByPosition(drawingContext.GetMousePosition(), out nodeDrawInfo))
                    {
                        if (nodeDrawInfo == _nodeFrom)
                        {
                            DrawUtilities.DrawLoop(drawingContext.ApplyScroll(_nodeFrom.Position), _nodeFrom.Radius, Color.white, drawingContext.Zoom);
                            GraphEditorWindow.NeedHandlesRepaint = true;
                            return true;
                        }
                    }

                    DrawUtilities.DrawDirectionalLine(drawingContext.ApplyScroll(_nodeFrom.Position), evt.mousePosition, _nodeFrom.Radius, Color.white, drawingContext.Zoom);
                    GraphEditorWindow.NeedHandlesRepaint = true;
                    return true;
                }
            }
            else
            {
                if (_creatingArc)
                {
                    TryCreateArc(graphDrawerSystem, drawingContext);
                    _creatingArc = false;
                    return true;
                }
            }

            _creatingArc = false;
            return false;
        }
        
        private void TryCreateArc(GraphDrawerSystem graphDrawerSystem, DrawingContext drawingContext)
        {
            var graphContext = GraphContext.Current;
            
            NodeDrawInfo nodeDrawInfo;
            if (graphDrawerSystem.GetNodeDrawInfoByPosition(drawingContext.GetMousePosition(), out nodeDrawInfo))
            {
                var nodeTo = nodeDrawInfo;
                graphContext.AddArc(_nodeFrom, nodeTo);
            }
        }
    }
}
