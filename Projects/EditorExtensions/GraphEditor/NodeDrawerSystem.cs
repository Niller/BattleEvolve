using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class NodeDrawerSystem
    {
        private const int SelectionRadius = 6;

        public void Draw(IEnumerable<NodeDrawInfo> drawInfos, HashSet<int> selectedNodes)
        {
            foreach (var drawInfo in drawInfos)
            {
                var drawingPosition = DrawingContext.Current.ApplyScroll(drawInfo.Position);
                if (selectedNodes.Contains(drawInfo.Id))
                {
                    Handles.DrawSolidDisc(drawingPosition, Vector3.forward, drawInfo.Radius + SelectionRadius);
                }
                Handles.DrawSolidDisc(drawingPosition, Vector3.forward, drawInfo.Radius);
                
            }
        }
    }
}