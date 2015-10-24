using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseManager.Export
{
    /// <summary>
    /// Contains settings used by <see cref="GitReleaseManagerExporter"/>.
    /// </summary>
    public sealed class GitReleaseManagerExportSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the tag name to be used when exporting the release notes.
        /// </summary>
        public string TagName { get; set; }

        /// <summary>
        /// Gets or sets the path on which GitReleaseManager should be executed.
        /// </summary>
        public DirectoryPath TargetDirectory { get; set; }

        /// <summary>
        /// Gets or sets the path to the GitReleaseManager log file.
        /// </summary>
        public FilePath LogFilePath { get; set; }
    }
}