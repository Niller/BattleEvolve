using System.Collections.Generic;
using System.Linq;
using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class NodeDrawerSystem
    {
        private const int SelectionRadius = 6;

        public void Draw(IEnumerable<NodeDrawInfo> drawInfos, HashSet<NodeDrawInfo> selectedNodes)
        {
            var defaultColor = Handles.color;
            foreach (var drawInfo in drawInfos)
            {
                var drawingPosition = DrawingContext.Current.ApplyScroll(drawInfo.Position);
                if (selectedNodes.Contains(drawInfo))
                {
                    Handles.color = Color.yellow;;
                    Handles.DrawSolidDisc(drawingPosition, Vector3.forward, drawInfo.Radius + SelectionRadius);
                }
                Handles.color = Color.red;
                Handles.DrawSolidDisc(drawingPosition, Vector3.forward, drawInfo.Radius);
                
            }
            Handles.color = defaultColor;
        }
    }
}