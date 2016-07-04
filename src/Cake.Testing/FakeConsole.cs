// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;

namespace Cake.Testing
{
    /// <summary>
    /// Implementation of a fake <see cref="IConsole"/>.
    /// </summary>
    public sealed class FakeConsole : IConsole
    {
        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the keys.
        /// </summary>
        /// <value>The keys.</value>
        public Queue<ConsoleKeyInfo> Keys { get; set; }

        /// <summary>
        /// Gets or sets the error messages.
        /// </summary>
        /// <value>The messages.</value>
        public List<string> ErrorMessages { get; set; }

        /// <summary>
        /// Gets or sets the foreground color.
        /// </summary>
        /// <value>The foreground color</value>
        public ConsoleColor ForegroundColor { get; set; }

        /// <summary>
        /// Gets or sets the background color.
        /// </summary>
        /// <value>The background color</value>
        public ConsoleColor BackgroundColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeConsole"/> class.
        /// </summary>
        public FakeConsole()
        {
            Messages = new List<string>();
            Keys = new Queue<ConsoleKeyInfo>();
            ErrorMessages = new List<string>();
            ForegroundColor = ConsoleColor.Gray;
            BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// console output using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">An array of objects to write using format.</param>
        public void Write(string format, params object[] arg)
        {
            Messages.Add(string.Format(format, arg));
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects, followed
        /// by the current line terminator, to the console output using the specified
        /// format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">An array of objects to write using format.</param>
        public void WriteLine(string format, params object[] arg)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                Messages.Add(string.Format(format, arg));
            }
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// console error output using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">An array of objects to write using format.</param>
        public void WriteError(string format, params object[] arg)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                ErrorMessages.Add(string.Format(format, arg));
            }
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects, followed
        /// by the current line terminator, to the console error output using the
        /// specified format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
        /// <param name="arg">An array of objects to write using format.</param>
        public void WriteErrorLine(string format, params object[] arg)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                ErrorMessages.Add(string.Format(format, arg));
            }
        }

        /// <summary>
        /// Sets the foreground and background console colors to their defaults.
        /// </summary>
        public void ResetColor()
        {
            ForegroundColor = ConsoleColor.Gray;
            BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        /// <returns>true if a key press is available; otherwise, false.</returns>
        public bool KeyAvailable
        {
            get { return Keys.Any(); }
        }

        /// <summary>
        /// Obtains the next character or function key pressed by the user.
        /// The pressed key is optionally displayed in the console window.
        /// </summary>
        /// <param name="intercept">Determines whether to display the pressed key in the console window.
        /// true to not display the pressed key; otherwise, false.</param>
        /// <returns>A System.ConsoleKeyInfo object that describes the System.ConsoleKey constant
        /// and Unicode character, if any, that correspond to the pressed console key. The
        /// System.ConsoleKeyInfo object also describes, in a bitwise combination of System.ConsoleModifiers
        /// values, whether one or more Shift, Alt, or Ctrl modifier keys was pressed simultaneously
        /// with the console key.</returns>
        public ConsoleKeyInfo ReadKey(bool intercept)
        {
            return Keys.Dequeue();
        }
    }
}
