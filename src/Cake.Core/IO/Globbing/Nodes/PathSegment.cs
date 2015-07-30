///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System.Collections.Generic;
using System.Text;

namespace Cake.Core.IO.Globbing.Nodes
{
    internal sealed class PathSegment : GlobNode
    {
        private readonly List<GlobToken> _tokens;

        public IReadOnlyList<GlobToken> Tokens
        {
            get { return _tokens; }
        }

        public bool IsIdentifier
        {
            get
            {
                return _tokens.Count == 1 && _tokens[0].Kind == GlobTokenKind.Identifier;
            }
        }

        public PathSegment(List<GlobToken> tokens)
        {
            _tokens = tokens;
        }

        public override string Render()
        {
            var builder = new StringBuilder();
            foreach (var token in _tokens)
            {
                if (token.Kind == GlobTokenKind.Identifier)
                {
                    builder.Append(token.Value.Replace("+", "\\+"));
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
            return builder.ToString();
        }

        public override void Accept(GlobVisitor globber, GlobVisitorContext context)
        {
            globber.VisitSegment(this, context);
        }

        public override string ToString()
        {
            var builder = new StringBuilder();
            foreach (var token in _tokens)
            {
                builder.Append(token.Value);
            }
            return builder.ToString();
        }
    }
}