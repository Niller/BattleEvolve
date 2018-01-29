using EditorExtensions.GraphEditor.Drawing;
using EditorExtensions.GraphEditor.Utilities;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Actions
{
    public class GraphViewerCreateArcAction : IGraphViewerAction
    {
        private bool _creatingArc;
        private INodeDrawInfo _nodeFrom;

        public bool TryExecute()
        {
            var drawingContext = DrawingContext.Current;
            var graphDrawerSystem = GraphContext.Current.GraphDrawerSystem;
            var evt = Event.current;

            if (evt.shift)
            {
                if (evt.type == EventType.mouseDown)
                {
                    INodeDrawInfo nodeDrawInfo;
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
                    INodeDrawInfo nodeDrawInfo;
                    if (graphDrawerSystem.GetNodeDrawInfoByPosition(drawingContext.GetMousePosition(), out nodeDrawInfo))
                    {
                        if (nodeDrawInfo == _nodeFrom)
                        {
                            graphDrawerSystem.DrawLoop(_nodeFrom, false);
                            GraphEditorWindow.NeedHandlesRepaint = true;
                            return true;
                        }
                    }

                    graphDrawerSystem.DrawArc(_nodeFrom, evt.mousePosition, false);
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
        
        private void TryCreateArc(IGraphDrawerSystem graphDrawerSystem, DrawingContext drawingContext)
        {
            var graphContext = GraphContext.Current;
            
            INodeDrawInfo nodeDrawInfo;
            if (graphDrawerSystem.GetNodeDrawInfoByPosition(drawingContext.GetMousePosition(), out nodeDrawInfo))
            {
                var nodeTo = nodeDrawInfo;
                graphContext.AddArc(graphDrawerSystem.GetNode(_nodeFrom), graphDrawerSystem.GetNode(nodeTo));
            }
        }
    }
}
