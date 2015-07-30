///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class WildcardSegment : GlobNode
    {
        public override string Render()
        {
            return ".*";
        }

        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitWildcardSegmentNode(this, context);
        }

        public override string ToString()
        {
            return "*";
        }
    }
}