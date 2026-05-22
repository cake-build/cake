// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Rwx.Commands;
using Cake.Common.Build.Rwx.Data;

namespace Cake.Common.Build.Rwx
{
    /// <summary>
    /// Represents an RWX Provider.
    /// </summary>
    public interface IRwxProvider
    {
        /// <summary>
        /// Gets a value indicating whether this instance is running on RWX.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running on RWX; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnRwx { get; }

        /// <summary>
        /// Gets the environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        RwxEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the RWX commands surface, used to write output values and upload
        /// artifacts at task runtime.
        /// </summary>
        /// <value>
        /// The commands.
        /// </value>
        RwxCommands Commands { get; }
    }
}
