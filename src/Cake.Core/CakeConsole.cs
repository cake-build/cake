// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;

namespace Cake.Core
{
    /// <summary>
    /// The default console implementation.
    /// </summary>
    public sealed class CakeConsole : IConsole
    {
        private readonly Lazy<bool> _supportAnsiEscapeCodes;

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        /// <value>The foreground color.</value>
        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        /// <value>The background color.</value>
        public ConsoleColor BackgroundColor
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the console supports ANSI escape codes.
        /// </summary>
        public bool SupportAnsiEscapeCodes => _supportAnsiEscapeCodes.Value;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeConsole"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public CakeConsole(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }

            _supportAnsiEscapeCodes = new Lazy<bool>(() => AnsiDetector.SupportsAnsi(environment));
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// console output using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using format.</param>
        public void Write(string format, params object[] arg)
        {
            Console.Write(format, arg);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects, followed
        /// by the current line terminator, to the console output using the specified
        /// format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using format.</param>
        public void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// console error output using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using format.</param>
        public void WriteError(string format, params object[] arg)
        {
            Console.Error.Write(format, arg);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects, followed
        /// by the current line terminator, to the console error output using the
        /// specified format information.
        /// </summary>
        /// <param name="format">A composite format string.</param>
        /// <param name="arg">An array of objects to write using format.</param>
        public void WriteErrorLine(string format, params object[] arg)
        {
            Console.Error.WriteLine(format, arg);
        }

        /// <summary>
        /// Sets the foreground and background console colors to their defaults.
        /// </summary>
        public void ResetColor()
        {
            Console.ResetColor();
        }
    }
}