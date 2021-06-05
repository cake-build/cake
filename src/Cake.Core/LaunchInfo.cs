// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

namespace Cake.Core
{
    /// <inheritdoc/>
    public sealed class LaunchInfo : ILaunchInfo
    {
        /// <inheritdoc/>
        public DirectoryPath LaunchDirectory { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="LaunchInfo"/> class.
        /// </summary>
        public LaunchInfo()
        {
            LaunchDirectory = Environment.CurrentDirectory;
        }
    }
}
