// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Testing
{
    /// <summary>
    /// An implementation of a fake <see cref="ILaunchInfo"/>.
    /// </summary>
    public sealed class FakeLaunchInfo : ILaunchInfo
    {
        /// <inheritdoc/>
        public DirectoryPath LaunchDirectory { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeLaunchInfo"/> class.
        /// </summary>
        public FakeLaunchInfo()
        {
            LaunchDirectory = "/";
        }
    }
}
