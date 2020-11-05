// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Frosting.Internal.Diagnostics
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