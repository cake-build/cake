// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.TravisCI.Data
{
    /// <summary>
    /// Provides Travis CI environment information for the current build.
    /// </summary>
    public sealed class TravisCIEnvironmentInfo : TravisCIInfo
    {
        /// <summary>
        /// Gets Travis CI build information for the current build.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Branch: {0}
        ///         BuildDirectory: {1}
        ///         BuildId: {2}",
        ///         BuildSystem.TravisCI.Environment.Build.Branch,
        ///         BuildSystem.TravisCI.Environment.Build.BuildDirectory,
        ///         BuildSystem.TravisCI.Environment.Build.BuildId
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Branch: {0}
        ///         BuildDirectory: {1}
        ///         BuildId: {2}",
        ///         TravisCI.Environment.Build.Branch,
        ///         TravisCI.Environment.Build.BuildDirectory,
        ///         TravisCI.Environment.Build.BuildId
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        public TravisCIBuildInfo Build { get; }

        /// <summary>
        /// Gets Travis CI pull request information.
        /// </summary>
        /// <value>
        /// The Travis CI pull request information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}",
        ///         BuildSystem.TravisCI.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.TravisCI.Environment.PullRequest.Id
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}",
        ///         TravisCI.Environment.PullRequest.IsPullRequest,
        ///         TravisCI.Environment.PullRequest.Id
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        public TravisCIPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Gets Travis CI job information for the current build.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Job:
        ///         JobId: {0}
        ///         JobNumber: {1}
        ///         OSName: {2}",
        ///         BuildSystem.TravisCI.Environment.Job.JobId,
        ///         BuildSystem.TravisCI.Environment.Job.JobNumber,
        ///         BuildSystem.TravisCI.Environment.Job.OSName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Job:
        ///         JobId: {0}
        ///         JobNumber: {1}
        ///         OSName: {2}",
        ///         TravisCI.Environment.Job.JobId,
        ///         TravisCI.Environment.Job.JobNumber,
        ///         TravisCI.Environment.Job.OSName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        public TravisCIJobInfo Job { get; }

        /// <summary>
        /// Gets Travis CI repository information for the current build.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Commit: {0}
        ///         CommitRange: {1}
        ///         PullRequest: {2}",
        ///         BuildSystem.TravisCI.Environment.Repository.Commit,
        ///         BuildSystem.TravisCI.Environment.Repository.CommitRange,
        ///         BuildSystem.TravisCI.Environment.Repository.PullRequest
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Commit: {0}
        ///         CommitRange: {1}
        ///         PullRequest: {2}",
        ///         TravisCI.Environment.Repository.Commit,
        ///         TravisCI.Environment.Repository.CommitRange,
        ///         TravisCI.Environment.Repository.PullRequest
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        public TravisCIRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets a value indicating whether the current build is continuous integration.
        /// </summary>
        /// <value>
        /// <c>true</c> if ci; otherwise, <c>false</c>.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"CI: {0}",
        ///         BuildSystem.TravisCI.Environment.CI
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"CI: {0}",
        ///         TravisCI.Environment.CI
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        public bool CI => GetEnvironmentBoolean("CI");

        /// <summary>
        /// Gets the Travis CI home directory.
        /// </summary>
        /// <value>
        /// The home.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Home: {0}",
        ///         BuildSystem.TravisCI.Environment.Home
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Home: {0}",
        ///         TravisCI.Environment.Home
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        public string Home => GetEnvironmentString("HOME");

        /// <summary>
        /// Gets a value indicating whether the environment is Travis.
        /// </summary>
        /// <value>
        ///   <c>true</c> if Travis; otherwise, <c>false</c>.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Travis: {0}",
        ///         BuildSystem.TravisCI.Environment.Travis
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TravisCI.</para>
        /// <example>
        /// <code>
        /// if (TravisCI.IsRunningOnTravisCI)
        /// {
        ///     Information(
        ///         @"Travis: {0}",
        ///         TravisCI.Environment.Travis
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TravisCI");
        /// }
        /// </code>
        /// </example>
        public bool Travis => GetEnvironmentBoolean("TRAVIS");

        /// <summary>
        /// Initializes a new instance of the <see cref="TravisCIEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TravisCIEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Build = new TravisCIBuildInfo(environment);
            PullRequest = new TravisCIPullRequestInfo(environment);
            Job = new TravisCIJobInfo(environment);
            Repository = new TravisCIRepositoryInfo(environment);
        }
    }
}