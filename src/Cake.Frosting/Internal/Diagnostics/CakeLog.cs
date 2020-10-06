// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Frosting.Internal.Diagnostics.Formatting;

namespace Cake.Frosting.Internal.Diagnostics
{
    internal sealed class CakeLog : ICakeLog
    {
        private readonly IConsole _console;
        private readonly object _lock;
        private readonly IDictionary<LogLevel, ConsolePalette> _palettes;

        public Verbosity Verbosity { get; set; }

        public CakeLog(IConsole console, Verbosity verbosity = Verbosity.Normal)
        {
            _console = console;
            _lock = new object();
            _palettes = CreatePalette();
            Verbosity = verbosity;
        }

        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            if (verbosity > Verbosity)
            {
                return;
            }
            lock (_lock)
            {
                try
                {
                    var palette = _palettes[level];
                    var tokens = FormatParser.Parse(format);
                    foreach (var token in tokens)
                    {
                        SetPalette(token, palette);
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
                    if (level > LogLevel.Error)
                    {
                        _console.WriteLine();
                    }
                    else
                    {
                        _console.WriteErrorLine();
                    }
                }
            }
        }

        private void SetPalette(FormatToken token, ConsolePalette palette)
        {
            var property = token as PropertyToken;
            if (property != null)
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

        private IDictionary<LogLevel, ConsolePalette> CreatePalette()
        {
            var background = _console.BackgroundColor;
            var palette = new Dictionary<LogLevel, ConsolePalette>
            {
                { LogLevel.Fatal, new ConsolePalette(ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.White) },
                { LogLevel.Error, new ConsolePalette(ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White) },
                { LogLevel.Warning, new ConsolePalette(background, ConsoleColor.Yellow, background, ConsoleColor.Yellow) },
                { LogLevel.Information, new ConsolePalette(background, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White) },
                { LogLevel.Verbose, new ConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Debug, new ConsolePalette(background, ConsoleColor.DarkGray, background, ConsoleColor.Gray) }
            };
            return palette;
        }
    }
}