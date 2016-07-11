// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO.Globbing.Nodes;

namespace Cake.Core.IO.Globbing
{
    internal static class GlobNodeRewriter
    {
        public static GlobNode Rewrite(IEnumerable<GlobNode> nodes)
        {
            return CreateLinkedList(RewriteSingleWildcards(nodes));
        }

        private static GlobNode CreateLinkedList(IEnumerable<GlobNode> nodes)
        {
            var result = new Stack<GlobNode>();
            var stack = new Stack<GlobNode>(nodes);
            var previous = stack.Pop();
            result.Push(previous);
            while (stack.Count > 0)
            {
                var current = stack.Pop();
                current.Next = previous;
                previous = current;
                result.Push(previous);
            }
            return result.Pop();
        }

        private static IEnumerable<GlobNode> RewriteSingleWildcards(IEnumerable<GlobNode> nodes)
        {
            foreach (var node in nodes)
            {
                var segmentNode = node as PathSegment;
                if (segmentNode != null)
                {
                    if (segmentNode.Tokens.Count == 1)
                    {
                        if (segmentNode.Tokens[0].Kind == GlobTokenKind.Wildcard)
                        {
                            yield return new WildcardSegment();
                            continue;
                        }
                    }
                }
                yield return node;
            }
        }
    }
}
