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

        public GlobToken CurrentToken { get; private set; }

        public RegexOptions Options { get; }

        public GlobParserContext(string pattern, bool caseSensitive)
        {
            _tokenizer = new GlobTokenizer(pattern);
            CurrentToken = null;
            Options = RegexOptions.Compiled | RegexOptions.Singleline;

            if (!caseSensitive)
            {
                Options |= RegexOptions.IgnoreCase;
            }
        }

        /// <summary>
        /// Gets the next GlobToken from the context
        /// </summary>
        /// <returns>The Peek'd token</returns>
        public GlobToken Peek()
        {
            return _tokenizer.Peek();
        }

        /// <summary>
        /// Accepts the current GlobToken and loads the next into CurrentToken
        /// </summary>
        public void Accept()
        {
            CurrentToken = _tokenizer.Scan();
        }

        /// <summary>
        /// Accepts the CurrentToken if it is of the specified TokenKind(s), and fetches the next token.
        /// </summary>
        /// <param name="kind">The types of acceptable <see cref="GlobTokenKind"/></param>
        public void Accept(params GlobTokenKind[] kind)
        {
            if (kind.Any(k => k == CurrentToken.Kind))
            {
                Accept();
                return;
            }

            throw new InvalidOperationException("Unexpected token kind.");
        }
    }
}