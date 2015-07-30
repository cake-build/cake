///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO.Globbing.Nodes;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobParser
    {
        private readonly GlobTokenizer _tokenizer;
        private readonly ICakeEnvironment _environment;
        private GlobToken _currentToken;

        public GlobParser(GlobTokenizer tokenizer, ICakeEnvironment environment)
        {
            _tokenizer = tokenizer;
            _environment = environment;
            _currentToken = null;
        }

        public GlobNode Parse()
        {
            Accept();

            // Parse the root.
            var items = new List<GlobNode> { ParseRoot() };
            if (items.Count == 1 && items[0] is RelativeRoot)
            {
                items.Add(ParseSegment());
            }

            // Parse all path segments.
            while (_currentToken.Kind == GlobTokenKind.PathSeparator)
            {
                Accept();
                items.Add(ParseSegment());
            }

            // Not an end of text token?
            if (_currentToken.Kind != GlobTokenKind.EndOfText)
            {
                throw new InvalidOperationException("Expected EOT");
            }

            // Return the path node.
            return GlobNodeRewriter.Rewrite(items);
        }

        private GlobNode ParseRoot()
        {
            if (_environment.IsUnix())
            {
                // Starts with a separator?
                if (_currentToken.Kind == GlobTokenKind.PathSeparator)
                {
                    return new UnixRoot();
                }
            }
            else
            {
                // Starts with a separator?
                if (_currentToken.Kind == GlobTokenKind.PathSeparator)
                {
                    if (_tokenizer.Peek().Kind == GlobTokenKind.PathSeparator)
                    {
                        throw new NotSupportedException("UNC paths are not supported.");
                    }
                    return new WindowsRoot(string.Empty);
                }

                // Is this a drive?
                if (_currentToken.Kind == GlobTokenKind.Identifier &&
                    _currentToken.Value.Length == 1 &&
                    _tokenizer.Peek().Kind == GlobTokenKind.WindowsRoot)
                {
                    var identifier = ParseIdentifier();
                    Accept(GlobTokenKind.WindowsRoot);
                    return new WindowsRoot(identifier.Value);
                }
            }

            // Starts with an identifier?
            if (_currentToken.Kind == GlobTokenKind.Identifier)
            {
                // Is the identifer indicating a current directory?
                if (_currentToken.Value == ".")
                {
                    Accept();
                    if (_currentToken.Kind != GlobTokenKind.PathSeparator)
                    {
                        throw new InvalidOperationException();
                    }
                    Accept();
                }
            }

            return new RelativeRoot();
        }

        private GlobNode ParseSegment()
        {
            if (_currentToken.Kind == GlobTokenKind.DirectoryWildcard)
            {
                Accept();
                return new RecursiveWildcardSegment();
            }

            var items = new List<GlobToken>();
            while (true)
            {
                switch (_currentToken.Kind)
                {
                    case GlobTokenKind.Identifier:
                    case GlobTokenKind.CharacterWildcard:
                    case GlobTokenKind.Wildcard:
                        items.Add(ParseSubSegment());
                        continue;
                }
                break;
            }
            return new PathSegment(items);
        }

        private GlobToken ParseSubSegment()
        {
            switch (_currentToken.Kind)
            {
                case GlobTokenKind.Identifier:
                    return ParseIdentifier();
                case GlobTokenKind.CharacterWildcard:
                case GlobTokenKind.Wildcard:
                    return ParseWildcard();
            }

            throw new NotSupportedException("Unable to parse sub segment.");
        }

        private GlobToken ParseIdentifier()
        {
            var token = _currentToken;
            Accept(GlobTokenKind.Identifier);
            return token;
        }

        private GlobToken ParseWildcard()
        {
            var token = _currentToken;
            Accept(GlobTokenKind.Wildcard, GlobTokenKind.CharacterWildcard);
            return token;
        }

        private void Accept()
        {
            _currentToken = _tokenizer.Scan();
        }

        private void Accept(GlobTokenKind kind)
        {
            Accept(new[] { kind });
        }

        private void Accept(params GlobTokenKind[] kind)
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
