// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;

namespace Cake.Core.IO.Globbing
{
    internal static class GlobTokenizer
    {
        public static GlobTokenBuffer Tokenize(GlobPattern input)
        {
            return Tokenize(new StringReader(input.Pattern));
        }

        private static GlobTokenBuffer Tokenize(StringReader reader)
        {
            var tokens = new List<GlobToken>();
            while (reader.Peek() != -1)
            {
                var token = ReadToken(reader);
                if (token != null)
                {
                    tokens.Add(token);
                }
            }

            var queue = new Queue<GlobToken>(tokens);
            var result = new List<GlobToken>();
            while (queue.Count > 0)
            {
                var current = queue.Peek();
                if (current.Kind == GlobTokenKind.Text)
                {
                    var accumulator = new List<GlobToken>();
                    while (queue.Count > 0)
                    {
                        var item = queue.Peek();
                        if (item.Kind != GlobTokenKind.Text)
                        {
                            break;
                        }
                        accumulator.Add(queue.Dequeue());
                    }
                    result.Add(new GlobToken(GlobTokenKind.Text, string.Join(string.Empty, accumulator.Select(i => i.Value))));
                }
                else
                {
                    result.Add(queue.Dequeue());
                }
            }

            // Turn the tokens into a token buffer.
            return new GlobTokenBuffer(result);
        }

        private static GlobToken ReadToken(StringReader reader)
        {
            char current = (char)reader.Peek();

            if (current == '?')
            {
                reader.Read();
                return new GlobToken(GlobTokenKind.CharacterWildcard, "?");
            }
            else if (current == '*')
            {
                reader.Read();
                return new GlobToken(GlobTokenKind.Wildcard, "*");
            }
            else if (current == '.')
            {
                reader.Read();
                if (reader.Peek() != -1)
                {
                    var next = (char)reader.Peek();
                    if (next == '/' || next == '\\')
                    {
                        return new GlobToken(GlobTokenKind.Current, ".");
                    }
                    if (next == '.')
                    {
                        reader.Read();
                        return new GlobToken(GlobTokenKind.Parent, "..");
                    }
                }
                return new GlobToken(GlobTokenKind.Text, current.ToString());
            }
            else if (current == ':')
            {
                reader.Read();
                return new GlobToken(GlobTokenKind.WindowsRoot, ":");
            }
            else if (current == '[')
            {
                return ReadScope(reader, GlobTokenKind.BracketWildcard, '[', ']');
            }
            else if (current == '{')
            {
                return ReadScope(reader, GlobTokenKind.BraceExpansion, '{', '}');
            }
            else if (current == '\\' || current == '/')
            {
                reader.Read();
                return new GlobToken(GlobTokenKind.PathSeparator, "/");
            }

            reader.Read();
            return new GlobToken(GlobTokenKind.Text, current.ToString());
        }

        private static GlobToken ReadScope(StringReader reader, GlobTokenKind kind, char first, char last)
        {
            char current = (char)reader.Read();
            Debug.Assert(current == first, "Unexpected token.");

            var accumulator = new StringBuilder();
            while (reader.Peek() != -1)
            {
                current = (char)reader.Peek();
                if (current == last)
                {
                    break;
                }
                accumulator.Append((char)reader.Read());
            }

            if (reader.Peek() == -1)
            {
                throw new InvalidOperationException($"Expected '{last}' but reached end of pattern.");
            }

            // Consume the last character.
            current = (char)reader.Read();
            Debug.Assert(current == last, "Unexpected token.");

            return new GlobToken(kind, accumulator.ToString());
        }
    }
}
