// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core;

/// <summary>
/// Represents console output.
/// </summary>
public interface IConsole
{
    /// <summary>
    /// Gets or sets the foreground color.
    /// </summary>
    /// <value>The foreground color.</value>
    ConsoleColor ForegroundColor { get; set; }

    /// <summary>
    /// Gets or sets the background color.
    /// </summary>
    /// <value>The background color.</value>
    ConsoleColor BackgroundColor { get; set; }

    /// <summary>
    /// Gets a value indicating whether or not the console supports ANSI escape codes.
    /// </summary>
    bool SupportAnsiEscapeCodes { get; }

    /// <summary>
    /// Writes the text representation of the specified array of objects to the
    /// console output using the specified format information.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg">An array of objects to write using format.</param>
    void Write(string format, params object[] arg);

    /// <summary>
    /// Writes the specified string value to the console output without parsing it as a format string.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void Write(string value) => Write("{0}", value);

    /// <summary>
    /// Writes the text representation of the specified array of objects, followed
    /// by the current line terminator, to the console output using the specified
    /// format information.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg">An array of objects to write using format.</param>
    void WriteLine(string format, params object[] arg);

    /// <summary>
    /// Writes the specified string value, followed by the current line terminator,
    /// to the console output without parsing it as a format string.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void WriteLine(string value) => WriteLine("{0}", value);

    /// <summary>
    /// Writes the text representation of the specified array of objects to the
    /// console error output using the specified format information.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg">An array of objects to write using format.</param>
    void WriteError(string format, params object[] arg);

    /// <summary>
    /// Writes the specified string value to the console error output without parsing it as a format string.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void WriteError(string value) => WriteError("{0}", value);

    /// <summary>
    /// Writes the text representation of the specified array of objects, followed
    /// by the current line terminator, to the console error output using the
    /// specified format information.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="arg">An array of objects to write using format.</param>
    void WriteErrorLine(string format, params object[] arg);

    /// <summary>
    /// Writes the specified string value, followed by the current line terminator,
    /// to the console error output without parsing it as a format string.
    /// </summary>
    /// <param name="value">The value to write.</param>
    void WriteErrorLine(string value) => WriteErrorLine("{0}", value);

    /// <summary>
    /// Sets the foreground and background console colors to their defaults.
    /// </summary>
    void ResetColor();
}
