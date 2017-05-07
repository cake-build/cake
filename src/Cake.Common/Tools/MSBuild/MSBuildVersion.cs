// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Represents a MSBuild version
    /// </summary>
    public enum MSBuildVersion
    {
        /// <summary>Version 2.0</summary>
        MSBuild20 = 1,

        /// <summary>Version 3.5</summary>
        MSBuild35 = 2,

        /// <summary>Version 4.0</summary>
        MSBuild4 = 3,

        /// <summary>Version 12.0</summary>
        MSBuild12 = 4,

        /// <summary>Version 14.0</summary>
        MSBuild14 = 5,

        /// <summary>Version 15.0</summary>
        MSBuild15 = 6
    }
}