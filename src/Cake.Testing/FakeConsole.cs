// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Text;
using Cake.Core;

namespace Cake.Testing
{
    /// <summary>
    /// Implementation of a fake <see cref="IConsole"/>.
    /// </summary>
    public sealed class FakeConsole : IConsole
    {
        private readonly StringBuilder _builder;
        private readonly StringBuilder _errorBuilder;

        /// <summary>
        /// Gets or sets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public List<string> Messages { get; set; }

        /// <summary>
        /// Gets or sets the error messages.
        /// </summary>
        /// <value>The messages.</value>
        public List<string> ErrorMessages { get; set; }

        /// <inheritdoc/>
        public ConsoleColor ForegroundColor { get; set; }

        /// <inheritdoc/>
        public ConsoleColor BackgroundColor { get; set; }

        /// <inheritdoc/>
        public bool SupportAnsiEscapeCodes { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether console color should be added to the text
        /// string if <see cref="SupportAnsiEscapeCodes"/> is set to <c>false</c>.
        /// </summary>
        public bool OutputConsoleColor { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeConsole"/> class.
        /// </summary>
        public FakeConsole()
        {
            _builder = new StringBuilder();
            _errorBuilder = new StringBuilder();

            Messages = new List<string>();
            ErrorMessages = new List<string>();
            ForegroundColor = ConsoleColor.Gray;
            BackgroundColor = ConsoleColor.Black;
        }

        /// <summary>
        /// Creates a new fake console that supports ANSI escape codes.
        /// </summary>
        /// <returns>The created <see cref="FakeConsole"/>.</returns>
        public static FakeConsole CreateAnsiConsole()
        {
            return new FakeConsole
            {
                SupportAnsiEscapeCodes = true
            };
        }

        /// <inheritdoc/>
        public void Write(string format, params object[] arg)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                var message = string.Format(CultureInfo.InvariantCulture, format, arg);

                if (OutputConsoleColor && !SupportAnsiEscapeCodes)
                {
                    var formatted = string.Format("#[{0}|{1}]{2}[/]", BackgroundColor, ForegroundColor, message);
                    _builder.Append(formatted);
                }
                else
                {
                    _builder.Append(message);
                }
            }
        }

        /// <inheritdoc/>
        public void WriteLine(string format, params object[] arg)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                Write(format, arg);
            }

            Messages.Add(_builder.ToString());
            _builder.Clear();
        }

        /// <inheritdoc/>
        public void WriteError(string format, params object[] arg)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                var message = string.Format(CultureInfo.InvariantCulture, format, arg);

                if (OutputConsoleColor && !SupportAnsiEscapeCodes)
                {
                    var formatted = string.Format("#[{0}|{1}]{2}[/]", BackgroundColor, ForegroundColor, message);
                    _errorBuilder.Append(formatted);
                }
                else
                {
                    _errorBuilder.Append(message);
                }
            }
        }

        /// <inheritdoc/>
        public void WriteErrorLine(string format, params object[] arg)
        {
            if (!string.IsNullOrWhiteSpace(format))
            {
                WriteError(format, arg);
            }

            ErrorMessages.Add(_errorBuilder.ToString());
            _errorBuilder.Clear();
        }

        /// <inheritdoc/>
        public void ResetColor()
        {
            ForegroundColor = ConsoleColor.Gray;
            BackgroundColor = ConsoleColor.Black;
        }
    }
}