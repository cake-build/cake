// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Cake.Frosting.Internal.Diagnostics.Formatting
{
    internal sealed class PropertyToken : FormatToken
    {
        public string Format { get; }

        public int Position { get; }

        public PropertyToken(int position, string format)
        {
            Position = position;
            Format = format;
        }

        public override string Render(object[] args)
        {
            var value = args[Position];
            if (!string.IsNullOrWhiteSpace(Format))
            {
                var formattable = value as IFormattable;
                if (formattable != null)
                {
                    return formattable.ToString(Format, CultureInfo.InvariantCulture);
                }
            }
            return value == null ? "[NULL]" : value.ToString();
        }
    }
}