///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

namespace Cake.Core.IO.Globbing
{
    internal sealed class Token
    {
        private readonly TokenKind _kind;
        private readonly string _value;

        public Token(TokenKind kind, string value)
        {
            _kind = kind;
            _value = value;
        }

        public TokenKind Kind
        {
            get { return _kind; }
        }

        public string Value
        {
            get { return _value; }
        }

    }
}
