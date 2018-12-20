// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="T:byte[]"/>.
    /// </summary>
    public static class ByteArrayExtensions
    {
        /// <summary>
        /// Determines if a <see cref="T:byte[]"/> starts with a specified prefix.
        /// </summary>
        /// <param name="value">The value.</param>
        /// <param name="prefix">The prefix to compare.</param>
        /// <returns>Whether or not the <see cref="T:byte[]"/> starts with the specified prefix.</returns>
        public static bool StartsWith(this byte[] value, byte[] prefix)
        {
            if (value == null)
            {
                throw new ArgumentNullException(nameof(value));
            }
            if (prefix == null)
            {
                throw new ArgumentNullException(nameof(prefix));
            }

            if (value.Length < prefix.Length)
            {
                return false;
            }

            for (int i = 0; i < prefix.Length; i++)
            {
                if (value[i] != prefix[i])
                {
                    return false;
                }
            }
            return true;
        }
    }
}
