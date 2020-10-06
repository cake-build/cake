// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Text;

namespace Cake.Frosting.Internal.Diagnostics.Formatting
{
    internal static class FormatParser
    {
        public static IEnumerable<FormatToken> Parse(string format)
        {
            var reader = new StringReader(format);
            while (true)
            {
                var current = reader.Peek();
                if (current == -1)
                {
                    break;
                }
                var character = (char)current;
                if (character == '{')
                {
                    yield return ParseProperty(reader);
                }
                else
                {
                    yield return ParseText(reader);
                }
            }
        }

        private static FormatToken ParseProperty(TextReader reader)
        {
            reader.Read(); // Consume
            if (reader.Peek() == -1)
            {
                return new LiteralToken("{");
            }
            if ((char)reader.Peek() == '{')
            {
                reader.Read();
                return new LiteralToken("{{");
            }
            var builder = new StringBuilder();
            while (true)
            {
                var current = reader.Peek();
                if (current == -1)
                {
                    break;
                }

                var character = (char)current;
                if (character == '}')
                {
                    reader.Read();

                    var accumulated = builder.ToString();
                    var parts = accumulated.Split(new[] { ':' }, StringSplitOptions.None);
                    if (parts.Length > 1)
                    {
                        var name = parts[0];
                        var format = string.Join(string.Empty, parts.Skip(1));
                        var positional = IsNumeric(name);
                        if (!positional)
                        {
                            throw new FormatException("Input string was not in a correct format.");
                        }
                        var position = int.Parse(name, CultureInfo.InvariantCulture);
                        return new PropertyToken(position, format);
                    }
                    else
                    {
                        var positional = IsNumeric(accumulated);
                        if (!positional)
                        {
                            throw new FormatException("Input string was not in a correct format.");
                        }
                        var position = int.Parse(accumulated, CultureInfo.InvariantCulture);
                        return new PropertyToken(position, null);
                    }
                }
                builder.Append((char)reader.Read());
            }
            return new LiteralToken(builder.ToString());
        }

        private static FormatToken ParseText(TextReader reader)
        {
            var builder = new StringBuilder();
            while (true)
            {
                var current = reader.Peek();
                if (current == -1)
                {
                    break;
                }
                var character = (char)current;
                if (character == '{')
                {
                    break;
                }
                builder.Append((char)reader.Read());
            }
            return new LiteralToken(builder.ToString());
        }

        private static bool IsNumeric(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }
            foreach (var character in value)
            {
                if (!char.IsDigit(character))
                {
                    return false;
                }
            }
            return true;
        }
    }
}