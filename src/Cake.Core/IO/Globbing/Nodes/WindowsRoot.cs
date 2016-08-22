// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Diagnostics;

namespace Cake.Core.IO.Globbing.Nodes
{
    [DebuggerDisplay("{Drive,nq}:")]
    internal sealed class WindowsRoot : GlobNode
    {
        public string Drive { get; }

        public WindowsRoot(string drive)
        {
            if (drive == null)
            {
                throw new ArgumentNullException(nameof(drive));
            }
            Drive = drive;
        }

        [DebuggerStepThrough]
        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitWindowsRoot(this, context);
        }
    }
}