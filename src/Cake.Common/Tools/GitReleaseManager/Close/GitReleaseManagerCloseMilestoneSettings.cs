using Cake.Core.IO;

namespace Cake.Common.Tools.GitReleaseManager.Close
{
    /// <summary>
    /// Contains settings used by <see cref="GitReleaseManagerMilestoneCloser"/>.
    /// </summary>
    public sealed class GitReleaseManagerCloseMilestoneSettings
    {
        /// <summary>
        /// Gets or sets the path on which GitReleaseManager should be executed.
        /// </summary>
        public DirectoryPath TargetDirectory { get; set; }

        /// <summary>
        /// Gets or sets the path to the GitReleaseManager log file.
        /// </summary>
        public FilePath LogFilePath { get; set; }

        /// <summary>
        /// Gets or sets the path to <c>GitReleaseManager.exe</c>.
        /// </summary>
        public FilePath ToolPath { get; set; }
    }
}