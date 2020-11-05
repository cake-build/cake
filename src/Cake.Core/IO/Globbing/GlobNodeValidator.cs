// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO.Globbing.Nodes;

namespace Cake.Core.IO.Globbing
{
    internal static class GlobNodeValidator
    {
        public static void Validate(GlobPattern pattern, GlobNode node)
        {
            var previous = (GlobNode)null;
            var current = node;
            while (current != null)
            {
                if (previous is RecursiveWildcardNode)
                {
                    if (current is ParentDirectoryNode)
                    {
                        throw new NotSupportedException("Visiting a parent that is a recursive wildcard is not supported.");
                    }
                }

                if (current is UncRootNode unc)
                {
                    if (string.IsNullOrWhiteSpace(unc.Server))
                    {
                        throw new CakeException($"The pattern '{pattern}' has no server part specified.");
                    }
                }

                previous = current;
                current = current.Next;
            }
        }
    }
}