// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitLink
{
    /// <summary>
    /// Contains settings used by <see cref="GitLinkRunner"/> .
    /// </summary>
    public sealed class GitLinkSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the Url to remote git repository.
        /// </summary>
        public string RepositoryUrl { get; set; }

        /// <summary>
        /// Gets or sets the Solution file name.
        /// </summary>
        public string SolutionFileName { get; set; }

        /// <summary>
        /// Gets or sets the name of the configuration.
        /// </summary>
        /// <remarks>Default is Release</remarks>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets the name of the platform.
        /// </summary>
        /// <remarks>Default is AnyCPU</remarks>
        public string Platform { get; set; }

        /// <summary>
        /// Gets or sets the name of the branch to use on the remote repository.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the path to the GitLink log file.
        /// </summary>
        public FilePath LogFilePath { get; set; }

        /// <summary>
        /// Gets or sets the SHA-1 hash of the git commit to be used.
        /// </summary>
        public string ShaHash { get; set; }

        /// <summary>
        /// Gets or sets the directory where the PDB files are located.
        /// </summary>
        public DirectoryPath PdbDirectoryPath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Use PowerShell Command option should be enabled.
        /// </summary>
        public bool UsePowerShell { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the ErrorsAsWarnings option should be enabled.
        /// </summary>
        public bool ErrorsAsWarnings { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the Skip Verify option should be enabled.
        /// </summary>
        public bool SkipVerify { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the debug output should be enabled.
        /// </summary>
        public bool IsDebug { get; set; }
    }
}
