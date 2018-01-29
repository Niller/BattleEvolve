using System.Collections.Generic;
using Graphs;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Drawing
{
    public interface IGraphDrawerSystem
    {
        IEnumerable<INodeDrawInfo> Nodes { get; }
        Arc SelectedArc { get; }
        HashSet<INodeDrawInfo> SelectedNodes { get; }
        void DrawNodes();
        void UpdateDrawInfos(Graph graph);
        void AddNode(Node node, Vector2 position);
        void RemoveNode(Node node);
        Node GetNode(INodeDrawInfo nodeDrawInfo);
        bool GetNodeDrawInfoByPosition(Vector2 position, out INodeDrawInfo drawInfo);
        HashSet<INodeDrawInfo> GetNodeDrawInfoByRect(Rect rect);
        bool SelectArcByPostion(Vector2 position);
        void Select(INodeDrawInfo node);
        void Deselect(INodeDrawInfo node);
        void CleanUpSelection();
        void DeselectArc();
        void DrawArcs(IEnumerable<Arc> arcs);
        void DrawLoop(INodeDrawInfo node, bool isSelected);
        void DrawArc(INodeDrawInfo from, Vector2 to, bool isSelected, bool isTwoDirectional = false);
        void DrawArc(INodeDrawInfo from, INodeDrawInfo to, bool isSelected, bool isTwoDirectional = false);
        void Layout();
        void Select(HashSet<INodeDrawInfo> nodes);
    }
}