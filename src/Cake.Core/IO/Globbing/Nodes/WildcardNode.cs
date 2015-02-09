///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System;

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class WildcardNode : Node
    {
        private readonly TokenKind _kind;

        public override bool IsWildcard
        {
            get { return true; }
        }

        public TokenKind Kind
        {
            get { return _kind; }
        }

        public WildcardNode(TokenKind kind)
        {
            _kind = kind;
        }

        public override string Render()
        {
            switch (Kind)
            {
                case TokenKind.Wildcard:
                    return ".*";
                case TokenKind.CharacterWildcard:
                    return ".{1}";
                default:
                    throw new NotSupportedException();
            }            
        }
    }
}