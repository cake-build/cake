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

        /// <inheritdoc/>
        public ConsoleColor ForegroundColor
        {
            get { return Console.ForegroundColor; }
            set { Console.ForegroundColor = value; }
        }

        /// <inheritdoc/>
        public ConsoleColor BackgroundColor
        {
            get { return Console.BackgroundColor; }
            set { Console.BackgroundColor = value; }
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void Write(string format, params object[] arg)
        {
            Console.Write(format, arg);
            Console.Out.Flush();
        }

        /// <inheritdoc/>
        public void WriteLine(string format, params object[] arg)
        {
            Console.WriteLine(format, arg);
            Console.Out.Flush();
        }

        /// <inheritdoc/>
        public void WriteError(string format, params object[] arg)
        {
            Console.Error.Write(format, arg);
            Console.Error.Flush();
        }

        /// <inheritdoc/>
        public void WriteErrorLine(string format, params object[] arg)
        {
            Console.Error.WriteLine(format, arg);
            Console.Error.Flush();
        }

        /// <inheritdoc/>
        public void ResetColor()
        {
            Console.ResetColor();
        }
    }
}