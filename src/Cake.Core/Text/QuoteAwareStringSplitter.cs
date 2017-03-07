// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cake.Core.Text
{
    /// <summary>
    /// Utility that that respect quotes when splitting a string.
    /// </summary>
    public static class QuoteAwareStringSplitter
    {
        /// <summary>
        /// Splits the provided string on spaces while respecting quoted strings.
        /// </summary>
        /// <param name="text">The string to split.</param>
        /// <returns>The split, individual parts.</returns>
        public static IEnumerable<string> Split(string text)
        {
            return Split(new StringReader(text));
        }

        private static IEnumerable<string> Split(StringReader reader)
        {
            while (reader.Peek() != -1)
            {
                var character = (char)reader.Peek();
                switch (character)
                {
                    case '\"':
                    yield return ReadQuote(reader);
                    break;
                    case ' ':
                    reader.Read();
                    break;
                    default:
                    yield return Read(reader);
                    break;
                }
            }
        }

        private static string ReadQuote(StringReader reader)
        {
            var accumulator = new StringBuilder();
            accumulator.Append((char)reader.Read());
            while (reader.Peek() != -1)
            {
                var character = (char)reader.Peek();
                if (character == '\"')
                {
                    accumulator.Append((char)reader.Read());
                    break;
                }
                reader.Read();
                accumulator.Append(character);
            }
            return accumulator.ToString();
        }

        private static string Read(StringReader reader)
        {
            var accumulator = new StringBuilder();
            accumulator.Append((char)reader.Read());
            while (reader.Peek() != -1)
            {
                if ((char)reader.Peek() == '\"')
                {
                    accumulator.Append(ReadQuote(reader));
                }
                else if ((char)reader.Peek() == ' ')
                {
                    break;
                }
                else
                {
                    accumulator.Append((char)reader.Read());
                }
            }
            return accumulator.ToString();
        }
    }
}