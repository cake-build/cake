// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Text;

namespace Cake.Core.IO;

/// <summary>
/// Escapes process argument values so a standard Windows argv parser
/// (<c>CommandLineToArgvW</c> / .NET <c>PasteArguments</c>) recovers the original string.
/// </summary>
public static class ProcessArgumentEscaper
{
    private const char Quote = '"';
    private const char Backslash = '\\';

    /// <summary>
    /// Escapes a literal argument value for inclusion on a process command line.
    /// </summary>
    /// <param name="value">The literal argument value. This should not already be quoted.</param>
    /// <param name="alwaysQuote">
    /// If set to <c>true</c>, the result is always wrapped in quotes (except that
    /// <see langword="null"/> and empty still become <c>""</c>).
    /// </param>
    /// <returns>The escaped argument text.</returns>
    public static string Escape(string value, bool alwaysQuote = false)
    {
        if (string.IsNullOrEmpty(value))
        {
            return "\"\"";
        }

        if (ContainsNoWhitespaceOrQuotes(value))
        {
            if (!alwaysQuote)
            {
                return value;
            }

            if (value[value.Length - 1] != Backslash)
            {
                return string.Concat("\"", value, "\"");
            }
        }

        var builder = new StringBuilder(value.Length + 8);
        builder.Append(Quote);

        var index = 0;
        while (index < value.Length)
        {
            var c = value[index++];
            if (c == Backslash)
            {
                var backslashCount = 1;
                while (index < value.Length && value[index] == Backslash)
                {
                    index++;
                    backslashCount++;
                }

                if (index == value.Length)
                {
                    builder.Append(Backslash, backslashCount * 2);
                }
                else if (value[index] == Quote)
                {
                    builder.Append(Backslash, (backslashCount * 2) + 1);
                    builder.Append(Quote);
                    index++;
                }
                else
                {
                    builder.Append(Backslash, backslashCount);
                }

                continue;
            }

            if (c == Quote)
            {
                builder.Append(Backslash);
                builder.Append(Quote);
                continue;
            }

            builder.Append(c);
        }

        builder.Append(Quote);
        return builder.ToString();
    }

    /// <summary>
    /// Returns the literal value encoded by <see cref="Escape"/>, or <paramref name="value"/>
    /// unchanged when it is not a quoted token produced by this escaper.
    /// </summary>
    /// <param name="value">A rendered argument token.</param>
    /// <returns>The literal argument value.</returns>
    public static string Unquote(string value)
    {
        if (string.IsNullOrEmpty(value) || !IsQuoted(value))
        {
            return value ?? string.Empty;
        }

        var builder = new StringBuilder(value.Length - 2);
        var index = 1;
        var last = value.Length - 1;
        while (index < last)
        {
            var c = value[index++];
            if (c != Backslash)
            {
                builder.Append(c);
                continue;
            }

            var backslashCount = 1;
            while (index < last && value[index] == Backslash)
            {
                index++;
                backslashCount++;
            }

            if (index == last)
            {
                builder.Append(Backslash, backslashCount / 2);
                break;
            }

            if (value[index] == Quote)
            {
                builder.Append(Backslash, backslashCount / 2);
                builder.Append(Quote);
                index++;
                continue;
            }

            builder.Append(Backslash, backslashCount);
        }

        return builder.ToString();
    }

    /// <summary>
    /// Determines whether <paramref name="value"/> is already wrapped in quotes whose
    /// closing quote is not escaped by a trailing backslash.
    /// </summary>
    /// <param name="value">The string to inspect.</param>
    /// <returns><c>true</c> if the value is an already-quoted token; otherwise, <c>false</c>.</returns>
    public static bool IsQuoted(string value)
    {
        if (string.IsNullOrEmpty(value) || value.Length < 2)
        {
            return false;
        }

        if (value[0] != Quote || value[value.Length - 1] != Quote)
        {
            return false;
        }

        var backslashCount = 0;
        for (var index = value.Length - 2; index >= 1; index--)
        {
            if (value[index] != Backslash)
            {
                break;
            }

            backslashCount++;
        }

        return backslashCount % 2 == 0;
    }

    private static bool ContainsNoWhitespaceOrQuotes(string value)
    {
        foreach (var c in value)
        {
            if (char.IsWhiteSpace(c) || c == Quote)
            {
                return false;
            }
        }

        return true;
    }
}
