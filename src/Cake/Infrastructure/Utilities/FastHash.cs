// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Security.Cryptography;

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
        return Convert.ToHexString(SHA512.HashData(input));
    }
}
