// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// Represents a log level.
    /// </summary>
    public enum LogLevel
    {
        /// <summary>
        /// Severe errors that cause premature termination.
        /// </summary>
        Fatal = 0,

        /// <summary>
        /// Other runtime errors or unexpected conditions.
        /// </summary>
        Error = 1,

        /// <summary>
        /// Use of deprecated APIs, poor use of API, 'almost' errors, other runtime
        /// situations that are undesirable or unexpected, but not necessarily "wrong".
        /// </summary>
        Warning = 2,

        /// <summary>
        /// Interesting runtime events.
        /// </summary>
        Information = 3,

        /// <summary>
        /// Detailed information on the flow through the system.
        /// </summary>
        Verbose = 4,

        /// <summary>
        /// Most detailed information.
        /// </summary>
        Debug = 5
    }
}
