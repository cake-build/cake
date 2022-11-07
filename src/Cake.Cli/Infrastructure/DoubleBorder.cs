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
    public class DoubleBorder : TableBorder
    {
        /// <summary>
        /// Gets a single instance of the DoubleBorder class.
        /// </summary>
        public static DoubleBorder Shared { get; } = new DoubleBorder();

        /// <summary>
        /// Get information about the custom border.
        /// </summary>
        /// <param name="part">The part that needs a border applied to it.</param>
        /// <returns>A simple double border character.</returns>
        public override string GetPart(TableBorderPart part)
        {
            return part switch
            {
                TableBorderPart.HeaderTop => "=",
                TableBorderPart.FooterBottom => "=",
                _ => string.Empty,
            };
        }
    }
}