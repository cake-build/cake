namespace Cake.Core.IO.Globbing.Nodes.Segments
{
    internal sealed class BracketWildcardSegment : PathSegment
    {
        public override string Value { get; }

        public override string Regex => Value;

        public BracketWildcardSegment(string content)
        {
            if (content.StartsWith("!"))
            {
                // Content is negated.
                content = content.TrimStart('!').Insert(0, "^");
            }

            Value = $"[{content}]";
        }
    }
}
