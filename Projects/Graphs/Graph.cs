﻿using System;
using System.Collections.Generic;
using System.Linq;

namespace Graphs
{
    public class Graph 
    {
        public List<Node> Nodes = new List<Node>();

        public Graph()
        {
            
        }

        public Graph(Graph graph)
        {
            
        }
        
        public Node AddNode()
        {
            var newNode = new Node();
            Nodes.Add(newNode);
            return newNode;
        }

        public Arc AddArc(Node from, Node to)
        {
            var newArc = new Arc(from, to);
            from.ArcsOut.Add(newArc);
            to.ArcsIn.Add(newArc);
            return newArc;
        }

        public void RemoveArc(Node from, Node to)
        {
            var arc = from.ArcsOut.FirstOrDefault(a => a.To == to);
            from.ArcsOut.Remove(arc);
            to.ArcsIn.Remove(arc);
        }

        public void RemoveArc(Arc arc)
        {
            arc.From.ArcsOut.Remove(arc);
            arc.To.ArcsIn.Remove(arc);
        }

        public void RemoveNode(Node node)
        {
            Nodes.Remove(node);
            foreach (var arc in node.ArcsOut)
            {
                RemoveArc(arc);
            }
            
            foreach (var arc in node.ArcsIn)
            {
                RemoveArc(arc);
            }
        }

        public Graph Clone(Graph graph)
        {
            return new Graph(graph);
        }
    }
}