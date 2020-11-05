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
        ///   <item>
        ///     <description>vso</description>
        ///   </item>
        ///   <item>
        ///     <description>gitlab</description>
        ///   </item>
        /// </list>
        /// </summary>
        /// <value>
        ///   The repository provider.
        /// </value>
        public string Provider => GetEnvironmentString("APPVEYOR_REPO_PROVIDER");

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
        public string Scm => GetEnvironmentString("APPVEYOR_REPO_SCM");

        /// <summary>
        /// Gets the repository name in format owner-name/repo-name.
        /// </summary>
        /// <value>
        ///   The repository name.
        /// </value>
        public string Name => GetEnvironmentString("APPVEYOR_REPO_NAME");

        /// <summary>
        /// Gets the build branch. For pull request commits it is base branch PR is merging into.
        /// </summary>
        /// <value>
        ///   The build branch.
        /// </value>
        public string Branch => GetEnvironmentString("APPVEYOR_REPO_BRANCH");

        /// <summary>
        /// Gets the tag information for the build.
        /// </summary>
        /// <value>
        ///   The tag information for the build.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         IsTag: {0}
        ///         Name: {1}",
        ///         BuildSystem.AppVeyor.Environment.Repository.Tag.IsTag,
        ///         BuildSystem.AppVeyor.Environment.Repository.Tag.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         IsTag: {0}
        ///         Name: {1}",
        ///         AppVeyor.Environment.Repository.Tag.IsTag,
        ///         AppVeyor.Environment.Repository.Tag.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public AppVeyorTagInfo Tag { get; }

        /// <summary>
        /// Gets the commit information for the build.
        /// </summary>
        /// <value>
        ///   The commit information for the build.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Author: {0}
        ///         Email: {1}
        ///         ExtendedMessage: {2}
        ///         Id: {3}
        ///         Message: {4}
        ///         Timestamp: {5}",
        ///         BuildSystem.AppVeyor.Environment.Repository.Commit.Author,
        ///         BuildSystem.AppVeyor.Environment.Repository.Commit.Email,
        ///         BuildSystem.AppVeyor.Environment.Repository.Commit.ExtendedMessage,
        ///         BuildSystem.AppVeyor.Environment.Repository.Commit.Id,
        ///         BuildSystem.AppVeyor.Environment.Repository.Commit.Message,
        ///         BuildSystem.AppVeyor.Environment.Repository.Commit.Timestamp
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AppVeyor.</para>
        /// <example>
        /// <code>
        /// if (AppVeyor.IsRunningOnAppVeyor)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Author: {0}
        ///         Email: {1}
        ///         ExtendedMessage: {2}
        ///         Id: {3}
        ///         Message: {4}
        ///         Timestamp: {5}",
        ///         AppVeyor.Environment.Repository.Commit.Author,
        ///         AppVeyor.Environment.Repository.Commit.Email,
        ///         AppVeyor.Environment.Repository.Commit.ExtendedMessage,
        ///         AppVeyor.Environment.Repository.Commit.Id,
        ///         AppVeyor.Environment.Repository.Commit.Message,
        ///         AppVeyor.Environment.Repository.Commit.Timestamp
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AppVeyor");
        /// }
        /// </code>
        /// </example>
        public AppVeyorCommitInfo Commit { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AppVeyorRepositoryInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Tag = new AppVeyorTagInfo(environment);
            Commit = new AppVeyorCommitInfo(environment);
        }
    }
}