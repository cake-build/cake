namespace Cake.Core.IO.Globbing.Nodes.Segments
{
    internal abstract class PathSegment
    {
        public abstract string Regex { get; }
        public abstract string Value { get; }
    }
}
