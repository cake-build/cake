// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Globalization;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Cli;

/// <summary>
/// A type converter for <see cref="Verbosity"/>.
/// </summary>
public sealed class VerbosityConverter : TypeConverter
{
    /// <inheritdoc/>
    public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
    {
        return sourceType == typeof(string) || base.CanConvertFrom(context, sourceType);
    }

    /// <inheritdoc/>
    public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
    {
        if (value is string stringValue)
        {
            if (!VerbosityParser.TryParse(stringValue, out var verbosity))
            {
                const string format = "The value '{0}' is not a valid verbosity.";
                var message = string.Format(CultureInfo.InvariantCulture, format, value);
                throw new CakeException(message);
            }
            return verbosity;
        }
        throw new NotSupportedException("Can't convert value to verbosity.");
    }
}
