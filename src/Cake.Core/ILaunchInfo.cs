// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Core
{
    /// <summary>
    /// Represents a launch info.
    /// </summary>
    public interface ILaunchInfo
    {
        /// <summary>
        /// Gets the working directory where the runner was invoked from.
        /// </summary>
        /// <value>The launch directory.</value>
        DirectoryPath LaunchDirectory { get; }
    }
}
