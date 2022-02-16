// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using System.Globalization;
using Cake.Core.IO;

namespace Cake.Cli
{
    /// <summary>
    /// A type converter for <see cref="FilePath"/>.
    /// </summary>
    public sealed class FilePathConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return new FilePath(stringValue);
            }

            throw new NotSupportedException("Can't convert value to file path.");
        }
    }

    /// <summary>
    /// A type converter for <see cref="DirectoryPath"/>.
    /// </summary>
    public sealed class DirectoryPathConverter : TypeConverter
    {
        /// <inheritdoc/>
        public override object ConvertFrom(ITypeDescriptorContext context, CultureInfo culture, object value)
        {
            if (value is string stringValue)
            {
                return new DirectoryPath(stringValue);
            }

            throw new NotSupportedException("Can't convert value to file path.");
        }
    }
}
