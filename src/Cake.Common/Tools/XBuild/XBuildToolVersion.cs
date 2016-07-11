// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.XBuild
{
    /// <summary>
    /// Represents a XBuild tool version.
    /// </summary>
    public enum XBuildToolVersion
    {
        /// <summary>
        /// The highest available XBuild tool version.
        /// </summary>
        Default = 0,

        /// <summary>
        /// XBuild tool version: <c>.NET 2.0</c>
        /// </summary>
        NET20 = 1,

        /// <summary>
        /// XBuild tool version: <c>.NET 3.0</c>
        /// </summary>
        NET30 = 1,

        /// <summary>
        /// XBuild tool version: <c>.NET 3.5</c>
        /// </summary>
        NET35 = 2,

        /// <summary>
        /// XBuild tool version: <c>.NET 4.0</c>
        /// </summary>
        NET40 = 3,
    }
}
