// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Cake.Utilities
{
    /// <summary>
    /// Optimized hash generator. Using SHA1CryptoServiceProvider since it is FIPS compliant.
    /// </summary>
    internal static class FastHash
    {
        /// <summary>
        /// Generates a hash of the passed byte arrays.
        /// </summary>
        /// <param name="input">The binary data to hash.</param>
        /// <returns>The hash value.</returns>
        public static string GenerateHash(byte[] input)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                sha1.TransformBlock(input, 0, input.Length, input, 0);

                // Just finalize with empty bytes so we don't have to iterate over the enumerable multiple times
                sha1.TransformFinalBlock(Encoding.UTF8.GetBytes(string.Empty), 0, 0);
                // Convert to hex string; This method is supposedly faster than the usual StringBuilder approach
                return ConvertBits(sha1.Hash);
            }
        }

        /// <summary>
        /// Generates a hash of the passed byte arrays.
        /// </summary>
        /// <param name="inputs">The binary data to hash.</param>
        /// <returns>The hash value.</returns>
        public static string GenerateHash(IEnumerable<byte[]> inputs)
        {
            using (var sha1 = new SHA1CryptoServiceProvider())
            {
                foreach (var input in inputs)
                {
                    sha1.TransformBlock(input, 0, input.Length, input, 0);
                }

                // Just finalize with empty bytes so we don't have to iterate over the enumerable multiple times
                sha1.TransformFinalBlock(Encoding.UTF8.GetBytes(string.Empty), 0, 0);
                // Convert to hex string; This method is supposedly faster than the usual StringBuilder approach
                return ConvertBits(sha1.Hash);
            }
        }

        private static string ConvertBits(byte[] hash)
        {
            return BitConverter.ToString(hash)
                    // without dashes
                    .Replace("-", string.Empty)
                    // make lowercase
                    .ToLower();
        }
    }
}
