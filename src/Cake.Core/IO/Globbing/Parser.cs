///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Cake.Core.IO.Globbing.Nodes;
using Cake.Core.IO.Globbing.Nodes.Roots;

namespace Cake.Core.IO.Globbing
{
    internal sealed class Parser
    {
        private readonly Scanner _scanner;
        private readonly ICakeEnvironment _environment;
        private Token _currentToken;

        public Parser(Scanner scanner, ICakeEnvironment environment)
        {
            _scanner = scanner;
            _environment = environment;
            _currentToken = null;
        }

        public List<Node> Parse()
        {
            Accept();

            // Parse the root.
            var items = new List<Node> { ParseRoot() };
            if (items.Count == 1 && items[0] is RelativeRoot)
            {
                items.Add(ParseSegment());
            }

            // Parse all path segments.
            while (_currentToken.Kind == TokenKind.PathSeparator)
            {
                Accept();
                items.Add(ParseSegment());
            }

            // Not an end of text token?
            if (_currentToken.Kind != TokenKind.EndOfText)
            {
                throw new InvalidOperationException("Expected EOT");
            }

            // Return the path node.
            return items;
        }

        private RootNode ParseRoot()
        {
            if (_environment.IsUnix())
            {
                // Starts with a separator?
                if (_currentToken.Kind == TokenKind.PathSeparator)
                {
                    return new UnixRoot();
                }
            }
            else
            {
                // Starts with a separator?
                if (_currentToken.Kind == TokenKind.PathSeparator)
                {
                    if (_scanner.Peek().Kind == TokenKind.PathSeparator)
                    {
                        throw new NotSupportedException("UNC paths are not supported.");
                    }
                    return new WindowsRoot(string.Empty);
                }

                // Is this a drive?
                if (_currentToken.Kind == TokenKind.Identifier &&
                    _currentToken.Value.Length == 1 &&
                    _scanner.Peek().Kind == TokenKind.WindowsRoot)
                {
                    var identifier = ParseIdentifier();
                    Accept(TokenKind.WindowsRoot);
                    return new WindowsRoot(identifier.Identifier);
                }
            }

            // Starts with an identifier?
            if (_currentToken.Kind == TokenKind.Identifier)
            {
                // Is the identifer indicating a current directory?
                if (_currentToken.Value == ".")
                {
                    Accept();
                    if (_currentToken.Kind != TokenKind.PathSeparator)
                    {
                        throw new InvalidOperationException();
                    }
                    Accept();
                }
                return new RelativeRoot();
            }

            throw new NotImplementedException();
        }

        private Node ParseSegment()
        {
            if (_currentToken.Kind == TokenKind.DirectoryWildcard)
            {
                Accept();
                return new WildcardSegmentNode();
            }

            var items = new List<Node>();
            while (true)
            {
                switch (_currentToken.Kind)
                {
                    case TokenKind.Identifier:
                    case TokenKind.CharacterWildcard:
                    case TokenKind.Wildcard:
                        items.Add(ParseSubSegment());
                        continue;
                }
                break;
            }
            return new SegmentNode(items);
        }

        private Node ParseSubSegment()
        {
            switch (_currentToken.Kind)
            {
                case TokenKind.Identifier:
                    return ParseIdentifier();
                case TokenKind.CharacterWildcard:
                case TokenKind.Wildcard:
                    return ParseWildcard(_currentToken.Kind);
            }

            throw new NotSupportedException("Unable to parse sub segment.");
        }

        private IdentifierNode ParseIdentifier()
        {
            if (_currentToken.Kind == TokenKind.Identifier)
            {
                var identifier = new IdentifierNode(_currentToken.Value);
                Accept();
                return identifier;
            }
            throw new InvalidOperationException("Unable to parse identifier.");
        }

        private Node ParseWildcard(TokenKind kind)
        {
            Accept(kind);
            return new WildcardNode(kind);
        }

        private void Accept(TokenKind kind)
        {
            if (_currentToken.Kind == kind)
            {
                Accept();
                return;
            }
            throw new InvalidOperationException("Unexpected token kind.");
        }

        private void Accept()
        {
            _currentToken = _scanner.Scan();
        }
    }
}
