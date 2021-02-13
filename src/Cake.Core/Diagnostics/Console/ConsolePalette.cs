// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;

namespace Cake.Core.Diagnostics
{
    internal sealed class ConsolePalette
    {
        public ConsoleColor Background { get; set; }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor ArgumentBackground { get; set; }
        public ConsoleColor ArgumentForeground { get; set; }

        public ConsolePalette(ConsoleColor background, ConsoleColor foreground,
            ConsoleColor argumentBackground, ConsoleColor argumentForeground)
        {
            Background = background;
            Foreground = foreground;
            ArgumentBackground = argumentBackground;
            ArgumentForeground = argumentForeground;
        }

        public static IDictionary<LogLevel, ConsolePalette> CreateLookup(IConsole console)
        {
            var background = console.BackgroundColor;
            if ((int)background < 0 || console.SupportAnsiEscapeCodes)
            {
                background = Constants.DefaultConsoleColor;
            }

            return new Dictionary<LogLevel, ConsolePalette>
            {
                { LogLevel.Fatal, new ConsolePalette(ConsoleColor.Magenta, ConsoleColor.White, ConsoleColor.DarkMagenta, ConsoleColor.White) },
                { LogLevel.Error, new ConsolePalette(ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White) },
                { LogLevel.Warning, new ConsolePalette(background, ConsoleColor.Yellow, background, ConsoleColor.Yellow) },
                { LogLevel.Information, new ConsolePalette(background, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White) },
                { LogLevel.Verbose, new ConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Debug, new ConsolePalette(background, ConsoleColor.DarkGray, background, ConsoleColor.Gray) }
            };
        }
    }
}