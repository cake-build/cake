// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GitLabCI.Data
{
    /// <summary>
    /// Provides GitLab CI project information for a current build.
    /// </summary>
    public sealed class GitLabCIProjectInfo : GitLabCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabCIProjectInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitLabCIProjectInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the unique id of the current project that GitLab CI uses internally.
        /// </summary>
        /// <value>
        /// The project ID.
        /// </value>
        public int Id => GetEnvironmentInteger("CI_PROJECT_ID");

        /// <summary>
        /// Gets the project name that is currently being built.
        /// </summary>
        /// <value>
        /// The project name.
        /// </value>
        public string Name => GetEnvironmentString("CI_PROJECT_NAME");

        /// <summary>
        /// Gets the project namespace (username or groupname) that is currently being built.
        /// </summary>
        /// <value>
        /// The project namespace.
        /// </value>
        public string Namespace => GetEnvironmentString("CI_PROJECT_NAMESPACE");

        /// <summary>
        /// Gets the namespace with project name.
        /// </summary>
        /// <value>
        /// The project namespace and project name.
        /// </value>
        public string Path => GetEnvironmentString("CI_PROJECT_PATH");

        /// <summary>
        /// Gets the HTTP address to access the project.
        /// </summary>
        /// <value>
        /// The HTTP address to access the project.
        /// </value>
        public string Url => GetEnvironmentString("CI_PROJECT_URL");

        /// <summary>
        /// Gets the full path where the repository is cloned and where the build is run.
        /// </summary>
        /// <value>
        /// The full path where the repository is cloned and where the build is run.
        /// </value>
        public string Directory => GetEnvironmentString("CI_PROJECT_DIR");
    }
}
