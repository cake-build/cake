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
        private readonly string _drive;

        public string Drive
        {
            get { return _drive; }
        }

        public WindowsRoot(string drive)
        {
            if (drive == null)
            {
                throw new ArgumentNullException("drive");
            }
            _drive = drive;
        }

        [DebuggerStepThrough]
        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitWindowsRoot(this, context);
        }
    }
}
