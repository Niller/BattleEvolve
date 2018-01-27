using System.Collections.Generic;
using Graphs;
using UnityEngine.Assertions.Must;

namespace EditorExtensions.GraphEditor
{
    public interface IGraphLayoutSystem
    {
        void Layout(Dictionary<Node, NodeDrawInfo> nodes);
    }
}