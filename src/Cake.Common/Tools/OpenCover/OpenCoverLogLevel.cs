// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.OpenCover
{
    /// <summary>
    /// Represents the logging output level for OpenCover.
    /// </summary>
    public enum OpenCoverLogLevel
    {
        /// <summary>
        /// Logs info messages and above (default)
        /// </summary>
        Info,

        /// <summary>
        /// This log level disables logging
        /// </summary>
        Off,

        /// <summary>
        /// Logs fatal messages
        /// </summary>
        Fatal,

        /// <summary>
        /// Logs error messages and above
        /// </summary>
        Error,

        /// <summary>
        /// Logs warn messages and above
        /// </summary>
        Warn,

        /// <summary>
        /// Logs debug messages and above
        /// </summary>
        Debug,

        /// <summary>
        /// Logs verbose messages and above
        /// </summary>
        Verbose,

        /// <summary>
        /// Logs all messages
        /// </summary>
        All
    }
}
