using System.Diagnostics;

namespace Cake.Core.IO.Globbing.Nodes
{
    [DebuggerDisplay("**")]
    internal sealed class RecursiveWildcardSegment : MatchableNode
    {
        [DebuggerStepThrough]
        public override void Accept(GlobVisitor globber, GlobVisitorContext context)
        {
            globber.VisitRecursiveWildcardSegment(this, context);
        }

        public override bool IsMatch(string value)
        {
            return true;
        }
    }
}