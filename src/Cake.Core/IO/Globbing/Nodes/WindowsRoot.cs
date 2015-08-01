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