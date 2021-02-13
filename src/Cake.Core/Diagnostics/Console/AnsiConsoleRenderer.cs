// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics.Formatting;

namespace Cake.Core.Diagnostics
{
    internal sealed class AnsiConsoleRenderer : IConsoleRenderer
    {
        private const string ResetEscapeCode = "\u001b[0m";

        private readonly IConsole _console;
        private readonly IDictionary<LogLevel, ConsolePalette> _palette;
        private readonly IDictionary<ConsoleColor, string> _ansiLookup;
        private readonly IDictionary<ConsoleColor, string> _ansiBackgroundLookup;

        public AnsiConsoleRenderer(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException(nameof(console));
            _palette = ConsolePalette.CreateLookup(_console);

            _ansiLookup = new Dictionary<ConsoleColor, string>
            {
                { Constants.DefaultConsoleColor, string.Empty }, // Default
                { ConsoleColor.DarkGray, "\u001b[30;1m" },  // Bright black
                { ConsoleColor.Red, "\u001b[31;1m" },       // Bright red
                { ConsoleColor.Green, "\u001b[32;1m" },     // Bright green
                { ConsoleColor.Yellow, "\u001b[33;1m" },    // Bright yellow
                { ConsoleColor.Blue, "\u001b[34;1m" },      // Bright blue
                { ConsoleColor.Magenta, "\u001b[35;1m" },   // Bright magenta
                { ConsoleColor.Cyan, "\u001b[36;1m" },      // Bright cyan
                { ConsoleColor.White, "\u001b[37;1m" },     // Bright white
                { ConsoleColor.Black, "\u001b[30m" },       // Black
                { ConsoleColor.DarkRed, "\u001b[31m" },     // Red
                { ConsoleColor.DarkGreen, "\u001b[32m" },   // Green
                { ConsoleColor.DarkYellow, "\u001b[33m" },  // Yellow
                { ConsoleColor.DarkBlue, "\u001b[34m" },    // Blue
                { ConsoleColor.DarkMagenta, "\u001b[35m" }, // Magenta
                { ConsoleColor.DarkCyan, "\u001b[36m" },    // Cyan
                { ConsoleColor.Gray, "\u001b[37m" },        // White
            };

            _ansiBackgroundLookup = new Dictionary<ConsoleColor, string>
            {
                { Constants.DefaultConsoleColor, string.Empty }, // Default
                { ConsoleColor.DarkGray, "\u001b[40;1m" },  // Bright black
                { ConsoleColor.Red, "\u001b[41;1m" },       // Bright red
                { ConsoleColor.Green, "\u001b[42;1m" },     // Bright green
                { ConsoleColor.Yellow, "\u001b[43;1m" },    // Bright yellow
                { ConsoleColor.Blue, "\u001b[44;1m" },      // Bright blue
                { ConsoleColor.Magenta, "\u001b[45;1m" },   // Bright magenta
                { ConsoleColor.Cyan, "\u001b[46;1m" },      // Bright cyan
                { ConsoleColor.White, "\u001b[47;1m" },     // Bright white
                { ConsoleColor.Black, "\u001b[40m" },       // Black
                { ConsoleColor.DarkRed, "\u001b[41m" },     // Red
                { ConsoleColor.DarkGreen, "\u001b[42m" },   // Green
                { ConsoleColor.DarkYellow, "\u001b[43m" },  // Yellow
                { ConsoleColor.DarkBlue, "\u001b[44m" },    // Blue
                { ConsoleColor.DarkMagenta, "\u001b[45m" }, // Magenta
                { ConsoleColor.DarkCyan, "\u001b[46m" },    // Cyan
                { ConsoleColor.Gray, "\u001b[47m" },        // White
            };
        }

        public void Render(LogLevel level, string format, params object[] args)
        {
            var palette = _palette[level];
            var tokens = FormatParser.Parse(format);

            var colorize = !"{0}".Equals(format, StringComparison.Ordinal);

            foreach (var token in tokens)
            {
                if (colorize)
                {
                    var colorEscapeCode = GetColorEscapeCode(token, palette);
                    var content = token.Render(args);

                    if (level > LogLevel.Error)
                    {
                        _console.Write("{0}", $"{colorEscapeCode}{content}{ResetEscapeCode}");
                    }
                    else
                    {
                        _console.WriteError("{0}", $"{colorEscapeCode}{content}{ResetEscapeCode}");
                    }
                }
                else
                {
                    // Render without colorization.
                    if (level > LogLevel.Error)
                    {
                        _console.Write("{0}", token.Render(args));
                    }
                    else
                    {
                        _console.WriteError("{0}", token.Render(args));
                    }
                }
            }

            // Append a new line
            if (level > LogLevel.Error)
            {
                _console.WriteLine();
            }
            else
            {
                _console.WriteErrorLine();
            }
        }

        private string GetColorEscapeCode(FormatToken token, ConsolePalette palette)
        {
            if (token is PropertyToken)
            {
                var result = _ansiBackgroundLookup[palette.ArgumentBackground];
                result += _ansiLookup[palette.ArgumentForeground];
                return result;
            }
            else
            {
                var result = _ansiBackgroundLookup[palette.Background];
                result += _ansiLookup[palette.Foreground];
                return result;
            }
        }
    }
}
