using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;

namespace Cake.Diagnostics
{
    internal class ColoredConsolePalette
    {
        public ConsoleColor Background { get; set; }
        public ConsoleColor Foreground { get; set; }
        public ConsoleColor ArgumentBackground { get; set; }
        public ConsoleColor ArgumentForeground { get; set; }

        public ColoredConsolePalette(ConsoleColor background, ConsoleColor foreground, 
            ConsoleColor argumentBackground, ConsoleColor argumentForeground)
        {
            Background = background;
            Foreground = foreground;
            ArgumentBackground = argumentBackground;
            ArgumentForeground = argumentForeground;
        }

        public static IDictionary<LogLevel, ColoredConsolePalette> GetColorfulPalette()
        {
            var palette = new Dictionary<LogLevel, ColoredConsolePalette>
            {
                { LogLevel.Error, new ColoredConsolePalette(ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White) },
                { LogLevel.Warning, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Yellow, ConsoleColor.Black, ConsoleColor.Yellow) },
                { LogLevel.Information, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White) },
                { LogLevel.Verbose, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White) },
                { LogLevel.Debug, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.DarkGray, ConsoleColor.Black, ConsoleColor.Gray) }
            };
            return palette;
        }

        public static IDictionary<LogLevel, ColoredConsolePalette> GetGreyscalePalette()
        {
            var palette = new Dictionary<LogLevel, ColoredConsolePalette>
            {
                { LogLevel.Error, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White) },
                { LogLevel.Warning, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White) },
                { LogLevel.Information, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White) },
                { LogLevel.Verbose, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White) },
                { LogLevel.Debug, new ColoredConsolePalette(ConsoleColor.Black, ConsoleColor.Gray, ConsoleColor.Black, ConsoleColor.White) }
            };
            return palette;
        }
    }
}
