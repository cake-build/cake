// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// The type of file logger output to generate.
    /// </summary>
    public enum MSBuildFileLoggerOutput
    {
        /// <summary>
        /// Show errors and warnings.
        /// </summary>
        All = 0,

        /// <summary>
        /// Show errors only.
        /// </summary>
        ErrorsOnly = 1,

        /// <summary>
        /// Show warnings only.
        /// </summary>
        WarningsOnly = 2,
    }
}