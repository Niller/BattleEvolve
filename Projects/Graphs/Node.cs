using System.Collections.Generic;

namespace Graphs
{
    public class Node
    {
        public int Id
        {
            get;
            private set;
        }
        
        public List<Arc> ArcsOut = new List<Arc>();
        public List<Arc> ArcsIn = new List<Arc>();
        
        public Node(int id)
        {
            Id = id;
        }
    }
}