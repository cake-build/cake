// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNetCore
{
    /// <summary>
    /// Contains the verbosity of logging to use..
    /// </summary>
    public enum DotNetCoreVerbosity
    {
        /// <summary>
        /// Quiet level.
        /// </summary>
        Quiet,

        /// <summary>
        /// Minimal level.
        /// </summary>
        Minimal,

        /// <summary>
        /// Normal level.
        /// </summary>
        Normal,

        /// <summary>
        /// Detailed level.
        /// </summary>
        Detailed,

        /// <summary>
        /// Diagnostic level.
        /// </summary>
        Diagnostic
    }
}
