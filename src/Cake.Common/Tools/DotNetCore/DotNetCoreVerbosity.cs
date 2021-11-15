// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Cake.Common.Tools.DotNet;

namespace Cake.Common.Tools.DotNetCore
{
    /// <summary>
    /// Contains the verbosity of logging to use.
    /// </summary>
    [TypeConverter(typeof(DotNetCoreVerbosityConverter))]
    public sealed class DotNetCoreVerbosity : IEquatable<DotNetCoreVerbosity>, IComparable, IConvertible, IFormattable
    {
        private readonly DotNetVerbosity _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreVerbosity" /> class.
        /// </summary>
        public DotNetCoreVerbosity()
        {
            _value = default;
        }

        internal DotNetCoreVerbosity(string stringValue)
        {
            _value = (DotNetVerbosity)Enum.Parse(typeof(DotNetVerbosity), stringValue);
        }

        private DotNetCoreVerbosity(DotNetVerbosity value)
        {
            _value = value;
        }

        /// <summary>
        /// Quiet level.
        /// </summary>
        public static readonly DotNetCoreVerbosity Quiet = new DotNetCoreVerbosity(DotNetVerbosity.Quiet);

        /// <summary>
        /// Minimal level.
        /// </summary>
        public static readonly DotNetCoreVerbosity Minimal = new DotNetCoreVerbosity(DotNetVerbosity.Minimal);

        /// <summary>
        /// Normal level.
        /// </summary>
        public static readonly DotNetCoreVerbosity Normal = new DotNetCoreVerbosity(DotNetVerbosity.Normal);

        /// <summary>
        /// Detailed level.
        /// </summary>
        public static readonly DotNetCoreVerbosity Detailed = new DotNetCoreVerbosity(DotNetVerbosity.Detailed);

        /// <summary>
        /// Diagnostic level.
        /// </summary>
        public static readonly DotNetCoreVerbosity Diagnostic = new DotNetCoreVerbosity(DotNetVerbosity.Diagnostic);

        /// <summary>
        /// Explicitly converts <see cref="DotNetCoreVerbosity"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="verbosity">The <see cref="DotNetCoreVerbosity"/>.</param>
        public static explicit operator int(DotNetCoreVerbosity verbosity)
        {
            return (int)verbosity._value;
        }

        /// <summary>
        /// Implicitly converts <see cref="DotNetCoreVerbosity"/> to <see cref="DotNetVerbosity"/>.
        /// </summary>
        /// <param name="verbosity">The <see cref="DotNetCoreVerbosity"/>.</param>
        public static implicit operator DotNetVerbosity(DotNetCoreVerbosity verbosity)
        {
            return verbosity._value;
        }

