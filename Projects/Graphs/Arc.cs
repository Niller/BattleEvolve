using System.Runtime.InteropServices;

namespace Graphs
{
    public class Arc
    {
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

        public Arc(Node from, Node to)
        {
            From = from;
            To = to;
        }

        public bool IsLoop()
        {
            return From == To;
        }
    }
}