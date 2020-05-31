// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Testing
{
    /// <summary>
    /// An implementation of a fake <see cref="ICakePlatform"/>.
    /// </summary>
    public sealed class FakePlatform : ICakePlatform
    {
        /// <summary>
        /// Gets or sets the platform family.
        /// </summary>
        /// <value>The platform family.</value>
        public PlatformFamily Family { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not the current operative system is 64 bit.
        /// </summary>
        /// <value>
        /// <c>true</c> if current operative system is 64 bit; otherwise, <c>false</c>.
        /// </value>
        public bool Is64Bit { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakePlatform"/> class.
        /// </summary>
        /// <param name="family">The family.</param>
        /// <param name="is64Bit">if set to <c>true</c>, the platform is 64-bit.</param>
        public FakePlatform(PlatformFamily family, bool is64Bit = true)
        {
            Family = family;
            Is64Bit = is64Bit;
        }
    }
}