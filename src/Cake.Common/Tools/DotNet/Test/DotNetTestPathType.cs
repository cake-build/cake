// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Test
{
    /// <summary>
    /// Represents the path type for .NET test command.
    /// </summary>
    public enum DotNetTestPathType
    {
        /// <summary>
        /// No explicit path type specified (default behavior).
        /// </summary>
        None,

        /// <summary>
        /// Automatically detect the path type based on the file extension.
        /// </summary>
        Auto,

        /// <summary>
        /// Explicitly specify the path as a project file.
        /// </summary>
        Project,

        /// <summary>
        /// Explicitly specify the path as a solution file.
        /// </summary>
        Solution
    }
}
