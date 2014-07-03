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
            var background = Console.BackgroundColor;
            var palette = new Dictionary<LogLevel, ColoredConsolePalette>
            {
                { LogLevel.Error, new ColoredConsolePalette(ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White) },
                { LogLevel.Warning, new ColoredConsolePalette(background, ConsoleColor.Yellow, background, ConsoleColor.Yellow) },
                { LogLevel.Information, new ColoredConsolePalette(background, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White) },
                { LogLevel.Verbose, new ColoredConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Debug, new ColoredConsolePalette(background, ConsoleColor.DarkGray, background, ConsoleColor.Gray) }
            };
            return palette;
        }

        public static IDictionary<LogLevel, ColoredConsolePalette> GetGreyscalePalette()
        {
            var background = Console.BackgroundColor;
            var palette = new Dictionary<LogLevel, ColoredConsolePalette>
            {
                { LogLevel.Error, new ColoredConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Warning, new ColoredConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Information, new ColoredConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Verbose, new ColoredConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Debug, new ColoredConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) }
            };
            return palette;
        }
    }
}
