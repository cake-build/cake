// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="T:IEnumerable{String}"/>.
    /// </summary>
    public static class EnumerableStringExtensions
    {
        /// <summary>
        /// Performs a conversion from <see cref="IEnumerable{String}"/> to <see cref="ProcessArgumentBuilder"/>.
        /// </summary>
        /// <param name="values">The text values to convert.</param>
        /// <returns>A <see cref="ProcessArgumentBuilder"/>.</returns>
        public static ProcessArgumentBuilder ToProcessArguments(this IEnumerable<string> values)
        {
            return ProcessArgumentBuilder.FromStrings(values);
        }

        /// <summary>
        /// Performs a conversion from <see cref="IEnumerable{String}"/> to <see cref="ProcessArgumentBuilder"/>.
        /// </summary>
        /// <param name="values">The text values to convert.</param>
        /// <returns>A <see cref="ProcessArgumentBuilder"/>.</returns>
        public static ProcessArgumentBuilder ToProcessArgumentsQuoted(this IEnumerable<string> values)
        {
            return ProcessArgumentBuilder.FromStringsQuoted(values);
        }
    }
}