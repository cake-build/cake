// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.WoodpeckerCI.Data
{
    /// <summary>
    /// Provides WoodpeckerCI repository information for the current build.
    /// </summary>
    public class WoodpeckerCIRepositoryInfo : WoodpeckerCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCIRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public WoodpeckerCIRepositoryInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the repository full name.
        /// </summary>
        /// <value>
        /// The repository full name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Repository: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.Repository.Repo
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
        ///         @"Repository: {0}",
        ///         WoodpeckerCI.Environment.Repository.Repo
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public string Repo => GetEnvironmentString("CI_REPO");

        /// <summary>
        /// Gets the repository owner.
        /// </summary>
        /// <value>
        /// The repository owner.
        /// </value>
        public string RepoOwner => GetEnvironmentString("CI_REPO_OWNER");

        /// <summary>
        /// Gets the repository name.
        /// </summary>
        /// <value>
        /// The repository name.
        /// </value>
        public string RepoName => GetEnvironmentString("CI_REPO_NAME");

        /// <summary>
        /// Gets the repository remote ID.
        /// </summary>
        /// <value>
        /// The repository remote ID.
        /// </value>
        public string RepoRemoteId => GetEnvironmentString("CI_REPO_REMOTE_ID");

        /// <summary>
        /// Gets the repository URL.
        /// </summary>
        /// <value>
        /// The repository URL.
        /// </value>
        public Uri RepoUrl => GetEnvironmentUri("CI_REPO_URL");

        /// <summary>
        /// Gets the repository clone URL.
        /// </summary>
        /// <value>
        /// The repository clone URL.
        /// </value>
        public Uri RepoCloneUrl => GetEnvironmentUri("CI_REPO_CLONE_URL");

        /// <summary>
        /// Gets the repository SSH clone URL.
        /// </summary>
        /// <value>
        /// The repository SSH clone URL.
        /// </value>
        public string RepoCloneSshUrl => GetEnvironmentString("CI_REPO_CLONE_SSH_URL");

        /// <summary>
        /// Gets the repository default branch.
        /// </summary>
        /// <value>
        /// The repository default branch.
        /// </value>
        public string RepoDefaultBranch => GetEnvironmentString("CI_REPO_DEFAULT_BRANCH");

        /// <summary>
        /// Gets a value indicating whether the repository is private.
        /// </summary>
        /// <value>
        /// <c>true</c> if the repository is private; otherwise, <c>false</c>.
        /// </value>
        public bool RepoPrivate => GetEnvironmentBoolean("CI_REPO_PRIVATE");

        /// <summary>
        /// Gets a value indicating whether the repository has trusted network access.
        /// </summary>
        /// <value>
        /// <c>true</c> if the repository has trusted network access; otherwise, <c>false</c>.
        /// </value>
        public bool RepoTrustedNetwork => GetEnvironmentBoolean("CI_REPO_TRUSTED_NETWORK");

        /// <summary>
        /// Gets a value indicating whether the repository has trusted volumes access.
        /// </summary>
        /// <value>
        /// <c>true</c> if the repository has trusted volumes access; otherwise, <c>false</c>.
        /// </value>
        public bool RepoTrustedVolumes => GetEnvironmentBoolean("CI_REPO_TRUSTED_VOLUMES");

        /// <summary>
        /// Gets a value indicating whether the repository has trusted security access.
        /// </summary>
        /// <value>
        /// <c>true</c> if the repository has trusted security access; otherwise, <c>false</c>.
        /// </value>
        public bool RepoTrustedSecurity => GetEnvironmentBoolean("CI_REPO_TRUSTED_SECURITY");
    }
}
