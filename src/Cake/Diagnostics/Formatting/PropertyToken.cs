// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;

namespace Cake.Diagnostics.Formatting
{
    internal sealed class PropertyToken : FormatToken
    {
        private readonly int _position;
        private readonly string _format;

        public string Format
        {
            get { return _format; }
        }

        public int Position
        {
            get { return _position; }
        }

        public PropertyToken(int position, string format)
        {
            _position = position;
            _format = format;
        }

        public override string Render(object[] args)
        {
            var value = args[_position];
            if (!string.IsNullOrWhiteSpace(_format))
            {
                var formattable = value as IFormattable;
                if (formattable != null)
                {
                    return formattable.ToString(_format, CultureInfo.InvariantCulture);
                }
            }
            return value == null ? "[NULL]" : value.ToString();
        }
    }
}
