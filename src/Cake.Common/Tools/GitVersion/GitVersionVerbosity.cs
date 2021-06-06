// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// The GitVersion verbosity.
    /// </summary>
    public enum GitVersionVerbosity
    {
        /// <summary>
        /// No messages will be logged.
        /// </summary>
        Quiet,

        /// <summary>
        /// Log error messages.
        /// </summary>
        Minimal,

        /// <summary>
        /// Log error and warning messages.
        /// </summary>
        Normal,

        /// <summary>
        /// Log error, warning and info messages.
        /// </summary>
        Verbose,

        /// <summary>
        /// Log error, warning, info and debug messages (log all).
        /// </summary>
        Diagnostic
    }
}
