// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.InspectCode
{
    /// <summary>
    /// Represents InspectCode's logging verbosity.
    /// </summary>
    public enum InspectCodeVerbosity
    {
        /// <summary>
        /// Verbosity: OFF.
        /// </summary>
        Off = 1,

        /// <summary>
        /// Verbosity: FATAL.
        /// </summary>
        Fatal = 2,

        /// <summary>
        /// Verbosity: ERROR.
        /// </summary>
        Error = 3,

        /// <summary>
        /// Verbosity: WARN.
        /// </summary>
        Warn = 4,

        /// <summary>
        /// Verbosity: INFO.
        /// </summary>
        Info = 5,

        /// <summary>
        /// Verbosity: VERBOSE.
        /// </summary>
        Verbose = 6,

        /// <summary>
        /// Verbosity: TRACE.
        /// </summary>
        Trace = 7
    }
}