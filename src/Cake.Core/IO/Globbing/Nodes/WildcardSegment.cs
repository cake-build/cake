///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Cake.Core.IO.Globbing.Nodes
{
    [DebuggerDisplay("*")]
    internal sealed class WildcardSegment : GlobNode
    {
        public override bool IsMatch(string value)
        {
            return true;
        }

        [DebuggerStepThrough]
        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitWildcardSegmentNode(this, context);
        }
    }
}