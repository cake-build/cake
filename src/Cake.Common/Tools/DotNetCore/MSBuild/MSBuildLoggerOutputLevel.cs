// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// Represents the logging output level for a MSBuild logger.
    /// </summary>
    public enum MSBuildLoggerOutputLevel
    {
        /// <summary>
        /// Show the error and warning summary at the end.
        /// </summary>
        Default = 0,

        /// <summary>
        /// Show only warnings summary at the end.
        /// </summary>
        WarningsOnly,

        /// <summary>
        /// Show only errors summary at the end.
        /// </summary>
        ErrorsOnly
    }
}