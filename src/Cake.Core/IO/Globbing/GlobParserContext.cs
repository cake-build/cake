// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Linq;
using System.Text.RegularExpressions;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobParserContext
    {
        private readonly GlobTokenizer _tokenizer;
        private readonly RegexOptions _regexOptions;
        private GlobToken _currentToken;

        public GlobToken CurrentToken
        {
            get { return _currentToken; }
        }

        public RegexOptions Options
        {
            get { return _regexOptions; }
        }

        public GlobParserContext(string pattern, bool caseSensitive)
        {
            _tokenizer = new GlobTokenizer(pattern);
            _currentToken = null;
            _regexOptions = RegexOptions.Compiled | RegexOptions.Singleline;

            if (!caseSensitive)
            {
                _regexOptions |= RegexOptions.IgnoreCase;
            }
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
