///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

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

        public override bool IsMatch(string value)
        {
            return false;
        }

        [DebuggerStepThrough]
        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitWindowsRoot(this, context);
        }
    }
}