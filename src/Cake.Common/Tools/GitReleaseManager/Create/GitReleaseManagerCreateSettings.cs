// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseManager.Create
{
    /// <summary>
    /// Contains settings used by <see cref="GitReleaseManagerCreator"/>.
    /// </summary>
    public sealed class GitReleaseManagerCreateSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the milestone to be used when creating the release.
        /// </summary>
        public string Milestone { get; set; }

        /// <summary>
        /// Gets or sets the name to be used when creating the release.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the location of a set of Release Notes to be used when creating the release.
        /// </summary>
        public FilePath InputFilePath { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether or not to create the release as a pre-release.
        /// </summary>
        public bool Prerelease { get; set; }

        /// <summary>
        /// Gets or sets the Path(s) to the file(s) to include in the release.
        /// </summary>
        public string Assets { get; set; }

        /// <summary>
        /// Gets or sets the commit to tag. Can be a branch or SHA. Defaults to repository's default branch..
        /// </summary>
        public string TargetCommitish { get; set; }

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
