// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Jenkins.Data
{
    /// <summary>
    /// Provides Jenkins environment information for the current build.
    /// </summary>
    public sealed class JenkinsEnvironmentInfo : JenkinsInfo
    {
        /// <summary>
        /// Gets Jenkins build information.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Build:
        ///         BuildNumber: {0}
        ///         BuildId: {1}
        ///         BuildDisplayName: {2}",
        ///         BuildSystem.Jenkins.Environment.Build.BuildNumber,
        ///         BuildSystem.Jenkins.Environment.Build.BuildId,
        ///         BuildSystem.Jenkins.Environment.Build.BuildDisplayName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Jenkins.</para>
        /// <example>
        /// <code>
        /// if (Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Build:
        ///         BuildNumber: {0}
        ///         BuildId: {1}
        ///         BuildDisplayName: {2}",
        ///         Jenkins.Environment.Build.BuildNumber,
        ///         Jenkins.Environment.Build.BuildId,
        ///         Jenkins.Environment.Build.BuildDisplayName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        public JenkinsBuildInfo Build { get; }

        /// <summary>
        /// Gets Jenkins repository information.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         BranchName: {0}
        ///         GitCommitSha: {1}
        ///         GitBranch: {2}",
        ///         BuildSystem.Jenkins.Environment.Repository.BranchName,
        ///         BuildSystem.Jenkins.Environment.Repository.GitCommitSha,
        ///         BuildSystem.Jenkins.Environment.Repository.GitBranch
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Jenkins.</para>
        /// <example>
        /// <code>
        /// if (Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         BranchName: {0}
        ///         GitCommitSha: {1}
        ///         GitBranch: {2}",
        ///         Jenkins.Environment.Repository.BranchName,
        ///         Jenkins.Environment.Repository.GitCommitSha,
        ///         Jenkins.Environment.Repository.GitBranch
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        public JenkinsRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets Jenkins job information.
        /// </summary>
        /// <value>
        /// The job.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Job:
        ///         JobName: {0}
        ///         JobBaseName: {1}
        ///         JobUrl: {2}",
        ///         BuildSystem.Jenkins.Environment.Job.JobName,
        ///         BuildSystem.Jenkins.Environment.Job.JobBaseName,
        ///         BuildSystem.Jenkins.Environment.Job.JobUrl
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Jenkins.</para>
        /// <example>
        /// <code>
        /// if (Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Job:
        ///         JobName: {0}
        ///         JobBaseName: {1}
        ///         JobUrl: {2}",
        ///         Jenkins.Environment.Job.JobName,
        ///         Jenkins.Environment.Job.JobBaseName,
        ///         Jenkins.Environment.Job.JobUrl
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        public JenkinsJobInfo Job { get; }

        /// <summary>
        /// Gets the node.
        /// </summary>
        /// <value>
        /// The node.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Node:
        ///         NodeName: {0}
        ///         NodeLabels: {1}",
        ///         BuildSystem.Jenkins.Environment.Node.NodeName,
        ///         BuildSystem.Jenkins.Environment.Node.NodeLabels
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Jenkins.</para>
        /// <example>
        /// <code>
        /// if (Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Node:
        ///         NodeName: {0}
        ///         NodeLabels: {1}",
        ///         Jenkins.Environment.Node.NodeName,
        ///         Jenkins.Environment.Node.NodeLabels
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        public JenkinsNodeInfo Node { get; }

        /// <summary>
        /// Gets the change.
        /// </summary>
        /// <value>
        /// The change.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Change:
        ///         Id: {0}
        ///         Url: {1}
        ///         Title: {2}",
        ///         BuildSystem.Jenkins.Environment.Change.Id,
        ///         BuildSystem.Jenkins.Environment.Change.Url,
        ///         BuildSystem.Jenkins.Environment.Change.Title
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Jenkins.</para>
        /// <example>
        /// <code>
        /// if (Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"Change:
        ///         Id: {0}
        ///         Url: {1}
        ///         Title: {2}",
        ///         Jenkins.Environment.Change.Id,
        ///         Jenkins.Environment.Change.Url,
        ///         Jenkins.Environment.Change.Title
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        public JenkinsChangeInfo Change { get; }

        /// <summary>
        /// Gets the absolute path of the directory assigned on the master node for Jenkins to store data.
        /// </summary>
        /// <value>
        /// The absolute path of the directory assigned on the master node for Jenkins to store data.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"JenkinsHome: {0}",
        ///         BuildSystem.Jenkins.Environment.JenkinsHome
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Jenkins.</para>
        /// <example>
        /// <code>
        /// if (Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"JenkinsHome: {0}",
        ///         Jenkins.Environment.JenkinsHome
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        public string JenkinsHome => GetEnvironmentString("JENKINS_HOME");

        /// <summary>
        /// Gets the full URL of Jenkins (note: only available if Jenkins URL is set in system configuration).
        /// </summary>
        /// <value>
        /// The full URL of Jenkins.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"JenkinsUrl: {0}",
        ///         BuildSystem.Jenkins.Environment.JenkinsUrl
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Jenkins.</para>
        /// <example>
        /// <code>
        /// if (Jenkins.IsRunningOnJenkins)
        /// {
        ///     Information(
        ///         @"JenkinsUrl: {0}",
        ///         Jenkins.Environment.JenkinsUrl
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Jenkins");
        /// }
        /// </code>
        /// </example>
        public string JenkinsUrl => GetEnvironmentString("JENKINS_URL");

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsEnvironmentInfo" /> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsEnvironmentInfo(ICakeEnvironment environment) : base(environment)
        {
            Build = new JenkinsBuildInfo(environment);
            Repository = new JenkinsRepositoryInfo(environment);
            Node = new JenkinsNodeInfo(environment);
            Job = new JenkinsJobInfo(environment);
            Change = new JenkinsChangeInfo(environment);
        }
    }
}