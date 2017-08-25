// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// Represents the settings for a file logger.
    /// </summary>
    public class MSBuildFileLoggerSettings : MSBuildLoggerSettings
    {
        /// <summary>
        /// Gets or sets the path to the log file into which the build log is written.
        /// </summary>
        /// <remarks>
        /// An empty string will use msbuild.log, in the current directory.
        /// </remarks>
        public string LogFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the build log is appended to the log file or overwrites it.
        /// </summary>
        public bool AppendToLogFile { get; set; }

        /// <summary>
        /// Gets or sets the encoding for the file (for example, UTF-8, Unicode, or ASCII).
        /// </summary>
        public string FileEncoding { get; set; }
    }
}