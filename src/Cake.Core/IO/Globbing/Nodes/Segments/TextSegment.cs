namespace Cake.Core.IO.Globbing.Nodes.Segments
{
    internal sealed class TextSegment : PathSegment
    {
        public override string Value { get; }

        public override string Regex { get; }

        public TextSegment(string text)
        {
            Value = text;
            Regex = Value.Replace("+", "\\+").Replace(".", "\\.");
        }
    }
}
