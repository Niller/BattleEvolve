using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using ISerializable = Utilities.ISerializable;

namespace Graphs
{
    public class Graph : ISerializable
    {
        public static uint Signature = 999;
        
        private uint _nodeIndex = 0;
        
        public readonly HashSet<Node> Nodes = new HashSet<Node>();
        public readonly HashSet<Arc> Arcs = new HashSet<Arc>();

        public Graph()
        {
            
        }

        public Graph(Graph graph)
        {
            
        }
        
        public Node AddNode()
        {
            return AddNode(_nodeIndex++);
        }
        
        private Node AddNode(uint id)
        {
            var newNode = new Node(id);
            Nodes.Add(newNode);
            return newNode;
        }
        
        public Arc AddArc(Node from, Node to)
        {
            if (from.GetArcTo(to) != null)
            {
                return null;
            }
            
            var newArc = new Arc(from, to);           
            from.ArcsOut.Add(newArc);
            to.ArcsIn.Add(newArc);
            Arcs.Add(newArc);
            return newArc;
        }
        
        public Arc AddArc(int id1, int id2)
        {
            return AddArc(GetNode(id1), GetNode(id2));
        }
        
        public void RemoveArc(Node from, Node to)
        {
            var arc = from.ArcsOut.FirstOrDefault(a => a.To == to);
            from.ArcsOut.Remove(arc);
            to.ArcsIn.Remove(arc);
            Arcs.Remove(arc);
        }

        public void RemoveArc(Arc arc)
        {
            arc.From.ArcsOut.Remove(arc);
            arc.To.ArcsIn.Remove(arc);
            Arcs.Remove(arc);
        }

        public Node GetNode(int id)
        {
            return Nodes.FirstOrDefault(n => n.Id == id);
        }

        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);
            foreach (var arc in node.ArcsOut.ToList())
            {
                RemoveArc(arc);
            }
            
            foreach (var arc in node.ArcsIn.ToList())
            {
                RemoveArc(arc);
            }
        }

        public Graph Clone(Graph graph)
        {
            return new Graph(graph);
        }

        public void Serialize(BinaryWriter writer)
        {
            writer.Write(Signature);
            
            writer.Write(Nodes.Count);

            foreach (var node in Nodes)
            {
                writer.Write(node.Id);    
            }
            
            writer.Write(Arcs.Count);

            foreach (var arc in Arcs)
            {
                writer.Write(arc.From.Id);
                writer.Write(arc.To.Id);
            }
        }

        public void Deserialize(BinaryReader reader)
        {
            var signature = reader.ReadUInt32();

            if (signature != Signature)
            {
                throw new SerializationException("Wrong signature");
            }
            
            var nodesCount = reader.ReadInt32();

            for (int i = 0; i < nodesCount; i++)
            {
                var id = reader.ReadUInt32();
                AddNode(id);
                _nodeIndex = Math.Max(_nodeIndex, id);
            }

            var arcsCount = reader.ReadInt32();

            for (int i = 0; i < arcsCount; i++)
            {
                AddArc(reader.ReadInt32(), reader.ReadInt32());
            }
        }
    }
}