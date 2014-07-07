using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;

namespace Cake.Diagnostics
{
    internal class ConsolePalette
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

        public static IDictionary<LogLevel, ConsolePalette> GetColorfulPalette()
        {
            var background = Console.BackgroundColor;
            var palette = new Dictionary<LogLevel, ConsolePalette>
            {
                { LogLevel.Error, new ConsolePalette(ConsoleColor.DarkRed, ConsoleColor.White, ConsoleColor.Red, ConsoleColor.White) },
                { LogLevel.Warning, new ConsolePalette(background, ConsoleColor.Yellow, background, ConsoleColor.Yellow) },
                { LogLevel.Information, new ConsolePalette(background, ConsoleColor.White, ConsoleColor.DarkBlue, ConsoleColor.White) },
                { LogLevel.Verbose, new ConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Debug, new ConsolePalette(background, ConsoleColor.DarkGray, background, ConsoleColor.Gray) }
            };
            return palette;
        }

        public static IDictionary<LogLevel, ConsolePalette> GetGreyscalePalette()
        {
            var background = Console.BackgroundColor;
            var palette = new Dictionary<LogLevel, ConsolePalette>
            {
                { LogLevel.Error, new ConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Warning, new ConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Information, new ConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Verbose, new ConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) },
                { LogLevel.Debug, new ConsolePalette(background, ConsoleColor.Gray, background, ConsoleColor.White) }
            };
            return palette;
        }
    }
}
