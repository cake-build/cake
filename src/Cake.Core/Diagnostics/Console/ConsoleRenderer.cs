// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics.Formatting;

namespace Cake.Core.Diagnostics
{
    internal sealed class ConsoleRenderer : IConsoleRenderer
    {
        private readonly IConsole _console;
        private readonly IDictionary<LogLevel, ConsolePalette> _palette;

        public ConsoleRenderer(IConsole console)
        {
            _console = console ?? throw new ArgumentNullException();
            _palette = ConsolePalette.CreateLookup(_console);
        }

        public void Render(LogLevel level, string format, params object[] args)
        {
            try
            {
                var palette = _palette[level];
                var tokens = FormatParser.Parse(format);

                var colorize = !"{0}".Equals(format, StringComparison.OrdinalIgnoreCase);

                foreach (var token in tokens)
                {
                    SetPalette(token, palette, colorize);

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
            finally
            {
                _console.ResetColor();
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

        private void SetPalette(FormatToken token, ConsolePalette palette, bool colorize)
        {
            if (colorize && token is PropertyToken)
            {
                _console.BackgroundColor = palette.ArgumentBackground;
                _console.ForegroundColor = palette.ArgumentForeground;
            }
            else
            {
                _console.BackgroundColor = palette.Background;
                _console.ForegroundColor = palette.Foreground;
            }
        }
    }
}
