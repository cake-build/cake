// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ReSharper disable once CheckNamespace
namespace Cake.Core.Tests
{
    public static class StringExtensions
    {
        /// <summary>
        /// Removes line endings from the specified text.
        /// </summary>
        /// <param name="text">The text.</param>
        /// <returns>The text without line ending</returns>
        public static string NormalizeGeneratedCode(this string text)
        {
            return text.NormalizeLineEndings()
                .TrimEnd('\r', '\n');
        }
    }
}
