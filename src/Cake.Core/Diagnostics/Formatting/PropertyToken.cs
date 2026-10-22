// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Cake.Core.Diagnostics.Formatting;

internal sealed class PropertyToken : FormatToken
{
    public string Format { get; }

    public int Position { get; }

    public int? Alignment { get; }

    public PropertyToken(int position, string format, int? alignment = null)
    {
        Position = position;
        Format = format;
        Alignment = alignment;
    }

    public override string Render(object[] args)
    {
        if (Position < 0 || Position >= args.Length)
        {
            throw new FormatException("Index (zero based) must be greater than or equal to zero and less than the size of the argument list.");
        }

        var value = args[Position];
        var result = !string.IsNullOrWhiteSpace(Format) && value is IFormattable formattable
            ? formattable.ToString(Format, CultureInfo.InvariantCulture)
            : value == null ? "[NULL]" : value.ToString();

        return Alignment switch
        {
            > 0 => result.PadLeft(Alignment.Value),
            < 0 => result.PadRight(-Alignment.Value),
            _ => result
        };
    }
}
