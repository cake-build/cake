// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// Contains settings used by <see cref="GitVersionRunner" />.
    /// </summary>
    public sealed class GitVersionSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the path for the Git repository to use.
        /// </summary>
        public DirectoryPath RepositoryPath { get; set; }

        /// <summary>
        /// Gets or sets the output type.
        /// </summary>
        public GitVersionOutput? OutputType { get; set; }

        /// <summary>
        /// Gets or sets the path to a file to store the asserted GitVersion numbers in JSON format.
        /// </summary>
        public FilePath OutputFile { get; set; }

        /// <summary>
        /// Gets or sets the path to config file.
        /// </summary>
        /// <remarks>Defaults to GitVersion.yml.</remarks>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to update all the AssemblyInfo files.
        /// </summary>
        public bool UpdateAssemblyInfo { get; set; }

        /// <summary>
        /// Gets or sets whether to update all the AssemblyInfo files.
        /// </summary>
        public FilePath UpdateAssemblyInfoFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to recursively search for all project files (.csproj/.vbproj/.fsproj) files in the git repo and update them.
        /// </summary>
        /// <remarks>This is only compatible with the newer Sdk projects.</remarks>
        public bool UpdateProjectFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether if the assembly info file specified with /updateassemblyinfo or /updateassemblyinfofilename is not found, it be created with these attributes: AssemblyFileVersion, AssemblyVersion and AssemblyInformationalVersion.
        /// </summary>
        /// <remarks>Supports writing version info for: C#, F#, VB.</remarks>
        public bool EnsureAssemblyInfo { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all the GitVersion variables are written to 'GitVersion_WixVersion.wxi'. The variables can then be referenced in other WiX project files for versioning.
        /// </summary>
        public bool UpdateWixVersionFile { get; set; }

        /// <summary>
        /// Gets or sets whether to only show a specific variable.
        /// </summary>
        public string ShowVariable { get; set; }

        /// <summary>
        /// Gets or sets the username for the target repository.
        /// </summary>
        public string UserName { get; set; }

        /// <summary>
        /// Gets or sets the password for the target repository.
        /// </summary>
        public string Password { get; set; }

        /// <summary>
        /// Gets or sets the Git URL to use if using dynamic repositories.
        /// </summary>
        public string Url { get; set; }

        /// <summary>
        /// Gets or sets the branch to use if using dynamic repositories.
        /// </summary>
        public string Branch { get; set; }

        /// <summary>
        /// Gets or sets the branch to use if using dynamic repositories.
        /// </summary>
        public string Commit { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to fetch repository information from remote when calculating version.
        /// </summary>
        /// <remarks>If your CI server clones the entire repository you can set this to 'true' to prevent GitVersion attempting any remote repository fetching.</remarks>
        public bool NoFetch { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to bypass the cached GitVersion result. Result will not be written to the cache.
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable normalize step on a build server.
        /// </summary>
        public bool NoNormalize { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to run GitVersion with additional diagnostic information.
        /// </summary>
        /// <remarks>Requires git.exe to be installed.</remarks>
        public bool Diag { get; set; }

        /// <summary>
        /// Gets or sets the dynamic repository path. Defaults to %TEMP%.
        /// </summary>
        public DirectoryPath DynamicRepositoryPath { get; set; }

        /// <summary>
        /// Gets or sets the path to the log file.
        /// </summary>
        public FilePath LogFilePath { get; set; }

        /// <summary>
        /// Gets or sets the logging verbosity.
        /// </summary>
        public GitVersionVerbosity? Verbosity { get; set; }
    }
}