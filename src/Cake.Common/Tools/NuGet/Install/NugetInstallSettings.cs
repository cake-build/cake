using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.Install
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetInstaller"/>.
    /// </summary>
    public sealed class NuGetInstallSettings
    {
        /// <summary>
        /// Path to nuget.exe
        /// </summary>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Specifies the directory in which packages will be installed. If none specified, uses the current directory.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        /// The version of the package to install. If none specified, uses the latest.
        /// </summary>
        public string Version { get; set; }
        
        /// <summary>
        /// If set, the destination folder will contain only the package name, not the version number
        /// </summary>
        public bool ExcludeVersion { get; set; }

        /// <summary>
        /// Allows prerelease packages to be installed. This flag is not required when restoring packages by installing from packages.config.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Checks if package install consent is granted before installing a package.
        /// </summary>
        public bool RequireConsent { get; set; }

        /// <summary>
        /// Solution root for package restore.
        /// </summary>
        public DirectoryPath SolutionDirectory { get; set; }

        /// <summary>
        /// A list of packages sources to use for this command.
        /// </summary>
        public ICollection<string> Source { get; set; }

        /// <summary>
        /// Sets whether or not to use the machine cache as the first package source.
        /// </summary>
        public bool NoCache { get; set; }

        /// <summary>
        /// Disable parallel processing of packages for this command.
        /// </summary>
        public bool DisableParallelProcessing { get; set; }

        /// <summary>
        /// Display this amount of details in the output: Normal, Quiet, Detailed.
        /// </summary>
        public NuGetVerbosity? Verbosity { get; set; }

        /// <summary>
        /// The NuGet configuration file. If not specified, file %AppData%\NuGet\NuGet.config is used as configuration file.
        /// </summary>
        public FilePath ConfigFile { get; set; }
    }
}