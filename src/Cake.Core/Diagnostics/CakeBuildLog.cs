// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// A log that writes to the console.
    /// </summary>
    public sealed class CakeBuildLog : ICakeLog
    {
        private readonly IConsole _console;
        private readonly object _lock;
        private readonly IConsoleRenderer _renderer;

        /// <summary>
        /// Gets or sets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeBuildLog" /> class.
        /// </summary>
        /// <param name="console">The console.</param>
        /// <param name="verbosity">The verbosity.</param>
        public CakeBuildLog(IConsole console, Verbosity verbosity = Verbosity.Normal)
        {
            _console = console;
            _lock = new object();

            if (_console.SupportAnsiEscapeCodes)
            {
                // Use ANSI console renderer.
                _renderer = new AnsiConsoleRenderer(_console);
            }
            else
            {
                // Use the default renderer.
                _renderer = new ConsoleRenderer(_console);
            }

            Verbosity = verbosity;
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// log using the specified verbosity, log level and format information.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            if (verbosity > Verbosity)
            {
                return;
            }

            lock (_lock)
            {
                _renderer.Render(level, format, args);
            }
        }
    }
}