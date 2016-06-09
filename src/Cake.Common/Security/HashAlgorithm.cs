// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Security
{
    /// <summary>
    /// The hash algorithm to use for a specific operation.
    /// </summary>
    public enum HashAlgorithm
    {
        /// <summary>
        /// The MD5 hash algorithm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        MD5,

        /// <summary>
        /// The SHA256 hash algorithm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SHA256,

        /// <summary>
        /// The SHA512 hash algorithm.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        SHA512
    }
}
