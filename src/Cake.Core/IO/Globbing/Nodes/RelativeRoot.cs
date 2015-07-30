using System.Diagnostics;

namespace Cake.Core.IO.Globbing.Nodes
{
    [DebuggerDisplay("./")]
    internal sealed class RelativeRoot : GlobNode
    {
        [DebuggerStepThrough]
        public override void Accept(GlobVisitor globber, GlobVisitorContext context)
        {
            globber.VisitRelativeRoot(this, context);
        }
    }
}