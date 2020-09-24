// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// MSBuild binary logger settings used by <see cref="DotNetCoreMSBuildSettings"/>.
    /// </summary>
    public class MSBuildBinaryLoggerSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether binary logging should be enabled.
        /// </summary>
        public bool Enabled { get; set; }

        /// <summary>
        /// Gets or sets the output filename.
        /// </summary>
        public string FileName { get; set; }

        /// <summary>
        /// Gets or sets what source files should be included in the log.
        /// </summary>
        public MSBuildBinaryLoggerImports Imports { get; set; }
    }
}
