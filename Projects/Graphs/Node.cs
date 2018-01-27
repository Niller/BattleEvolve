using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public class Node
    {
        public int Id
        {
            get;
            private set;
        }
        
        public readonly HashSet<Arc> ArcsOut = new HashSet<Arc>();
        public readonly HashSet<Arc> ArcsIn = new HashSet<Arc>();
        
        public Node(int id)
        {
            Id = id;
        }

        public Arc GetArcTo(Node node)
        {
            return ArcsOut.FirstOrDefault(a => a.To == node);
        }
    }
}