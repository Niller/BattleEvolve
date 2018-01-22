using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public struct NodeDrawInfo
    {
        public int Id
        {
            get;
            private set;
        }
        public Vector2 Position;
        public float Radius => 20;
        
        public EventType LastEventType 
        { 
            get; 
            set; 
        }

        public NodeDrawInfo(int id, Vector2 postion)
        {
            Id = id;
            Position = postion;
        }
    }
}