// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Jenkins.Data
{
    /// <summary>
    /// Provides Jenkins change information for the current build in multibranch projects.
    /// </summary>
    public class JenkinsChangeInfo : JenkinsInfo
    {
        /// <summary>
        /// Gets the change id.
        /// </summary>
        /// <value>
        /// The changed id such as a pull request number.
        /// </value>
        public string Id => GetEnvironmentString("CHANGE_ID");

        /// <summary>
        /// Gets a value indicating whether the current build was started by a pull request.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current build was started by a pull request; otherwise, <c>false</c>.
        /// </value>
        public bool IsPullRequest => !string.IsNullOrWhiteSpace(Id);

        /// <summary>
        /// Gets the change URL.
        /// </summary>
        /// <value>
        /// The change URL.
        /// </value>
        public string Url => GetEnvironmentString("CHANGE_URL");

        /// <summary>
        /// Gets the change title.
        /// </summary>
        /// <value>
        /// The change title.
        /// </value>
        public string Title => GetEnvironmentString("CHANGE_TITLE");

        /// <summary>
        /// Gets change author.
        /// </summary>
        /// <value>
        /// The change author.
        /// </value>
        public string Author => GetEnvironmentString("CHANGE_AUTHOR");

        /// <summary>
        /// Gets the human name of the change author.
        /// </summary>
        /// <value>
        /// The human name of the change author.
        /// </value>
        public string AuthorDisplayName => GetEnvironmentString("CHANGE_AUTHOR_DISPLAY_NAME");

        /// <summary>
        /// Gets the email address of the change author.
        /// </summary>
        /// <value>
        /// The email address of the change author.
        /// </value>
        public string AuthorEmail => GetEnvironmentString("CHANGE_AUTHOR_EMAIL");

        /// <summary>
        /// Gets the target/base branch of the change.
        /// </summary>
        /// <value>
        /// The target of the change.
        /// </value>
        public string Target => GetEnvironmentString("CHANGE_TARGET");

        /// <summary>
        /// Gets the origin branch of the change.
        /// </summary>
        /// <value>
        /// The origin branch of the change.
        /// </value>
        public string Branch => GetEnvironmentString("CHANGE_BRANCH");

        /// <summary>
        /// Gets the fork name of the change.
        /// </summary>
        /// <value>
        /// The fork name of the change.
        /// </value>
        public string Fork => GetEnvironmentString("CHANGE_FORK");

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsChangeInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsChangeInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
