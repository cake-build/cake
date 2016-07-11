// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;

namespace Cake.Core.Configuration.Parser
{
    internal static class ConfigurationTokenizer
    {
        public static ConfigurationTokenStream Tokenize(string text)
        {
            var result = new List<ConfigurationToken>();
            var reader = new StringReader(text);
            while (reader.Peek() != -1)
            {
                var token = Tokenize(reader);
                if (token != null)
                {
                    result.Add(token);
                }
            }
            return new ConfigurationTokenStream(result);
        }

        private static ConfigurationToken Tokenize(StringReader reader)
        {
            EatWhitespace(reader);
            if (reader.Peek() == -1)
            {
                return null;
            }

            var character = (char)reader.Peek();
            switch (character)
            {
                case ';':
                    reader.Read();
                    SkipComment(reader);
                    return null;
                case '[':
                    reader.Read();
                    return ReadSection(reader);
                case '=':
                    reader.Read();
                    return new ConfigurationToken(ConfigurationTokenKind.Equals, "=");
            }

            return ReadValue(reader);
        }

        private static void EatWhitespace(TextReader reader)
        {
            int ch;
            while ((ch = reader.Peek()) != -1)
            {
                if (!char.IsWhiteSpace((char)ch))
                {
                    break;
                }
                reader.Read();
            }
        }

        private static void SkipComment(StringReader reader)
        {
            while (reader.Peek() != -1)
            {
                var token = (char)reader.Read();
                if (token == '\n' || token == '\r')
                {
                    break;
                }
            }
        }

        private static ConfigurationToken ReadSection(StringReader reader)
        {
            var accumulator = new StringBuilder();
            while (reader.Peek() != -1)
            {
                var character = (char)reader.Read();
                if (character == ']')
                {
                    return new ConfigurationToken(ConfigurationTokenKind.Section, accumulator.ToString());
                }
                accumulator.Append(character);
            }
            throw new InvalidOperationException("Encountered malformed section.");
        }

        private static ConfigurationToken ReadValue(StringReader reader)
        {
            var accumulator = new StringBuilder();
            while (reader.Peek() != -1)
            {
                var character = (char)reader.Peek();
                if (character == '=' || character == '\n' || character == '\r')
                {
                    break;
                }
                reader.Read();
                accumulator.Append(character);
            }
            return new ConfigurationToken(ConfigurationTokenKind.Value,  accumulator.ToString());
        }
    }
}
