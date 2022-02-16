// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;

namespace Cake.Core.Graph
{
    /// <summary>
    /// Represents the Cake task graph.
    /// </summary>
    public sealed class CakeGraph
    {
        private readonly List<string> _nodes;
        private readonly List<CakeGraphEdge> _edges;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeGraph"/> class.
        /// </summary>
        public CakeGraph()
        {
            _nodes = new List<string>();
            _edges = new List<CakeGraphEdge>();
        }

        /// <summary>
        /// Gets the nodes in the graph.
        /// </summary>
        public IReadOnlyList<string> Nodes => _nodes;

        /// <summary>
        /// Gets the edges in the graph.
        /// </summary>
        public IReadOnlyList<CakeGraphEdge> Edges => _edges;

        /// <summary>
        /// Adds a node to the graph.
        /// </summary>
        /// <param name="node">The node to add.</param>
        public void Add(string node)
        {
            if (node == null)
            {
                throw new ArgumentNullException(nameof(node));
            }
            if (_nodes.Any(x => x == node))
            {
                throw new CakeException("Node has already been added to graph.");
            }
            _nodes.Add(node);
        }

        /// <summary>
        /// Connects two nodes in the graph together.
        /// If any of the nodes are missing, they will be added.
        /// </summary>
        /// <param name="start">The start node.</param>
        /// <param name="end">The end node.</param>
        public void Connect(string start, string end)
        {
            if (start.Equals(end, StringComparison.OrdinalIgnoreCase))
            {
                throw new CakeException("Reflexive edges in graph are not allowed.");
            }
            if (_edges.Any(x => x.Start.Equals(end, StringComparison.OrdinalIgnoreCase)
                && x.End.Equals(start, StringComparison.OrdinalIgnoreCase)))
            {
                var firstBadEdge = _edges.First(x => x.Start.Equals(end, StringComparison.OrdinalIgnoreCase) && x.End.Equals(start, StringComparison.OrdinalIgnoreCase));
                throw new CakeException($"Unidirectional edges in graph are not allowed.{Environment.NewLine}\"{firstBadEdge.Start}\" and \"{firstBadEdge.End}\" cannot depend on each other.");
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

        /// <summary>
        /// Gets whether or not a node exists in the graph.
        /// </summary>
        /// <param name="name">The name of the node.</param>
        /// <returns>Whether or not the node exists in the graph.</returns>
        public bool Exist(string name)
        {
            return _nodes.Any(x => x.Equals(name, StringComparison.OrdinalIgnoreCase));
        }

        /// <summary>
        /// Traverses the graph leading to the specified target.
        /// </summary>
        /// <param name="target">The target to traverse to.</param>
        /// <returns>A list of nodes.</returns>
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