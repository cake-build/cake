///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Cake.Core.IO.Globbing.Nodes.Roots
{
    internal sealed class RelativeRoot : RootNode
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