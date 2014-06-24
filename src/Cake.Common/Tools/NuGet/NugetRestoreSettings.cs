using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet
{
    public sealed class NugetRestoreSettings
    {
        /// <summary>
        /// Solution to restore packages for
        /// </summary>
        public FilePath Solution { get; private set; }

        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Checks if package restore consent is granted before installing a package.
        /// </summary>
        public bool RequireConsent { get; set; }

        /// <summary>
        /// Specifies the packages folder.
        /// </summary>
        public DirectoryPath PackagesDirectory { get; set; }

        /// <summary>
        /// A list of packages sources to use for this command.
        /// </summary>
        public ICollection<string>  Source { get; set; }

        /// <summary>
        /// Disable using the machine cache as the first package source.
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

        public NugetRestoreSettings(FilePath solution)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }
            
            Solution = solution;
        }
    }
}