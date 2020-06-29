// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides Azure Pipelines Repository information for the current build.
    /// </summary>
    public sealed class AzurePipelinesRepositoryInfo : AzurePipelinesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AzurePipelinesRepositoryInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the branch the build was queued for.
        /// </summary>
        /// <value>
        /// The full SCM branch.
        /// </value>
        public string SourceBranch => GetEnvironmentString("BUILD_SOURCEBRANCH");

        /// <summary>
        /// Gets name of the branch the build was queued for.
        /// </summary>
        /// <value>
        /// The SCM branch name.
        /// </value>
        public string SourceBranchName => GetEnvironmentString("BUILD_SOURCEBRANCHNAME");

        /// <summary>
        /// Gets the latest version control change that is included in this build.
        /// </summary>
        /// <remarks>Note: for Git this is the commit ID. For TFVC this is the changeset.</remarks>
        /// <value>
        /// The SCM source version.
        /// </value>
        public string SourceVersion => GetEnvironmentString("BUILD_SOURCEVERSION");

        /// <summary>
        /// Gets the comment of the commit or changeset.
        /// </summary>
        /// <remarks>Note: This variable is available in TFS 2015.4.</remarks>
        /// <value>
        /// The comment.
        /// </value>
        public string SourceVersionMessage => GetEnvironmentString("BUILD_SOURCEVERSIONMESSAGE");

        /// <summary>
        /// Gets the name of the shelveset you are building, if you are running a gated build or a shelveset build.
        /// </summary>
        /// <remarks>Defined only if your repository is Team Foundation Version Control.</remarks>
        /// <value>
        /// The shelveset name.
        /// </value>
        public string Shelveset => GetEnvironmentString("BUILD_SOURCETFVCSHELVESET");

        /// <summary>
        /// Gets the name of the repository.
        /// </summary>
        /// <value>
        /// The name of the repository.
        /// </value>
        public string RepoName => GetEnvironmentString("BUILD_REPOSITORY_NAME");

        /// <summary>
        /// Gets the type of the current repository.
        /// </summary>
        /// <value>
        /// The type of the current repository.
        /// </value>
        public AzurePipelinesRepositoryType? Provider => GetRepositoryType("BUILD_REPOSITORY_PROVIDER");

        /// <summary>
        /// Gets the value you've selected for Checkout submodules on the repository tab.
        /// </summary>
        /// <value>
        /// The checkout submodule value.
        /// </value>
        public string GitSubmoduleCheckout => GetEnvironmentString("BUILD_REPOSITORY_GIT_SUBMODULECHECKOUT");
    }
}