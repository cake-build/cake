///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Cake.Core.IO.Globbing.Nodes.Roots
{
    internal sealed class WindowsRoot : RootNode
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

        public override bool IsWildcard
        {
            get { return false; }
        }

        public override string Render()
        {
            return Drive + ":";
        }
    }
}