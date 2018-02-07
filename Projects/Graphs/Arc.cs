using System.Runtime.InteropServices;
using Graphs.Tags;

namespace Graphs
{
    public class Arc
    {
        public IGraphTag Tag
        {
            get;
            private set;
        }
        
        public Node From
        {
            get; 
            private set;
        }

        public Node To
        {
            get; 
            private set;
        }

        public Arc(Node from, Node to, IGraphTag tag)
        {
            From = from;
            To = to;
            Tag = tag;
        }

        public bool IsLoop()
        {
            return From == To;
        }
    }
}