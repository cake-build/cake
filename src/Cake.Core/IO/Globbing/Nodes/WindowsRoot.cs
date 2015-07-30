///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class WindowsRoot : GlobNode
    {
        private readonly string _drive;

        public WindowsRoot(string drive)
        {
            _drive = drive;
        }

        public string Drive
        {
            get { return _drive; }
        }

        public override string Render()
        {
            return Drive + ":";
        }

        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitWindowsRoot(this, context);
        }

        public override string ToString()
        {
            return string.Concat(_drive ?? "?", ":");
        }
    }
}