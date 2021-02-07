// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.Graph
{
    /// <summary>
    /// Represents an edge in a <see cref="CakeGraph"/>.
    /// </summary>
    public sealed class CakeGraphEdge
    {
        /// <summary>
        /// Gets the start node of the edge.
        /// </summary>
        public string Start { get; }

        /// <summary>
        /// Gets the end node of the edge.
        /// </summary>
        public string End { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeGraphEdge"/> class.
        /// </summary>
        /// <param name="start">The start node.</param>
        /// <param name="end">The end node.</param>
        public CakeGraphEdge(string start, string end)
        {
            Start = start;
            End = end;
        }
    }
}