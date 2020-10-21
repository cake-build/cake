// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

///////////////////////////////////////////////////////////////////////
// Portions of this code was ported from glob-js by Kevin Thompson.
// https://github.com/kthompson/glob-js
///////////////////////////////////////////////////////////////////////

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO.Globbing.Nodes;
using Cake.Core.IO.Globbing.Nodes.Segments;

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobParser
    {
        private readonly ICakeEnvironment _environment;

        public GlobParser(ICakeEnvironment environment)
        {
            _environment = environment;
        }

        public GlobNode Parse(GlobPattern pattern, GlobberSettings settings)
        {
            var buffer = GlobTokenizer.Tokenize(pattern);
            var isCaseSensitive = settings.IsCaseSensitive ?? _environment.Platform.IsUnix();
            return Parse(new GlobParserContext(pattern, buffer, isCaseSensitive));
        }

        private GlobNode Parse(GlobParserContext context)
        {
            context.Accept();

            // Parse the root.
            var items = new List<GlobNode> { ParseRoot(context) };
            if (items.Count == 1 && items[0] is RelativeRootNode)
            {
                items.Add(ParseNode(context));
            }

            // Parse all path segments.
            while (context.TokenCount > 0 && context.CurrentToken?.Kind == GlobTokenKind.PathSeparator)
            {
                context.Accept();
                items.Add(ParseNode(context));
            }

            // Rewrite the items into a linked list.
            var result = GlobNodeRewriter.Rewrite(context.Pattern, items);
            GlobNodeValidator.Validate(context.Pattern, result);
            return result;
        }

        private GlobNode ParseRoot(GlobParserContext context)
        {
            if (_environment.Platform.IsUnix())
            {
                // Starts with a separator?
                if (context.CurrentToken.Kind == GlobTokenKind.PathSeparator)
                {
                    return new UnixRootNode();
                }
            }
            else
            {
                // Starts with a separator?
                if (context.CurrentToken.Kind == GlobTokenKind.PathSeparator)
                {
                    if (context.Peek().Kind == GlobTokenKind.PathSeparator)
                    {
                        context.Accept();
                        return new UncRootNode(null);
                    }

                    // Get the drive from the working directory.
                    var workingDirectory = _environment.WorkingDirectory;
                    var root = workingDirectory.FullPath.Split(':').First();
                    return new WindowsRootNode(root);
                }

                // Is this a drive?
                if (context.CurrentToken.Kind == GlobTokenKind.Text &&
                    context.CurrentToken.Value.Length == 1 &&
                    context.Peek().Kind == GlobTokenKind.WindowsRoot)
                {
                    var identifier = ParseText(context);
                    context.Accept(GlobTokenKind.WindowsRoot);
                    return new WindowsRootNode(identifier.Value);
                }
            }

            // Starts with an identifier?
            if (context.CurrentToken.Kind == GlobTokenKind.Text)
            {
                // Is the identifier indicating a current directory?
                if (context.CurrentToken.Value == ".")
                {
                    context.Accept(GlobTokenKind.Text);
                    if (context.CurrentToken.Kind != GlobTokenKind.PathSeparator)
                    {
                        throw new InvalidOperationException();
                    }
                    context.Accept(GlobTokenKind.PathSeparator);
                }
            }

            return new RelativeRootNode();
        }

        private static GlobNode ParseNode(GlobParserContext context)
        {
            if (context.CurrentToken?.Kind == GlobTokenKind.Wildcard)
            {
                var next = context.Peek();
                if (next != null && next.Kind == GlobTokenKind.Wildcard)
                {
                    context.Accept(GlobTokenKind.Wildcard);
                    context.Accept(GlobTokenKind.Wildcard);
                    return new RecursiveWildcardNode();
                }
            }
            else if (context.CurrentToken?.Kind == GlobTokenKind.Parent)
            {
                context.Accept(GlobTokenKind.Parent);
                return new ParentDirectoryNode();
            }
            else if (context.CurrentToken?.Kind == GlobTokenKind.Current)
            {
                context.Accept(GlobTokenKind.Current);
                return new CurrentDirectoryNode();
            }

            var items = new List<PathSegment>();
            while (true)
            {
                switch (context.CurrentToken?.Kind)
                {
                    case GlobTokenKind.Text:
                    case GlobTokenKind.CharacterWildcard:
                    case GlobTokenKind.Wildcard:
                    case GlobTokenKind.BracketWildcard:
                    case GlobTokenKind.BraceExpansion:
                        items.Add(ParsePathSegment(context));
                        continue;
                }
                break;
            }

            return new PathNode(items, context.Options);
        }

        private static PathSegment ParsePathSegment(GlobParserContext context)
        {
            switch (context.CurrentToken.Kind)
            {
                case GlobTokenKind.Text:
                    return ParseText(context);
                case GlobTokenKind.CharacterWildcard:
                case GlobTokenKind.Wildcard:
                    return ParseWildcard(context);
                case GlobTokenKind.BracketWildcard:
                    return ParseBracketWildcard(context);
                case GlobTokenKind.BraceExpansion:
                    return ParseBraceExpansion(context);
            }

            throw new NotSupportedException("Unable to parse sub segment.");
        }

        private static PathSegment ParseText(GlobParserContext context)
        {
            var token = context.Accept(GlobTokenKind.Text);
            return new TextSegment(token.Value);
        }

        private static PathSegment ParseWildcard(GlobParserContext context)
        {
            var token = context.Accept(GlobTokenKind.Wildcard, GlobTokenKind.CharacterWildcard);
            return token.Kind == GlobTokenKind.Wildcard
                ? (PathSegment)new WildcardSegment()
                : new CharacterWildcardSegment();
        }

        private static PathSegment ParseBracketWildcard(GlobParserContext context)
        {
            var token = context.Accept(GlobTokenKind.BracketWildcard);
            return new BracketWildcardSegment(token.Value);
        }

        private static BraceExpansionSegment ParseBraceExpansion(GlobParserContext context)
        {
            var token = context.Accept(GlobTokenKind.BraceExpansion);
            return new BraceExpansionSegment(token.Value);
        }
    }
}