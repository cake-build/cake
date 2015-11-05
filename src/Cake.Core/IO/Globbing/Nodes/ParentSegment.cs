using System.Diagnostics;

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class ParentSegment : GlobNode
    {
        [DebuggerStepThrough]
        public override void Accept(GlobVisitor visitor, GlobVisitorContext context)
        {
            visitor.VisitParent(this, context);
        }
    }
}