using System.Runtime.InteropServices;
using UnityEngine;

namespace EditorExtensions.GraphEditor
{
    public class NodeDrawInfo
    {
        private int _radius = 10;
        
        public uint Id
        {
            get;
            private set;
        }
        public Vector2 Position;

        public int Radius
        {
            get
            {
                return (int)(_radius * DrawingContext.Current.Zoom);
                
            }
            set
            {
                _radius = value;
            }
        }
        
        public NodeDrawInfo(uint id, Vector2 postion)
        {
            Id = id;
            Position = postion;
        }
    }
}