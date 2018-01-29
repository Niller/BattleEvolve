using System.Collections.Generic;
using Graphs;

namespace EditorExtensions.GraphEditor.Drawing
{
    public interface IGraphLayoutSystem
    {
        void Layout(Dictionary<Node, NodeDrawInfo> nodes);
    }
}