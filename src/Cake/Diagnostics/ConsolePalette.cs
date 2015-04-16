using System;

namespace Cake.Diagnostics
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
    }
}
