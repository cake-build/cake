using System;
using System.Linq;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobParserContext
    {
        private readonly GlobTokenizer _tokenizer;
        private GlobToken _currentToken;

        public GlobToken CurrentToken
        {
            get { return _currentToken; }
        }

        public GlobParserContext(string pattern)
        {
            _tokenizer = new GlobTokenizer(pattern);
            _currentToken = null;
        }

        public GlobToken Peek()
        {
            return _tokenizer.Peek();
        }

        public void Accept()
        {
            _currentToken = _tokenizer.Scan();
        }

        public void Accept(GlobTokenKind kind)
        {
            Accept(new[] { kind });
        }

        public void Accept(params GlobTokenKind[] kind)
        {
            if (kind.Any(k => k == _currentToken.Kind))
            {
                Accept();
                return;
            }
            throw new InvalidOperationException("Unexpected token kind.");
        }
    }
}
