// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.Polyfill;

namespace Cake.Core
{
    /// <inheritdoc/>
    public sealed class CakePlatform : ICakePlatform
    {
        /// <inheritdoc/>
        public PlatformFamily Family { get; }

        /// <inheritdoc/>
        public bool Is64Bit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakePlatform"/> class.
        /// </summary>
        public CakePlatform()
        {
            Family = EnvironmentHelper.GetPlatformFamily();
            Is64Bit = EnvironmentHelper.Is64BitOperativeSystem();
        }
    }
}