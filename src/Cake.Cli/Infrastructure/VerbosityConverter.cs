// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Cli
{
    /// <summary>
    /// A type converter for <see cref="Verbosity"/>.
    /// </summary>
    public sealed class VerbosityConverter : TypeConverter
    {
        private readonly Dictionary<string, Verbosity> _lookup;

        /// <summary>
        /// Initializes a new instance of the <see cref="VerbosityConverter"/> class.
        /// </summary>
        public VerbosityConverter()
        {
            _lookup = new Dictionary<string, Verbosity>(StringComparer.OrdinalIgnoreCase)
            {
                { "q", Verbosity.Quiet },
                { "quiet", Verbosity.Quiet },
                { "m", Verbosity.Minimal },
                { "minimal", Verbosity.Minimal },
                { "n", Verbosity.Normal },
                { "normal", Verbosity.Normal },
                { "v", Verbosity.Verbose },
                { "verbose", Verbosity.Verbose },
                { "d", Verbosity.Diagnostic },
                { "diagnostic", Verbosity.Diagnostic }
            };
        }

        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                var result = _lookup.TryGetValue(stringValue, out var verbosity);
                if (!result)
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
}
