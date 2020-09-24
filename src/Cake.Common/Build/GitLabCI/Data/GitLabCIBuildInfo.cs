// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GitLabCI.Data
{
    /// <summary>
    /// Provide GitLab CI build information for a current build.
    /// </summary>
    public sealed class GitLabCIBuildInfo : GitLabCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabCIBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitLabCIBuildInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the unique id of the current build that GitLab CI uses internally.
        /// </summary>
        /// <value>
        /// The build ID.
        /// </value>
        public int Id => GetEnvironmentInteger("CI_JOB_ID", "CI_BUILD_ID");

        /// <summary>
        /// Gets the commit revision for which project is built.
        /// </summary>
        /// <value>
        /// The commit revision hash.
        /// </value>
        public string Reference => GetEnvironmentString("CI_COMMIT_SHA", "CI_BUILD_REF");

        /// <summary>
        /// Gets the commit tag name. Present only when building tags.
        /// </summary>
        /// <value>
        /// The build tag name.
        /// </value>
        public string Tag => GetEnvironmentString("CI_COMMIT_TAG", "CI_BUILD_TAG");

        /// <summary>
        /// Gets the name of the build as defined in .gitlab-ci.yml.
        /// </summary>
        /// <value>
        /// The name of the build.
        /// </value>
        public string Name => GetEnvironmentString("CI_JOB_NAME", "CI_BUILD_NAME");

        /// <summary>
        /// Gets the name of the stage as defined in .gitlab-ci.yml.
        /// </summary>
        /// <value>
        /// The name of the current stage.
        /// </value>
        public string Stage => GetEnvironmentString("CI_JOB_STAGE", "CI_BUILD_STAGE");

        /// <summary>
        /// Gets the branch or tag name for which project is built.
        /// </summary>
        /// <value>
        /// The branch or tag for this build.
        /// </value>
        public string RefName => GetEnvironmentString("CI_COMMIT_REF_NAME", "CI_BUILD_REF_NAME");

        /// <summary>
        /// Gets the URL to clone the Git repository.
        /// </summary>
        /// <value>
        /// The repository URL.
        /// </value>
        public string RepoUrl => GetEnvironmentString("CI_REPOSITORY_URL", "CI_BUILD_REPO");

        /// <summary>
        /// Gets a value indicating whether the build was triggered.
        /// </summary>
        /// <value>
        /// <c>True</c> if the build was triggered, otherwise <c>false</c>.
        /// </value>
        public bool Triggered => GetEnvironmentBoolean("CI_PIPELINE_TRIGGERED", "CI_BUILD_TRIGGERED");

        /// <summary>
        /// Gets a value indicating whether the build was manually started.
        /// </summary>
        /// <value>
        /// <c>True</c> if the build was started manually, otherwise <c>false</c>.
        /// </value>
        public bool Manual => GetEnvironmentBoolean("CI_JOB_MANUAL", "CI_BUILD_MANUAL");

        /// <summary>
        /// Gets the token used for authenticating with the GitLab Container Registry.
        /// </summary>
        /// <value>
        /// The build authorisation token.
        /// </value>
        public string Token => GetEnvironmentString("CI_JOB_TOKEN", "CI_BUILD_TOKEN");

        /// <summary>
        /// Gets the unique id of the current pipeline that GitLab CI uses internally.
        /// </summary>
        /// <value>
        /// The unique build ID.
        /// </value>
        public int PipelineId => GetEnvironmentInteger("CI_PIPELINE_ID");

        /// <summary>
        /// Gets the unique id of the current pipeline scoped to the project.
        /// </summary>
        /// <value>
        /// The unique build ID.
        /// </value>
        public int PipelineIId => GetEnvironmentInteger("CI_PIPELINE_IID");

        /// <summary>
        /// Gets the id of the user who started the build.
        /// </summary>
        /// <value>
        /// The user ID.
        /// </value>
        public int UserId => GetEnvironmentInteger("GITLAB_USER_ID");

        /// <summary>
        /// Gets the email of the user who started the build.
        /// </summary>
        /// <value>
        /// The email address of the user.
        /// </value>
        public string UserEmail => GetEnvironmentString("GITLAB_USER_EMAIL");
    }
}
