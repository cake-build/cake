// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Represents a MSBuild platform target.
    /// </summary>
    public enum PlatformTarget
    {
        /// <summary>
        /// Platform target: <c>MSIL</c> (Any CPU)
        /// </summary>
        MSIL = 0,

        /// <summary>
        /// Platform target: <c>x86</c>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x86 = 1,

        /// <summary>
        /// Platform target: <c>x64</c>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        x64 = 2,

        /// <summary>
        /// Platform target: <c>ARM</c>
        /// </summary>
        // ReSharper disable once InconsistentNaming
        ARM = 3,

        /// <summary>
        /// Platform target: <c>Win32</c>
        /// </summary>
        Win32 = 4
    }
}
