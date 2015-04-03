///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class UnixRoot : Node
    {
        public override bool IsWildcard
        {
            get { return false; }
        }

        public override string Render()
        {
            return string.Empty;
        } 
    }
}