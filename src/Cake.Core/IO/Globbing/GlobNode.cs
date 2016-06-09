// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Diagnostics;

namespace Cake.Core.IO.Globbing
{
    [DebuggerDisplay("{ToString(),nq}")]
    internal abstract class GlobNode
    {
        public GlobNode Next { get; internal set; }

        public abstract void Accept(GlobVisitor visitor, GlobVisitorContext context);
    }
}
