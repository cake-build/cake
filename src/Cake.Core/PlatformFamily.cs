// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core
{
    /// <summary>
    /// Represents a platform family.
    /// </summary>
    public enum PlatformFamily
    {
        /// <summary>
        /// The platform family is unknown.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Represents the Windows platform family.
        /// </summary>
        Windows = 1,

        /// <summary>
        /// Represents the Linux platform family.
        /// </summary>
        Linux = 2,

        /// <summary>
        /// Represents the OSX platform family.
        /// </summary>
        // ReSharper disable once InconsistentNaming
        OSX = 3
    }
}