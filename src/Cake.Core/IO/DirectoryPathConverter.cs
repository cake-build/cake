using System;
using System.ComponentModel;
using System.Globalization;

namespace Cake.Core.IO
{
    /// <summary>
    /// A type converter for <see cref="DirectoryPath"/>.
    /// </summary>
    public sealed class DirectoryPathConverter : TypeConverter
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
                return new DirectoryPath(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(DirectoryPath) || base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is DirectoryPath directoryPathValue)
            {
                return directoryPathValue.FullPath;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}