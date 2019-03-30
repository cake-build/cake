// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitLink
{
    /// <summary>
    /// Contains settings used by <see cref="GitLink3Runner"/> .
    /// </summary>
    public sealed class GitLink3Settings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the Url to remote Git repository.
        /// </summary>
        public string RepositoryUrl { get; set; }

        /// <summary>
        /// Gets or sets the SHA-1 hash of the Git commit to be used.
        /// </summary>
        public string ShaHash { get; set; }

        /// <summary>
        /// Gets or sets the path to the root of the Git repository
        /// </summary>
        public DirectoryPath BaseDir { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Use PowerShell Command option should be enabled.
        /// This option will use PowerShell instead of HTTP in SRCSRV to retrieve the source code.
        /// </summary>
        public bool UsePowerShell { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Skip Verify option should be enabled.
        /// This option indicates whether verification of all source files are available in source control.
        /// </summary>
        public bool SkipVerify { get; set; }
    }
}