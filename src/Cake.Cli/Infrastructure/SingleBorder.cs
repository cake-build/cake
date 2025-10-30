// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Spectre.Console;
using Spectre.Console.Rendering;

namespace Cake.Cli
{
    /// <summary>
    /// A custom Spectre.Console border class, used for outputting information about steps.
    /// </summary>
    public class SingleBorder : TableBorder
    {
        /// <summary>
        /// Gets a single instance of the SingleBorder class.
        /// </summary>
        public static SingleBorder Shared { get; } = new SingleBorder();

        /// <inheritdoc/>
        public override TableBorder SafeBorder { get; } = new Safe();

        /// <summary>
        /// Get information about the custom border.
        /// </summary>
        /// <param name="part">The part that needs a border applied to it.</param>
        /// <returns>A simple single border character.</returns>
        public override string GetPart(TableBorderPart part)
        {
            return part switch
            {
                TableBorderPart.HeaderTopLeft => "─",
                TableBorderPart.HeaderTop => "─",
                TableBorderPart.HeaderTopRight => "─",
                TableBorderPart.FooterBottomLeft => "─",
                TableBorderPart.FooterBottom => "─",
                TableBorderPart.FooterBottomRight => "─",
                _ => string.Empty,
            };
        }

        private sealed class Safe : TableBorder
        {
            public override string GetPart(TableBorderPart part)
            {
                return part switch
                {
                    TableBorderPart.HeaderTopLeft => "-",
                    TableBorderPart.HeaderTop => "-",
                    TableBorderPart.HeaderTopRight => "-",
                    TableBorderPart.FooterBottomLeft => "-",
                    TableBorderPart.FooterBottom => "-",
                    TableBorderPart.FooterBottomRight => "-",
                    _ => string.Empty,
                };
            }
        }
    }
}