// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.DotNetTool.Module
{
    /// <summary>
    /// Represents dotnet tool operation.
    /// </summary>
    public enum DotNetToolOperation
    {
        /// <summary>
        /// Install operation.
        /// </summary>
        Install,

        /// <summary>
        /// Uninstall operation.
        /// </summary>
        Uninstall,

        /// <summary>
        /// Update operation.
        /// </summary>
        Update
    }
}