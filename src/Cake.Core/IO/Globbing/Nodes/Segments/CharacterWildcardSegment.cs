namespace Cake.Core.IO.Globbing.Nodes.Segments
{
    internal sealed class CharacterWildcardSegment : PathSegment
    {
        public override string Value => "?";

        public override string Regex => ".{1}";
    }
}
