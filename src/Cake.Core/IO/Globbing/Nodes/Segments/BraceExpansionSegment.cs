// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.IO.Globbing.Nodes.Segments
{
    internal sealed class BraceExpansionSegment : PathSegment
    {
        public override string Value { get; }

        public override string Regex { get; }

        public BraceExpansionSegment(string value)
        {
            Value = $"{{{value}}}";
            Regex = $"({value})".Replace(",", "|");
        }
    }
}
