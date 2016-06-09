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

namespace Cake.Core.IO.Globbing
{
    internal sealed class GlobParser
    {
        private readonly ICakeEnvironment _environment;

        public GlobParser(ICakeEnvironment environment)
        {
            _environment = environment;
        }

        public GlobNode Parse(string pattern, bool caseSensitive)
        {
            return Parse(new GlobParserContext(pattern, caseSensitive));
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

            // Rewrite the items into a linked list.
            var result = GlobNodeRewriter.Rewrite(items);
            GlobNodeValidator.Validate(result);
            return result;
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

                    // Get the drive from the working directory.
                    var workingDirectory = _environment.WorkingDirectory;
                    var root = workingDirectory.FullPath.Split(':').First();
                    return new WindowsRoot(root);
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
                // Is the identifier indicating a current directory?
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
            if (context.CurrentToken.Kind == GlobTokenKind.Parent)
            {
                context.Accept();
                return new ParentSegment();
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
            return new PathSegment(items, context.Options);
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
