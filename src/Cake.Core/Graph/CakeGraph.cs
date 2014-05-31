using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.Graph
{
    internal sealed class CakeGraph
    {
        private readonly List<CakeTask> _nodes;
        private readonly List<CakeGraphEdge> _edges;

        public CakeGraph()
        {
            _nodes = new List<CakeTask>();
            _edges = new List<CakeGraphEdge>();
        }

        public IReadOnlyList<CakeTask> Nodes
        {
            get { return _nodes; }
        }

        public IReadOnlyList<CakeGraphEdge> Edges
        {
            get { return _edges; }
        }

        public void Add(CakeTask node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (_nodes.Any(x => x.Name == node.Name))
            {                
                throw new CakeException("Node has already been added to graph.");
            }
            _nodes.Add(node);   
        }

        public void Connect(CakeTask start, CakeTask end)
        {
            if (start.Name == end.Name)
            {
                throw new CakeException("Reflexive edges in graph are not allowed.");
            }
            if (_edges.Any(x => x.Start.Name == end.Name && x.End.Name == start.Name))
            {
                throw new CakeException("Unidirectional edges in graph are not allowed.");
            }
            if (_edges.Any(x => x.Start.Name == start.Name && x.End.Name == end.Name))
            {
                return;
            }
            if (_nodes.All(x => x.Name != start.Name))
            {
                _nodes.Add(start);
            }
            if (_nodes.All(x => x.Name != end.Name))
            {
                _nodes.Add(end);
            }
            _edges.Add(new CakeGraphEdge(start, end));
        }

        public CakeTask Find(string name)
        {
            return _nodes.SingleOrDefault(x => x.Name == name);
        }

        public IEnumerable<CakeTask> Traverse(string target)
        {
            var root = Find(target);
            if (root == null)
            {
                return Enumerable.Empty<CakeTask>();
            }
            var result = new List<CakeTask>();
            Traverse(root, result);
            return result;
        }

        private void Traverse(CakeTask node, ICollection<CakeTask> result, ISet<CakeTask> visited = null)
        {
            visited = visited ?? new HashSet<CakeTask>();
            if (!visited.Contains(node))
            {
                visited.Add(node);
                var incoming = _edges.Where(x => x.End.Name == node.Name).Select(x => x.Start);
                foreach (var child in incoming)
                {
                    Traverse(child, result, visited);
                }
                result.Add(node);
            }
            else if (!result.Contains(node))
            {
                throw new CakeException("Graph contains circular references.");
            }
        }
    }
}
