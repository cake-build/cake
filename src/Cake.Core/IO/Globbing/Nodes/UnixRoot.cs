///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System.Diagnostics;

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class UnixRoot : GlobNode
    {
        public override bool IsMatch(string value)
        {
            return false;
        }

        [DebuggerStepThrough]
        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitUnixRoot(this, context);
        }
    }
}