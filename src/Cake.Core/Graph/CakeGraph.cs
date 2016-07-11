// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.Graph
{
    internal sealed class CakeGraph
    {
        private readonly List<string> _nodes;
        private readonly List<CakeGraphEdge> _edges;

        public CakeGraph()
        {
            _nodes = new List<string>();
            _edges = new List<CakeGraphEdge>();
        }

        public IReadOnlyList<string> Nodes
        {
            get { return _nodes; }
        }

        public IReadOnlyList<CakeGraphEdge> Edges
        {
            get { return _edges; }
        }

        public void Add(string node)
        {
            if (node == null)
            {
                throw new ArgumentNullException("node");
            }
            if (_nodes.Any(x => x == node))
            {
                throw new CakeException("Node has already been added to graph.");
            }
            _nodes.Add(node);
        }

        public void Connect(string start, string end)
        {
            if (start.Equals(end, StringComparison.OrdinalIgnoreCase))
            {
                throw new CakeException("Reflexive edges in graph are not allowed.");
            }
            if (_edges.Any(x => x.Start.Equals(end, StringComparison.OrdinalIgnoreCase)
                && x.End.Equals(start, StringComparison.OrdinalIgnoreCase)))
            {
                throw new CakeException("Unidirectional edges in graph are not allowed.");
            }
            if (_edges.Any(x => x.Start.Equals(start, StringComparison.OrdinalIgnoreCase)
                && x.End.Equals(end, StringComparison.OrdinalIgnoreCase)))
            {
                return;
            }
            if (_nodes.All(x => !x.Equals(start, StringComparison.OrdinalIgnoreCase)))
            {
                _nodes.Add(start);
            }
            if (_nodes.All(x => !x.Equals(end, StringComparison.OrdinalIgnoreCase)))
            {
                _nodes.Add(end);
            }
            _edges.Add(new CakeGraphEdge(start, end));
        }

        public bool Exist(string name)
        {
            return _nodes.Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        public IEnumerable<string> Traverse(string target)
        {
            if (!Exist(target))
            {
                return Enumerable.Empty<string>();
            }
            var result = new List<string>();
            Traverse(target, result);
            return result;
        }

        private void Traverse(string node, ICollection<string> result, ISet<string> visited = null)
        {
            visited = visited ?? new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            if (!visited.Contains(node))
            {
                visited.Add(node);
                var incoming = _edges.Where(x => x.End.Equals(node, StringComparison.OrdinalIgnoreCase)).Select(x => x.Start);
                foreach (var child in incoming)
                {
                    Traverse(child, result, visited);
                }
                result.Add(node);
            }
            else if (!result.Any(x => x.Equals(node, StringComparison.OrdinalIgnoreCase)))
            {
                throw new CakeException("Graph contains circular references.");
            }
        }
    }
}
