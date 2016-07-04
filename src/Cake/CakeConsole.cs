// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Threading;
using System.Threading.Tasks;
using Cake.Core;

namespace Cake
{
    internal sealed class CakeConsole : IConsole
    {
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
        /// Writes the text representation of the specified array of objects to the
        /// console output using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
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
        /// <param name="format">A composite format string</param>
        /// <param name="arg">An array of objects to write using format.</param>
        public void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// console error output using the specified format information.
        /// </summary>
        /// <param name="format">A composite format string</param>
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
        /// <param name="format">A composite format string</param>
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

        /// <summary>
        /// Gets a value indicating whether a key press is available in the input stream.
        /// </summary>
        /// <returns>true if a key press is available; otherwise, false.</returns>
        public bool KeyAvailable
        {
            get { return Console.KeyAvailable; }
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
            return Console.ReadKey(intercept);
        }
    }
}
