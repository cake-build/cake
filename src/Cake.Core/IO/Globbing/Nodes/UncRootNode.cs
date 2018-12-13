// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Cake.Core.IO.Globbing.Nodes
{
    [DebuggerDisplay(@"\\")]
    internal sealed class UncRootNode : GlobNode
    {
        public string Server { get; }

        public UncRootNode(string server)
        {
            Server = server;
        }

        [DebuggerStepThrough]
        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitUncRoot(this, context);
        }
    }
}