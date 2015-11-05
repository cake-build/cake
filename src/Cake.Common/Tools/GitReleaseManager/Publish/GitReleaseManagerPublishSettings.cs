using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseManager.Publish
{
    /// <summary>
    /// Contains settings used by <see cref="GitReleaseManagerPublisher"/>.
    /// </summary>
    public sealed class GitReleaseManagerPublishSettings : ToolSettings
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