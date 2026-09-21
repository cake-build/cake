// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="System.String"/>.
    /// </summary>
    public static class StringExtensions
    {
        /// <summary>
        /// Quotes the specified <see cref="System.String"/> as a process argument literal.
        /// Trailing backslashes and embedded quotes are escaped so a standard Windows
        /// argv parser recovers the original value.
        /// </summary>
        /// <param name="value">The literal string to quote. Already-quoted tokens are left unchanged.</param>
        /// <returns>A quoted string.</returns>
        public static string Quote(this string value)
        {
            if (ProcessArgumentEscaper.IsQuoted(value))
            {
                return value;
            }

            return ProcessArgumentEscaper.Escape(value, alwaysQuote: true);
        }

        /// <summary>
        /// Unquotes a process argument token produced by <see cref="Quote"/> /
        /// <see cref="ProcessArgumentEscaper.Escape"/>.
        /// Trailing backslashes and embedded quotes are unescaped; this is not a naive trim of <c>"</c>.
        /// </summary>
        /// <param name="value">The string to unquote.</param>
        /// <returns>The literal argument value.</returns>
        public static string UnQuote(this string value)
        {
            return ProcessArgumentEscaper.Unquote(value);
        }

        /// <summary>
        /// Splits the <see cref="String"/> into lines.
        /// </summary>
        /// <param name="content">The string to split.</param>
        /// <returns>The lines making up the provided string.</returns>
        public static string[] SplitLines(this string content)
        {
            content = NormalizeLineEndings(content);
            return content.Split(new[] { "\r\n" }, StringSplitOptions.None);
        }

        /// <summary>
        /// Normalizes the line endings in a <see cref="String"/>.
        /// </summary>
        /// <param name="value">The string to normalize line endings in.</param>
        /// <returns>A <see cref="String"/> with normalized line endings.</returns>
        public static string NormalizeLineEndings(this string value)
        {
            if (value != null)
            {
                value = value.Replace("\r\n", "\n");
                value = value.Replace("\r", string.Empty);
                return value.Replace("\n", "\r\n");
            }
            return string.Empty;
        }
    }
}