using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.List
{
    /// <summary>
    /// Contains settings used by <see cref="NuGetList"/>.
    /// </summary>
    public sealed class NuGetListSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether list all versions of a package.By default, only the latest package version is displayed.
        /// </summary>
        public bool AllVersions { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow prerelease packages to be shown.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow unlisted packages to be shown.
        /// </summary>
        public bool IncludeDelisted { get; set; }

        /// <summary>
        /// Gets or sets the NuGet configuration file. If not specified, file %AppData%\NuGet\NuGet.config is used as configuration file.
        /// </summary>
        public FilePath ConfigFile { get; set; }

        /// <summary>
        /// Gets or sets a list of packages sources to search.
        /// </summary>
        public ICollection<string> Source { get; set; } = new List<string>();
    }
}
