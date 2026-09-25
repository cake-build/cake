// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.Text;

namespace Cake.Infrastructure.Utilities;

/// <summary>
/// Optimized hash generator. Using SHA512 since it is FIPS compliant.
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
        using (var sha512 = SHA512.Create())
        {
            sha512.TransformBlock(input, 0, input.Length, input, 0);

            // Just finalize with empty bytes so we don't have to iterate over the enumerable multiple times
            sha512.TransformFinalBlock(Encoding.UTF8.GetBytes(string.Empty), 0, 0);
            return Convert.ToHexString(sha512.Hash);
        }
    }

    /// <summary>
    /// Generates a hash of the passed byte arrays.
    /// </summary>
    /// <param name="inputs">The binary data to hash.</param>
    /// <returns>The hash value.</returns>
    public static string GenerateHash(IEnumerable<byte[]> inputs)
    {
        using (var sha512 = SHA512.Create())
        {
            foreach (var input in inputs)
            {
                sha512.TransformBlock(input, 0, input.Length, input, 0);
            }

            // Just finalize with empty bytes so we don't have to iterate over the enumerable multiple times
            sha512.TransformFinalBlock(Encoding.UTF8.GetBytes(string.Empty), 0, 0);
            return Convert.ToHexString(sha512.Hash);
        }
    }
}
