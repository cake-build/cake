// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.ComponentModel;
using Cake.Common.Tools.DotNet;

namespace Cake.Common.Tools.DotNetCore
{
    /// <summary>
    /// Contains the roll forward policy to be used.
    /// </summary>
    [TypeConverter(typeof(DotNetCoreRollForwardConverter))]
    public sealed class DotNetCoreRollForward : IEquatable<DotNetCoreRollForward>, IComparable, IConvertible, IFormattable
    {
        private readonly DotNetRollForward _value;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreRollForward" /> class.
        /// </summary>
        public DotNetCoreRollForward()
        {
            _value = default;
        }

        internal DotNetCoreRollForward(string stringValue)
        {
            _value = (DotNetRollForward)Enum.Parse(typeof(DotNetRollForward), stringValue);
        }

        private DotNetCoreRollForward(DotNetRollForward value)
        {
            _value = value;
        }

        /// <summary>
        /// Roll forward to the lowest higher minor version, if requested minor version is missing.
        /// </summary>
        public static readonly DotNetCoreRollForward Minor = new DotNetCoreRollForward(DotNetRollForward.Minor);

        /// <summary>
        /// Roll forward to the highest patch version. This disables minor version roll forward.
        /// </summary>
        public static readonly DotNetCoreRollForward LatestPatch = new DotNetCoreRollForward(DotNetRollForward.LatestPatch);

        /// <summary>
        /// Roll forward to lowest higher major version, and lowest minor version, if requested major version is missing.
        /// </summary>
        public static readonly DotNetCoreRollForward Major = new DotNetCoreRollForward(DotNetRollForward.Major);

        /// <summary>
        /// Roll forward to highest minor version, even if requested minor version is present.
        /// </summary>
        public static readonly DotNetCoreRollForward LatestMinor = new DotNetCoreRollForward(DotNetRollForward.LatestMinor);

        /// <summary>
        /// Roll forward to highest major and highest minor version, even if requested major is present.
        /// </summary>
        public static readonly DotNetCoreRollForward LatestMajor = new DotNetCoreRollForward(DotNetRollForward.LatestMajor);

        /// <summary>
        /// Don't roll forward. Only bind to specified version.
        /// </summary>
        public static readonly DotNetCoreRollForward Disable = new DotNetCoreRollForward(DotNetRollForward.Disable);

        /// <summary>
        /// Explicitly converts <see cref="DotNetCoreRollForward"/> to <see cref="int"/>.
        /// </summary>
        /// <param name="rollForward">The <see cref="DotNetCoreRollForward"/>.</param>
        public static explicit operator int(DotNetCoreRollForward rollForward)
        {
            return (int)rollForward._value;
        }

        /// <summary>
        /// Implicitly converts <see cref="DotNetCoreRollForward"/> to <see cref="DotNetRollForward"/>.
        /// </summary>
        /// <param name="rollForward">The <see cref="DotNetCoreRollForward"/>.</param>
        public static implicit operator DotNetRollForward(DotNetCoreRollForward rollForward)
        {
            return rollForward._value;
        }

        /// <summary>
        /// Implicitly converts <see cref="DotNetRollForward"/> to <see cref="DotNetCoreRollForward"/>.
        /// </summary>
        /// <param name="rollForward">The <see cref="DotNetRollForward"/>.</param>
        public static implicit operator DotNetCoreRollForward(DotNetRollForward rollForward)
        {
            return rollForward switch
            {
                DotNetRollForward.Minor => Minor,
                DotNetRollForward.LatestPatch => LatestPatch,
                DotNetRollForward.Major => Major,
                DotNetRollForward.LatestMinor => LatestMinor,
                DotNetRollForward.LatestMajor => LatestMajor,
                DotNetRollForward.Disable => Disable,
                _ => new DotNetCoreRollForward(rollForward),
            };
        }

        /// <summary>
        /// Determines whether the specified object is equal to the current object.
        /// </summary>
        /// <param name="left">The object to compare with the right-hand side object.</param>
        /// <param name="right">The object to compare with the left-hand side object.</param>
        /// <returns><see langword="true"/> if the specified object is equal to the current object; otherwise, <see langword="false"/>.</returns>
        public static bool operator ==(DotNetCoreRollForward left, DotNetCoreRollForward right)
        {
            return left.Equals(right);
        }

        /// <summary>
        /// Determines whether the specified object is not equal to the current object.
        /// </summary>
        /// <param name="left">The object to compare with the right-hand side object.</param>
        /// <param name="right">The object to compare with the left-hand side object.</param>
        /// <returns><see langword="true"/> if the specified object is not equal to the current object; otherwise, <see langword="false"/>.</returns>
        public static bool operator !=(DotNetCoreRollForward left, DotNetCoreRollForward right)
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

            if (!(obj is DotNetCoreRollForward))
            {
                return false;
            }

            return Equals((DotNetCoreRollForward)obj);
        }

        /// <inheritdoc/>
        public bool Equals(DotNetCoreRollForward other)
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
        public bool HasFlag(DotNetCoreRollForward flag)
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
