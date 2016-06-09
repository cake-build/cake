// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="IConsole"/>.
    /// </summary>
    public static class ConsoleExtensions
    {
        /// <summary>
        /// Writes an empty line to the console output.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        public static void WriteLine(this IConsole console)
        {
            if (console != null)
            {
                console.WriteLine(string.Empty);
            }
        }

        /// <summary>
        /// Writes an empty line to the console error output.
        /// </summary>
        /// <param name="console">The console to write to.</param>
        public static void WriteErrorLine(this IConsole console)
        {
            if (console != null)
            {
                console.WriteErrorLine(string.Empty);
            }
        }
    }
}
