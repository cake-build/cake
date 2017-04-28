// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;

namespace Cake.Core
{
    /// <summary>
    /// Extensions for validating arguments.
    /// </summary>
    public static class CakeArgumentChecks
    {
        /// <summary>
        /// Throws an exception if the specified parameter's value is null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <returns>The value of the argument.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        [DebuggerStepThrough]
        public static T NotNull<T>([ValidatedNotNull]this T value, string parameterName)
            where T : class
        {
            parameterName.NotNull(nameof(parameterName));

            if (value == null)
            {
                throw new ArgumentNullException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Throws an exception if the value of a property from a parameter is null.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the property.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <param name="propertyName">The name of the property to include in any thrown exception.</param>
        /// <returns>The value of the argument.</returns>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        [DebuggerStepThrough]
        public static T NotNull<T>([ValidatedNotNull]this T value, string parameterName, string propertyName)
            where T : class
        {
            parameterName.NotNull(nameof(parameterName));
            propertyName.NotNull(nameof(propertyName));

            if (value == null)
            {
                throw new ArgumentException(
                    string.Format("{0}.{1}", parameterName, propertyName));
            }

            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null or empty.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <returns>The value of the argument.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is empty.</exception>
        [DebuggerStepThrough]
        public static string NotNullOrEmpty([ValidatedNotNull]this string value, string parameterName)
        {
            parameterName.NotNull(nameof(parameterName));

            value.NotNull(parameterName);

            if (string.IsNullOrEmpty(value))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null, empty or consists only of white-space characters.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <returns>The value of the argument.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c>.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is empty or consists only of white-space characters.</exception>
        [DebuggerStepThrough]
        public static string NotNullOrWhiteSpace([ValidatedNotNull]this string value, string parameterName)
        {
            parameterName.NotNull(nameof(parameterName));

            value.NotNull(parameterName);

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is negative.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <returns>The value of the argument.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is negative.</exception>
        [DebuggerStepThrough]
        public static int NotNegative(this int value, string parameterName)
        {
            parameterName.NotNull(nameof(parameterName));

            if (value < 0)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is negative or zero.
        /// </summary>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <returns>The value of the argument.</returns>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> is negative or zero.</exception>
        [DebuggerStepThrough]
        public static int NotNegativeOrZero(this int value, string parameterName)
        {
            parameterName.NotNull(nameof(parameterName));

            if (value <= 0)
            {
                throw new ArgumentOutOfRangeException(parameterName);
            }

            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null or empty.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <returns>The value of the argument.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        [DebuggerStepThrough]
        public static IEnumerable<T> NotNullOrEmpty<T>([ValidatedNotNull] this IEnumerable<T> value, string parameterName)
        {
            parameterName.NotNull(nameof(parameterName));

            // ReSharper disable once PossibleMultipleEnumeration
            value.NotNull(parameterName);

            // ReSharper disable once PossibleMultipleEnumeration
            if (!value.Any())
            {
                throw new ArgumentException("Empty enumeration.", parameterName);
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return value;
        }

        /// <summary>
        /// Throws an exception if the specified parameter's value is null, empty or contains an empty element.
        /// </summary>
        /// <typeparam name="T">The type of the parameter.</typeparam>
        /// <param name="value">The value of the argument.</param>
        /// <param name="parameterName">The name of the parameter to include in any thrown exception.</param>
        /// <returns>The value of the argument.</returns>
        /// <exception cref="ArgumentNullException">Thrown if <paramref name="value"/> is <c>null</c></exception>
        /// <exception cref="ArgumentException">Thrown if <paramref name="value"/> is empty.</exception>
        /// <exception cref="ArgumentOutOfRangeException">Thrown if <paramref name="value"/> contains an empty element.</exception>
        [DebuggerStepThrough]
        public static IEnumerable<T> ContainsNoNulls<T>([ValidatedNotNull] this IEnumerable<T> value, string parameterName)
        {
            parameterName.NotNull(nameof(parameterName));

            // ReSharper disable once PossibleMultipleEnumeration
            value.NotNullOrEmpty(parameterName);

            // ReSharper disable once PossibleMultipleEnumeration
            if (value.Any(x => x == null))
            {
                throw new ArgumentOutOfRangeException(parameterName, "Enumeration contains null value.");
            }

            // ReSharper disable once PossibleMultipleEnumeration
            return value;
        }
    }
}