        /// <summary>
        /// Implicitly converts <see cref="DotNetVerbosity"/> to <see cref="DotNetCoreVerbosity"/>.
        /// </summary>
        /// <param name="verbosity">The <see cref="DotNetVerbosity"/>.</param>
        public static implicit operator DotNetCoreVerbosity(DotNetVerbosity verbosity)
        {
            return verbosity switch
            {
                DotNetVerbosity.Quiet => Quiet,
                DotNetVerbosity.Minimal => Minimal,
                DotNetVerbosity.Normal => Normal,
                DotNetVerbosity.Detailed => Detailed,
                DotNetVerbosity.Diagnostic => Diagnostic,
                _ => new DotNetCoreVerbosity(verbosity),
            };
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="left">The object to compare with the right-hand side object.</param>
        /// <param name="right">The object to compare with the left-hand side object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(DotNetCoreVerbosity left, DotNetCoreVerbosity right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether the specified object is not equal to the current object.
        /// </summary>
        /// <param name="left">The object to compare with the right-hand side object.</param>
        /// <param name="right">The object to compare with the left-hand side object.</param>
        /// <returns><see langword="true"/> if the specified object is not equal to the current object; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(DotNetCoreVerbosity left, DotNetCoreVerbosity right)
        {
            return !(left == right);
        }

        /// <inheritdoc/>
        public override string ToString()
        {
            return _value.ToString();
        }

        /// <inheritdoc/>
        public override int GetHashCode()
        {
            return _value.GetHashCode();
        }

        /// <inheritdoc/>
        public override bool Equals(object obj)
        {
            if (obj is null)
            {
                return false;
            }

            if (!(obj is DotNetCoreVerbosity))
            {
                return false;
            }

            return Equals((DotNetCoreVerbosity)obj);
        }

        /// <inheritdoc/>
        public bool Equals(DotNetCoreVerbosity other)
        {
            return _value.Equals(other._value);
        }

        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// </summary>
        /// <param name="flag">An enumeration value.</param>
        /// <returns>
        /// <see langword="true"/> if the bit field or bit fields that are set in flag are also set in the
        /// current instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool HasFlag(DotNetCoreVerbosity flag)
        {
            return _value.HasFlag(flag._value);
        }

        /// <summary>
        /// Determines whether one or more bit fields are set in the current instance.
        /// </summary>
        /// <param name="flag">An enumeration value.</param>
        /// <returns>
        /// <see langword="true"/> if the bit field or bit fields that are set in flag are also set in the
        /// current instance; otherwise, <see langword="false"/>.
        /// </returns>
        public bool HasFlag(Enum flag)
        {
            return _value.HasFlag(flag);
        }

        /// <inheritdoc/>
        public int CompareTo(object obj)
        {
            return _value.CompareTo(obj);
        }

        /// <inheritdoc/>
        public TypeCode GetTypeCode()
        {
            return Convert.GetTypeCode(_value);
        }

        /// <inheritdoc/>
        bool IConvertible.ToBoolean(IFormatProvider provider)
        {
            return Convert.ToBoolean(_value, provider);
        }

        /// <inheritdoc/>
        byte IConvertible.ToByte(IFormatProvider provider)
        {
            return Convert.ToByte(_value, provider);
        }

        /// <inheritdoc/>
        char IConvertible.ToChar(IFormatProvider provider)
        {
            return Convert.ToChar(_value, provider);
        }

        /// <inheritdoc/>
        DateTime IConvertible.ToDateTime(IFormatProvider provider)
        {
            return Convert.ToDateTime(_value, provider);
        }

        /// <inheritdoc/>
        decimal IConvertible.ToDecimal(IFormatProvider provider)
        {
            return Convert.ToDecimal(_value, provider);
        }

        /// <inheritdoc/>
        double IConvertible.ToDouble(IFormatProvider provider)
        {
            return Convert.ToDouble(_value, provider);
        }

        /// <inheritdoc/>
        short IConvertible.ToInt16(IFormatProvider provider)
        {
            return Convert.ToInt16(_value, provider);
        }

        /// <inheritdoc/>
        int IConvertible.ToInt32(IFormatProvider provider)
        {
            return Convert.ToInt32(_value, provider);
        }

        /// <inheritdoc/>
        long IConvertible.ToInt64(IFormatProvider provider)
        {
            return Convert.ToInt64(_value, provider);
        }

        /// <inheritdoc/>
#pragma warning disable CS3002 // Return type is not CLS-compliant
        sbyte IConvertible.ToSByte(IFormatProvider provider)
#pragma warning restore CS3002 // Return type is not CLS-compliant
        {
            return Convert.ToSByte(_value, provider);
        }

        /// <inheritdoc/>
        float IConvertible.ToSingle(IFormatProvider provider)
        {
            return Convert.ToSingle(_value, provider);
        }

        /// <inheritdoc/>
        string IConvertible.ToString(IFormatProvider provider)
        {
            return Convert.ToString(_value, provider);
        }

        /// <inheritdoc/>
        object IConvertible.ToType(Type conversionType, IFormatProvider provider)
        {
            return Convert.ChangeType(_value, conversionType, provider);
        }

        /// <inheritdoc/>
#pragma warning disable CS3002 // Return type is not CLS-compliant
        ushort IConvertible.ToUInt16(IFormatProvider provider)
#pragma warning restore CS3002 // Return type is not CLS-compliant
        {
            return Convert.ToUInt16(_value, provider);
        }

        /// <inheritdoc/>
#pragma warning disable CS3002 // Return type is not CLS-compliant
        uint IConvertible.ToUInt32(IFormatProvider provider)
#pragma warning restore CS3002 // Return type is not CLS-compliant
        {
            return Convert.ToUInt32(_value, provider);
        }

        /// <inheritdoc/>
#pragma warning disable CS3002 // Return type is not CLS-compliant
        ulong IConvertible.ToUInt64(IFormatProvider provider)
#pragma warning restore CS3002 // Return type is not CLS-compliant
        {
            return Convert.ToUInt64(_value, provider);
        }

        /// <inheritdoc/>
        [Obsolete("The provider argument is not used. Please use ToString(String).")]
        public string ToString(string format, IFormatProvider formatProvider)
        {
            return _value.ToString(format, formatProvider);
        }
    }
}
