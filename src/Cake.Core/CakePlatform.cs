// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Core
{
    /// <summary>
    /// Represents the platform that Cake is running on.
    /// </summary>
    public sealed class CakePlatform : ICakePlatform
    {
        private readonly PlatformFamily _family;
        private readonly bool _is64Bit;

        /// <summary>
        /// Gets the platform family.
        /// </summary>
        /// <value>The platform family.</value>
        public PlatformFamily Family
        {
            get { return _family; }
        }

        /// <summary>
        /// Gets a value indicating whether or not the current platform is 64 bit.
        /// </summary>
        /// <value>
        /// <c>true</c> if current platform is 64 bit; otherwise, <c>false</c>.
        /// </value>
        public bool Is64Bit
        {
            get { return _is64Bit; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakePlatform"/> class.
        /// </summary>
        public CakePlatform()
        {
            _family = Machine.GetPlatformFamily();
            _is64Bit = Machine.Is64BitOperativeSystem();
        }
    }
}