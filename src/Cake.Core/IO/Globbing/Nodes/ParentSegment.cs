// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Diagnostics;

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class ParentSegment : GlobNode
    {
        [DebuggerStepThrough]
        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitParent(this, context);
        }
    }
}
