// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.Diagnostics;
using System.Text;
using System.Text.RegularExpressions;

namespace Cake.Core.IO.Globbing.Nodes
{
    [DebuggerDisplay("{GetPath(),nq}")]
    internal sealed class PathSegment : MatchableNode
    {
        private readonly List<GlobToken> _tokens;
        private readonly Regex _regex;
        private readonly bool _isIdentifier;

        public IReadOnlyList<GlobToken> Tokens
        {
            get { return _tokens; }
        }

        public bool IsIdentifier
        {
            get { return _isIdentifier; }
        }

        public PathSegment(List<GlobToken> tokens, RegexOptions options)
        {
            _tokens = tokens;
            _isIdentifier = _tokens.Count == 1 && _tokens[0].Kind == GlobTokenKind.Identifier;
            _regex = CreateRegex(tokens, options);
        }

        public override bool IsMatch(string value)
        {
            return _regex.IsMatch(value);
        }

        public string GetPath()
        {
            var builder = new StringBuilder();
            foreach (var token in _tokens)
            {
                builder.Append(token.Value);
            }
            return builder.ToString();
        }

        [DebuggerStepThrough]
        public override void Accept(GlobVisitor globber, GlobVisitorContext context)
        {
            globber.VisitSegment(this, context);
        }

        private static Regex CreateRegex(List<GlobToken> tokens, RegexOptions options)
        {
            var builder = new StringBuilder();
            foreach (var token in tokens)
            {
                if (token.Kind == GlobTokenKind.Identifier)
                {
                    builder.Append(token.Value.Replace("+", "\\+").Replace(".", "\\."));
                }
                if (token.Kind == GlobTokenKind.Wildcard)
                {
                    builder.Append(".*");
                }
                if (token.Kind == GlobTokenKind.CharacterWildcard)
                {
                    builder.Append(".{1}");
                }
            }
            return new Regex(string.Concat("^", builder.ToString(), "$"), options);
        }
    }
}
