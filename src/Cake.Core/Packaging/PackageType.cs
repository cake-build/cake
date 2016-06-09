// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Packaging
{
    /// <summary>
    /// Represents a package type.
    /// </summary>
    public enum PackageType
    {
        /// <summary>
        /// Represents an unspecified package type.
        /// </summary>
        Unspecified = 0,

        /// <summary>
        /// Represents an addin.
        /// </summary>
        Addin = 1,

        /// <summary>
        /// Represents a tool.
        /// </summary>
        Tool = 2
    }
}
