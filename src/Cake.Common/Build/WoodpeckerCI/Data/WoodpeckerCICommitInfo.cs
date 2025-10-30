// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.WoodpeckerCI.Data
{
    /// <summary>
    /// Provides WoodpeckerCI commit information for the current build.
    /// </summary>
    public class WoodpeckerCICommitInfo : WoodpeckerCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCICommitInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public WoodpeckerCICommitInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the commit SHA.
        /// </summary>
        /// <value>
        /// The commit SHA.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Commit SHA: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.Commit.Sha
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Commit SHA: {0}",
        ///         WoodpeckerCI.Environment.Commit.Sha
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public string Sha => GetEnvironmentString("CI_COMMIT_SHA");

        /// <summary>
        /// Gets the commit ref.
        /// </summary>
        /// <value>
        /// The commit ref.
        /// </value>
        public string Ref => GetEnvironmentString("CI_COMMIT_REF");

        /// <summary>
        /// Gets the commit ref spec.
        /// </summary>
        /// <value>
        /// The commit ref spec.
        /// </value>
        public string Refspec => GetEnvironmentString("CI_COMMIT_REFSPEC");

        /// <summary>
        /// Gets the commit branch.
        /// </summary>
        /// <value>
        /// The commit branch.
        /// </value>
        public string Branch => GetEnvironmentString("CI_COMMIT_BRANCH");

        /// <summary>
        /// Gets the commit source branch.
        /// </summary>
        /// <value>
        /// The commit source branch.
        /// </value>
        public string SourceBranch => GetEnvironmentString("CI_COMMIT_SOURCE_BRANCH");

        /// <summary>
        /// Gets the commit target branch.
        /// </summary>
        /// <value>
        /// The commit target branch.
        /// </value>
        public string TargetBranch => GetEnvironmentString("CI_COMMIT_TARGET_BRANCH");

        /// <summary>
        /// Gets the commit tag name.
        /// </summary>
        /// <value>
        /// The commit tag name.
        /// </value>
        public string Tag => GetEnvironmentString("CI_COMMIT_TAG");

        /// <summary>
        /// Gets the commit pull request number.
        /// </summary>
        /// <value>
        /// The commit pull request number.
        /// </value>
        public string PullRequest => GetEnvironmentString("CI_COMMIT_PULL_REQUEST");

        /// <summary>
        /// Gets the commit pull request labels.
        /// </summary>
        /// <value>
        /// The commit pull request labels.
        /// </value>
        public string PullRequestLabels => GetEnvironmentString("CI_COMMIT_PULL_REQUEST_LABELS");

        /// <summary>
        /// Gets the commit message.
        /// </summary>
        /// <value>
        /// The commit message.
        /// </value>
        public string Message => GetEnvironmentString("CI_COMMIT_MESSAGE");

        /// <summary>
        /// Gets the commit author username.
        /// </summary>
        /// <value>
        /// The commit author username.
        /// </value>
        public string Author => GetEnvironmentString("CI_COMMIT_AUTHOR");

        /// <summary>
        /// Gets the commit author email address.
        /// </summary>
        /// <value>
        /// The commit author email address.
        /// </value>
        public string AuthorEmail => GetEnvironmentString("CI_COMMIT_AUTHOR_EMAIL");

        /// <summary>
        /// Gets a value indicating whether the release is a pre-release.
        /// </summary>
        /// <value>
        /// <c>true</c> if the release is a pre-release; otherwise, <c>false</c>.
        /// </value>
        public bool PreRelease => GetEnvironmentBoolean("CI_COMMIT_PRERELEASE");
    }
}
