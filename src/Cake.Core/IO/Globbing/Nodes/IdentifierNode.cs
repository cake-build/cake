///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class IdentifierNode : Node
    {
        private readonly string _identifier;

        public override bool IsWildcard
        {
            get { return false; }
        }

        public string Identifier
        {
            get { return _identifier; }
        }

        public IdentifierNode(string identifier)
        {
            _identifier = identifier;
        }

        public override string Render()
        {
            return _identifier;
        }
    }
}