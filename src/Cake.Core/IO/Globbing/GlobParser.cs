///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using Cake.Core.IO.Globbing.Nodes;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobParser
    {
        private readonly ICakeEnvironment _environment;

        public GlobParser(ICakeEnvironment environment)
        {
            _environment = environment;
        }

        public GlobNode Parse(string pattern)
        {
            return Parse(new GlobParserContext(pattern));
        }

        private GlobNode Parse(GlobParserContext context)
        {
            context.Accept();

            // Parse the root.
            var items = new List<GlobNode> { ParseRoot(context) };
            if (items.Count == 1 && items[0] is RelativeRoot)
            {
                items.Add(ParseSegment(context));
            }

            // Parse all path segments.
            while (context.CurrentToken.Kind == GlobTokenKind.PathSeparator)
            {
                context.Accept();
                items.Add(ParseSegment(context));
            }

            // Not an end of text token?
            if (context.CurrentToken.Kind != GlobTokenKind.EndOfText)
            {
                throw new InvalidOperationException("Expected EOT");
            }

            // Return the path node.
            return GlobNodeRewriter.Rewrite(items);
        }

        private GlobNode ParseRoot(GlobParserContext context)
        {
            if (_environment.IsUnix())
            {
                // Starts with a separator?
                if (context.CurrentToken.Kind == GlobTokenKind.PathSeparator)
                {
                    return new UnixRoot();
                }
            }
            else
            {
                // Starts with a separator?
                if (context.CurrentToken.Kind == GlobTokenKind.PathSeparator)
                {
                    if (context.Peek().Kind == GlobTokenKind.PathSeparator)
                    {
                        throw new NotSupportedException("UNC paths are not supported.");
                    }
                    return new WindowsRoot(string.Empty);
                }

                // Is this a drive?
                if (context.CurrentToken.Kind == GlobTokenKind.Identifier &&
                    context.CurrentToken.Value.Length == 1 &&
                    context.Peek().Kind == GlobTokenKind.WindowsRoot)
                {
                    var identifier = ParseIdentifier(context);
                    context.Accept(GlobTokenKind.WindowsRoot);
                    return new WindowsRoot(identifier.Value);
                }
            }

            // Starts with an identifier?
            if (context.CurrentToken.Kind == GlobTokenKind.Identifier)
            {
                // Is the identifer indicating a current directory?
                if (context.CurrentToken.Value == ".")
                {
                    context.Accept();
                    if (context.CurrentToken.Kind != GlobTokenKind.PathSeparator)
                    {
                        throw new InvalidOperationException();
                    }
                    context.Accept();
                }
            }

            return new RelativeRoot();
        }

        private static GlobNode ParseSegment(GlobParserContext context)
        {
            if (context.CurrentToken.Kind == GlobTokenKind.DirectoryWildcard)
            {
                context.Accept();
                return new RecursiveWildcardSegment();
            }

            var items = new List<GlobToken>();
            while (true)
            {
                switch (context.CurrentToken.Kind)
                {
                    case GlobTokenKind.Identifier:
                    case GlobTokenKind.CharacterWildcard:
                    case GlobTokenKind.Wildcard:
                        items.Add(ParseSubSegment(context));
                        continue;
                }
                break;
            }
            return new PathSegment(items);
        }

        private static GlobToken ParseSubSegment(GlobParserContext context)
        {
            switch (context.CurrentToken.Kind)
            {
                case GlobTokenKind.Identifier:
                    return ParseIdentifier(context);
                case GlobTokenKind.CharacterWildcard:
                case GlobTokenKind.Wildcard:
                    return ParseWildcard(context);
            }

            throw new NotSupportedException("Unable to parse sub segment.");
        }

        private static GlobToken ParseIdentifier(GlobParserContext context)
        {
            var token = context.CurrentToken;
            context.Accept(GlobTokenKind.Identifier);
            return token;
        }

        private static GlobToken ParseWildcard(GlobParserContext context)
        {
            var token = context.CurrentToken;
            context.Accept(GlobTokenKind.Wildcard, GlobTokenKind.CharacterWildcard);
            return token;
        }
    }
}
