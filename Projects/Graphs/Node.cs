using System.Collections.Generic;
using System.IO;
using System.Linq;
using Graphs.Tags;
using Utilities;

namespace Graphs
{
    public class Node
    {
        public IGraphTag Tag
        {
            get;
            private set;
        }
        
        public uint Id
        {
            get;
            private set;
        }
        
        public readonly HashSet<Arc> ArcsOut = new HashSet<Arc>();
        public readonly HashSet<Arc> ArcsIn = new HashSet<Arc>();
        
        public Node(uint id, IGraphTag tag)
        {
            Id = id;
            Tag = tag;
        }

        public Arc GetArcTo(Node node)
        {
            return ArcsOut.FirstOrDefault(a => a.To == node);
        }
    }
}