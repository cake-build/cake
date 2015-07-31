///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System.Diagnostics;

namespace Cake.Core.IO.Globbing.Nodes
{
    [DebuggerDisplay("**")]
    internal sealed class RecursiveWildcardSegment : GlobNode
    {
        public override bool IsMatch(string value)
        {
            return true;
        }

        [DebuggerStepThrough]
        public override void Accept(GlobVisitor globber, GlobVisitorContext context)
        {
            globber.VisitRecursiveWildcardSegment(this, context);
        }
    }
}