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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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