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
        None,

        /// <summary>
        /// Only log error messages.
        /// </summary>
        Error,

        /// <summary>
        /// Only log wanring messages.
        /// </summary>
        Warn,

        /// <summary>
        /// Only log info messages.
        /// </summary>
        Info,

        /// <summary>
        /// Only log debug messages.
        /// </summary>
        Debug
    }
}