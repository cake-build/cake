using System;
using System.ComponentModel;
using System.Globalization;

namespace Cake.Common.Tools.DotNetCore
{
    /// <summary>
    /// A type converter for <see cref="DotNetCoreVerbosity"/>.
    /// </summary>
    public class DotNetCoreVerbosityConverter : TypeConverter
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
                return new DotNetCoreVerbosity(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(DotNetCoreVerbosity) || base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is DotNetCoreVerbosity dotNetVerbosityValue)
            {
                return dotNetVerbosityValue.ToString();
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}
