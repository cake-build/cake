// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.AppVeyor.Data
{
    /// <summary>
    /// Provides AppVeyor repository information for a current build.
    /// </summary>
    public sealed class AppVeyorRepositoryInfo : AppVeyorInfo
    {
        private readonly AppVeyorTagInfo _tagProvider;
        private readonly AppVeyorCommitInfo _commitProvider;

        /// <summary>
        /// Gets the repository provider.
        /// <list type="bullet">
        ///   <item>
        ///     <description>github</description>
        ///   </item>
        ///   <item>
        ///     <description>bitbucket</description>
        ///   </item>
        ///   <item>
        ///     <description>kiln</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <value>
        ///   The repository provider.
        /// </value>
        public string Provider
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_PROVIDER"); }
        }

        /// <summary>
        /// Gets the revision control system.
        /// <list type="bullet">
        ///   <item>
        ///     <description>git</description>
        ///   </item>
        ///   <item>
        ///     <description>mercurial</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <value>
        ///   The revision control system.
        /// </value>
        public string Scm
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_SCM"); }
        }

        /// <summary>
        /// Gets the repository name in format owner-name/repo-name.
        /// </summary>
        /// <value>
        ///   The repository name.
        /// </value>
        public string Name
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_NAME"); }
        }

        /// <summary>
        /// Gets the build branch. For pull request commits it is base branch PR is merging into.
        /// </summary>
        /// <value>
        ///   The build branch.
        /// </value>
        public string Branch
        {
            get { return GetEnvironmentString("APPVEYOR_REPO_BRANCH"); }
        }

        /// <summary>
        /// Gets the tag information for the build.
        /// </summary>
        /// <value>
        ///   The tag information for the build.
        /// </value>
        public AppVeyorTagInfo Tag
        {
            get { return _tagProvider; }
        }

        /// <summary>
        /// Gets the commit information for the build.
        /// </summary>
        /// <value>
        ///   The commit information for the build.
        /// </value>
        public AppVeyorCommitInfo Commit
        {
            get { return _commitProvider; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AppVeyorRepositoryInfo(ICakeEnvironment environment)
            : base(environment)
        {
            _tagProvider = new AppVeyorTagInfo(environment);
            _commitProvider = new AppVeyorCommitInfo(environment);
        }
    }
}
