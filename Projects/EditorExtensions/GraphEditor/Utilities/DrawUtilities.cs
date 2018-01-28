using UnityEditor;
using UnityEngine;

namespace EditorExtensions.GraphEditor.Utilities
{
    public static class DrawUtilities
    {
        private static Color _oldColor;
        
        private const float ArrowWidth = 5;
        private const float ArrowHeight = 12;
        
        public static void DrawDirectionalLine(Vector2 from, Vector2 to, int deltaLength, Color color, float zoom = 1f, bool twoDirections = false)
        {
            Vector3 forward = (to - from).normalized;
            Vector3 backward = (from - to).normalized;
            
            Vector3[] arrowLine = new Vector3[2];
            Vector3 start = (Vector3)from + forward * deltaLength;
            Vector3 end = (Vector3) to - forward * deltaLength;
            
            arrowLine[0] = start;
            arrowLine[1] = end;
            
            Handles.BeginGUI(); 
            {
                BeginHandlesDrawColor(color);
                {
                    Handles.DrawAAPolyLine(arrowLine);
                    Handles.DrawAAConvexPolygon(GetArrowHead(end, forward, zoom));
                    if (twoDirections)
                    {
                        Handles.DrawAAConvexPolygon(GetArrowHead(start, backward, zoom));
                    }
                }
                EndHandlesDrawColor();
            }
            Handles.EndGUI();
        }

        public static void DrawLoop(Vector2 fromCenter, int deltaLength, Color color, float zoom = 1f)
        {
            const int bezierLineWidth = 2;
            const int bezierDivision = 20;
            
            Vector3 fromDirection = Vector3.down;
            Vector3 toDirection = Vector3.right;;

            var from = (Vector3)fromCenter + fromDirection * deltaLength;
            var to = (Vector3)fromCenter + toDirection * deltaLength;

            var tangentDistance = deltaLength * 4;
            
            var startTangent = from + fromDirection * tangentDistance;
            var endTangent = from + toDirection * tangentDistance;
            
            Handles.BeginGUI(); 
            {
                
                Handles.DrawBezier(from, to, startTangent, endTangent, color, null, bezierLineWidth);
                var points = Handles.MakeBezierPoints(from, to, startTangent, endTangent, bezierDivision);
                
                BeginHandlesDrawColor(color);
                {
                    var arrowDirection = (points[points.Length - 1] - points[points.Length - 2]).normalized;
                    Handles.DrawAAConvexPolygon(GetArrowHead(to + arrowDirection * ArrowWidth, arrowDirection, zoom));
                }
                EndHandlesDrawColor();
            }
            Handles.EndGUI();
        }

        private static Vector3[] GetArrowHead(Vector3 edgePos, Vector3 direction, float zoom = 1f)
        {
            var arrowHead = new Vector3[3];
            var pos = edgePos - direction * ArrowHeight * zoom;
            var right = Vector3.Cross(Vector3.back, direction).normalized;
            arrowHead[0] = pos + direction * ArrowHeight * 0.75f * zoom;
            arrowHead[1] = pos - direction * ArrowHeight * 0.25f * zoom + right * ArrowWidth * zoom;
            arrowHead[2] = pos - direction * ArrowHeight * 0.25f * zoom - right * ArrowWidth * zoom;
            return arrowHead;
        }

        public static void BeginHandlesDrawColor(Color c)
        {
            _oldColor = Handles.color;
            Handles.color = c;
        }

        public static void EndHandlesDrawColor()
        {
            Handles.color = _oldColor;
        }
    }
}