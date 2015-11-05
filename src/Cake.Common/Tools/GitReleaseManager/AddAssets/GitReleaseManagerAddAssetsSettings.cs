using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseManager.AddAssets
{
    /// <summary>
    /// Contains settings used by <see cref="GitReleaseManagerAssetsAdder"/>.
    /// </summary>
    public sealed class GitReleaseManagerAddAssetsSettings : ToolSettings
    {
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