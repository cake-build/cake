using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Diagnostics.Formatting;

namespace Cake.Diagnostics
{
    internal sealed class ConsoleBuildLog : ICakeLog
    {
        private readonly object _lock;
        private readonly IDictionary<LogLevel, ConsolePalette> _palettes;

        public ConsoleBuildLog(bool grayscale = false)
        {
            _lock = new object();
            _palettes = GetPalette(grayscale);
        }

        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            lock (_lock)
            {
                try
                {
                    var palette = _palettes[level];
                    var tokens = FormatParser.Parse(format);
                    foreach (var token in tokens)
                    {
                        SetPalette(token, palette);
                        Console.Write(token.Render(args));
                    }
                    Console.Write(Environment.NewLine);
                }
                finally
                {
                    Console.ResetColor();
                }
            }
        }

        private static void SetPalette(FormatToken token, ConsolePalette palette)
        {
            var property = token as PropertyToken;
            if (property != null)
            {
                Console.BackgroundColor = palette.ArgumentBackground;
                Console.ForegroundColor = palette.ArgumentForeground;
            }
            else
            {
                Console.BackgroundColor = palette.Background;
                Console.ForegroundColor = palette.Foreground;
            }
        }

        private static IDictionary<LogLevel, ConsolePalette> GetPalette(bool grayscale)
        {
            return grayscale 
                ? ConsolePalette.GetGreyscalePalette() 
                : ConsolePalette.GetColorfulPalette();
        }
    }
}
