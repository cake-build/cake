namespace Cake.Core.IO.Globbing.Nodes.Segments
{
    internal sealed class WildcardSegment : PathSegment
    {
        public override string Value => "*";

        public override string Regex => ".*";
    }
}
