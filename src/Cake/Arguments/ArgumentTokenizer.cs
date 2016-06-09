// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cake.Arguments
{
    internal static class ArgumentTokenizer
    {
        public static IEnumerable<string> Tokenize(string arguments)
        {
            return Tokenize(new StringReader(arguments));
        }

        private static IEnumerable<string> Tokenize(StringReader reader)
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
