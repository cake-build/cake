// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.IO.Globbing.Nodes;

namespace Cake.Core.IO.Globbing
{
    internal static class GlobNodeValidator
    {
        public static void Validate(GlobNode node)
        {
            var previous = (GlobNode)null;
            var current = node;
            while (current != null)
            {
                if (previous is RecursiveWildcardSegment)
                {
                    if (current is ParentSegment)
                    {
                        throw new NotSupportedException("Visiting a parent that is a recursive wildcard is not supported.");
                    }
                }
                previous = current;
                current = current.Next;
            }
        }
    }
}
