namespace Cake.Core.IO.Globbing.Nodes
{
    internal abstract class MatchableNode : GlobNode
    {
        public abstract bool IsMatch(string value);
    }
}