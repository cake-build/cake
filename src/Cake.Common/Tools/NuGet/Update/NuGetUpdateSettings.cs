using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.Update
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetUpdater"/>.
    /// </summary>
    public sealed class NuGetUpdateSettings
    {
        /// <summary>
        /// Gets or sets the path to <c>nuget.exe</c>.
        /// </summary>
        public FilePath ToolPath { get; set; }

        /// <summary>
        /// Gets or sets the package ids to update.
        /// </summary>
        /// <value>The package ids to update.</value>
        public ICollection<string> Id { get; set; }

        /// <summary>
        /// Gets or sets a list of package sources to use for this command.
        /// </summary>
        public ICollection<string> Source { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to look for updates with the highest
        /// version available within the same major and minor version as the installed package.
        /// </summary>
        /// <value>
        ///   <c>true</c> if safe; otherwise, <c>false</c>.
        /// </value>
        public bool Safe { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow updating to prerelease versions.
        /// This flag is not required when updating prerelease packages that are already installed.
        /// </summary>
        /// <value>
        ///   <c>true</c> to allow updating to prerelease versions; otherwise, <c>false</c>.
        /// </value>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets the amount of output details.
        /// </summary>
        public NuGetVerbosity? Verbosity { get; set; }
    }
}