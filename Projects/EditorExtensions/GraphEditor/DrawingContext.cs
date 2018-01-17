using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class DrawingContext
    {
        public static DrawingContext Instance
        {
            get;
            private set;
        }

        public static void Create()
        {
            Instance = new DrawingContext();;
        }
        
        public Rect Viewport
        {
            get;
            set;
        }

        public Vector2 Scroll
        {
            get;
            set;
        }
    }
}