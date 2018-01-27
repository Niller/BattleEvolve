using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class NodeDrawInfo
    {
        public int Id
        {
            get;
            private set;
        }
        public Vector2 Position;
        public int Radius => 10;
        
        public EventType LastEventType 
        { 
            get; 
            set; 
        }

        public NodeDrawInfo(int id, Vector2 postion)
        {
            Id = id;
            Position = postion;
            LastEventType = EventType.Layout;
        }
    }
}