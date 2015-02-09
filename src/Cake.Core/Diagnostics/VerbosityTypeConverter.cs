using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Globalization;

namespace Cake.Core.Diagnostics
{
    internal sealed class VerbosityTypeConverter : TypeConverter
    {
        private readonly Dictionary<string, Verbosity> _lookup;

        public VerbosityTypeConverter()
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

        public override bool CanConvertFrom(ITypeDescriptorContext context, Type sourceType)
        {
            return sourceType == typeof(string);
        }

        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            var stringValue = value as string;
            if (stringValue != null)
            {
                Verbosity verbosity;
                if (_lookup.TryGetValue(stringValue, out verbosity))
                {
                    return verbosity;
                }
            }
            return base.ConvertFrom(context, culture, value);
        }
    }
}