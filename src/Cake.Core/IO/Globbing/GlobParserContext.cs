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
        private readonly GlobTokenBuffer _buffer;

        public GlobPattern Pattern { get; }
        public int TokenCount => _buffer.Count;
        public GlobToken CurrentToken { get; private set; }
        public RegexOptions Options { get; }

        public GlobParserContext(GlobPattern pattern, GlobTokenBuffer buffer, bool caseSensitive)
        {
            _buffer = buffer;

            Pattern = pattern;
            CurrentToken = null;
            Options = RegexOptions.Compiled | RegexOptions.Singleline;

            if (!caseSensitive)
            {
                Options |= RegexOptions.IgnoreCase;
            }
        }

        public GlobToken Peek()
        {
            return _buffer.Peek();
        }

        public GlobToken Accept()
        {
            var result = CurrentToken;
            CurrentToken = _buffer.Read();
            return result;
        }

        public GlobToken Accept(params GlobTokenKind[] kind)
        {
            if (kind.Any(k => k == CurrentToken.Kind))
            {
                var result = CurrentToken;
                Accept();
                return result;
            }

            throw new InvalidOperationException("Unexpected token kind.");
        }
    }
}