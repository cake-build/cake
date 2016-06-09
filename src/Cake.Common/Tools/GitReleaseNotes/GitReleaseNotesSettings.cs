// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitReleaseNotes
{
    /// <summary>
    /// Contains settings used by <see cref="GitReleaseNotesRunner" />.
    /// </summary>
    public sealed class GitReleaseNotesSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the Working Directory for generating release notes from.
        /// </summary>
        public DirectoryPath WorkingDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether verbose logging is used.
        /// </summary>
        public bool Verbose { get; set; }

        /// <summary>
        /// Gets or sets the IssueTracker to use.
        /// </summary>
        public GitReleaseNotesIssueTracker? IssueTracker { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all tags should be included in the releasenotes.
        /// </summary>
        /// <remarks>If not specified then only the issues since that last tag are included.</remarks>
        public bool AllTags { get; set; }

        /// <summary>
        /// Gets or sets the username to use when accessing repository.
        /// </summary>
        public string RepoUserName { get; set; }

        /// <summary>
        /// Gets or sets the password to use when accessing repository.
        /// </summary>
        public string RepoPassword { get; set; }

        /// <summary>
        /// Gets or sets the token to use when accessing repository.
        /// </summary>
        /// <remarks>To be used instead of username/password</remarks>
        public string RepoToken { get; set; }

        /// <summary>
        /// Gets or sets the Url to use when accessing repository.
        /// </summary>
        /// <remarks>To be used instead of username/password</remarks>
        public string RepoUrl { get; set; }

        /// <summary>
        /// Gets or sets the branch name to checkout any existing release notes file.
        /// </summary>
        /// <remarks>To be used instead of username/password</remarks>
        public string RepoBranch { get; set; }

        /// <summary>
        /// Gets or sets the Url to the Issue Tracker.
        /// </summary>
        /// <remarks>To be used instead of username/password</remarks>
        public string IssueTrackerUrl { get; set; }

        /// <summary>
        /// Gets or sets the username to use when accessing issue tracker.
        /// </summary>
        public string IssueTrackerUserName { get; set; }

        /// <summary>
        /// Gets or sets the password to use when accessing issue tracker.
        /// </summary>
        public string IssueTrackerPassword { get; set; }

        /// <summary>
        /// Gets or sets the token to use when accessing issue tracker.
        /// </summary>
        /// <remarks>To be used instead of username/password</remarks>
        public string IssueTrackerToken { get; set; }

        /// <summary>
        /// Gets or sets the Project Id to use when accessing issue tracker.
        /// </summary>
        public string IssueTrackerProjectId { get; set; }

        /// <summary>
        /// Gets or sets the Jql query for closed issues you would like included if mentioned.
        /// </summary>
        public string IssueTrackerFilter { get; set; }

        /// <summary>
        /// Gets or sets a value which allows additional labels to be treated as categories.
        /// </summary>
        public string Categories { get; set; }

        /// <summary>
        /// Gets or sets a value which specifies the version to publish.
        /// </summary>
        public string Version { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether all labels should be included in the releasenotes.
        /// </summary>
        /// <remarks>If not specified then only the defaults (bug, enhancement, feature) are included.</remarks>
        public bool AllLabels { get; set; }
    }
}
