// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.IO.Globbing.Nodes.Segments
{
    internal sealed class BracketWildcardSegment : PathSegment
    {
        public override string Value { get; }

        public override string Regex => Value;

        public BracketWildcardSegment(string content)
        {
            if (content.StartsWith("!"))
            {
                // Content is negated.
                content = content.TrimStart('!').Insert(0, "^");
            }

            Value = $"[{content}]";
        }
    }
}
