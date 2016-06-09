// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core
{
    /// <summary>
    /// Represents console output.
    /// </summary>
    public interface IConsole
    {
        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        /// <value>The foreground color</value>
        ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        /// <value>The background color</value>
        ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// console output using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">An array of objects to write using format.</param>
        void Write(string format, params object[] arg);

        /// <summary>
        /// Writes the text representation of the specified array of objects, followed
        /// by the current line terminator, to the console output using the specified
        /// format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">An array of objects to write using format.</param>
        void WriteLine(string format, params object[] arg);

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// console error output using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">An array of objects to write using format.</param>
        void WriteError(string format, params object[] arg);

        /// <summary>
        /// Writes the text representation of the specified array of objects, followed
        /// by the current line terminator, to the console error output using the
        /// specified format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">An array of objects to write using format.</param>
        void WriteErrorLine(string format, params object[] arg);

        /// <summary>
        /// Sets the foreground and background console colors to their defaults.
        /// </summary>
        void ResetColor();
    }
}
