using System;
using System.ComponentModel;
using System.Globalization;

namespace Cake.Core.IO
{
    /// <summary>
    /// A type converter for <see cref="FilePath"/>.
    /// </summary>
    public sealed class FilePathConverter : TypeConverter
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
                return new FilePath(stringValue);
            }

            return base.ConvertFrom(context, culture, value);
        }

        /// <inheritdoc/>
        public override bool CanConvertTo(ITypeDescriptorContext context, Type destinationType)
        {
            return destinationType == typeof(FilePath) || base.CanConvertTo(context, destinationType);
        }

        /// <inheritdoc/>
        public override object ConvertTo(ITypeDescriptorContext context, CultureInfo culture, object value, Type destinationType)
        {
            if (destinationType == typeof(string) && value is FilePath filePathValue)
            {
                return filePathValue.FullPath;
            }

            return base.ConvertTo(context, culture, value, destinationType);
        }
    }
}