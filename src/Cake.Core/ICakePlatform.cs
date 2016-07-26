// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core
{
    /// <summary>
    /// Represents the platform that Cake is running on.
    /// </summary>
    public interface ICakePlatform
    {
        /// <summary>
        /// Gets the platform family.
        /// </summary>
        /// <value>The platform family.</value>
        PlatformFamily Family { get; }

        /// <summary>
        /// Gets a value indicating whether or not the current operative system is 64 bit.
        /// </summary>
        /// <value>
        ///   <c>true</c> if current operative system is 64 bit; otherwise, <c>false</c>.
        /// </value>
        bool Is64Bit { get; }
    }
}