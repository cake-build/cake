// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.Bitrise.Data
{
    /// <summary>
    /// Provides Bitrise environment information for the current build.
    /// </summary>
    public class BitriseEnvironmentInfo : BitriseInfo
    {
        /// <summary>
        /// Gets Bitrise application information.
        /// </summary>
        /// <value>
        /// The application.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Application:
        ///         Title: {0}
        ///         Url: {1}
        ///         Slug: {2}",
        ///         BuildSystem.Bitrise.Environment.Application.ApplicationTitle,
        ///         BuildSystem.Bitrise.Environment.Application.ApplicationUrl,
        ///         BuildSystem.Bitrise.Environment.Application.AppSlug
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bitrise.</para>
        /// <example>
        /// <code>
        /// if (Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Application:
        ///         Title: {0}
        ///         Url: {1}
        ///         Slug: {2}",
        ///         Bitrise.Environment.Application.ApplicationTitle,
        ///         Bitrise.Environment.Application.ApplicationUrl,
        ///         Bitrise.Environment.Application.AppSlug
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        public BitriseApplicationInfo Application { get; }

        /// <summary>
        /// Gets Bitrise build information.
        /// </summary>
        /// <value>
        /// The build.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Build Number: {0}
        ///         Build Url: {1}
        ///         Build Slug: {2}
        ///         Build Trigger Timestamp: {3}
        ///         Build Status: {4}",
        ///         BuildSystem.Bitrise.Environment.Build.BuildNumber,
        ///         BuildSystem.Bitrise.Environment.Build.BuildUrl,
        ///         BuildSystem.Bitrise.Environment.Build.BuildSlug,
        ///         BuildSystem.Bitrise.Environment.Build.BuildTriggerTimestamp,
        ///         BuildSystem.Bitrise.Environment.Build.BuildStatus
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bitrise.</para>
        /// <example>
        /// <code>
        /// if (Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Build Number: {0}
        ///         Build Url: {1}
        ///         Build Slug: {2}
        ///         Build Trigger Timestamp: {3}
        ///         Build Status: {4}",
        ///         Bitrise.Environment.Build.BuildNumber,
        ///         Bitrise.Environment.Build.BuildUrl,
        ///         Bitrise.Environment.Build.BuildSlug,
        ///         Bitrise.Environment.Build.BuildTriggerTimestamp,
        ///         Bitrise.Environment.Build.BuildStatus
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        public BitriseBuildInfo Build { get; }

        /// <summary>
        /// Gets Bitrise pull request information.
        /// </summary>
        /// <value>
        /// The Bitrise pull request information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}",
        ///         BuildSystem.Bitrise.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.Bitrise.Environment.PullRequest.Id
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bitrise.</para>
        /// <example>
        /// <code>
        /// if (Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}",
        ///         Bitrise.Environment.PullRequest.IsPullRequest,
        ///         Bitrise.Environment.PullRequest.Id
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        public BitrisePullRequestInfo PullRequest { get; }

        /// <summary>
        /// Gets Bitrise directory information.
        /// </summary>
        /// <value>
        /// The directory.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Directory:
        ///         Source Directory: {0}
        ///         Deploy Directory: {1}",
        ///         BuildSystem.Bitrise.Environment.Directory.SourceDirectory,
        ///         BuildSystem.Bitrise.Environment.Directory.DeployDirectory
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bitrise.</para>
        /// <example>
        /// <code>
        /// if (Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Directory:
        ///         Source Directory: {0}
        ///         Deploy Directory: {1}",
        ///         Bitrise.Environment.Directory.SourceDirectory,
        ///         Bitrise.Environment.Directory.DeployDirectory
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        public BitriseDirectoryInfo Directory { get; }

        /// <summary>
        /// Gets Bitrise provisioning information.
        /// </summary>
        /// <value>
        /// The provisioning.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Provisioning:
        ///         Provision Url: {0}
        ///         Certificate Url: {1}
        ///         Certificate Passphrase: {2}",
        ///         BuildSystem.Bitrise.Environment.Provisioning.ProvisionUrl,
        ///         BuildSystem.Bitrise.Environment.Provisioning.CertificateUrl,
        ///         BuildSystem.Bitrise.Environment.Provisioning.CertificatePassphrase
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bitrise.</para>
        /// <example>
        /// <code>
        /// if (Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Provisioning:
        ///         Provision Url: {0}
        ///         Certificate Url: {1}
        ///         Certificate Passphrase: {2}",
        ///         Bitrise.Environment.Provisioning.ProvisionUrl,
        ///         Bitrise.Environment.Provisioning.CertificateUrl,
        ///         Bitrise.Environment.Provisioning.CertificatePassphrase
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        public BitriseProvisioningInfo Provisioning { get; }

        /// <summary>
        /// Gets Bitrise repository information.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Git Repository Url: {0}
        ///         Git Branch: {1}
        ///         Git Tag: {2}
        ///         Git Commit: {3}
        ///         Pull Request: {4}",
        ///         BuildSystem.Bitrise.Environment.Repository.GitRepositoryUrl,
        ///         BuildSystem.Bitrise.Environment.Repository.GitBranch,
        ///         BuildSystem.Bitrise.Environment.Repository.GitTag,
        ///         BuildSystem.Bitrise.Environment.Repository.GitCommit,
        ///         BuildSystem.Bitrise.Environment.Repository.PullRequest
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bitrise.</para>
        /// <example>
        /// <code>
        /// if (Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Git Repository Url: {0}
        ///         Git Branch: {1}
        ///         Git Tag: {2}
        ///         Git Commit: {3}
        ///         Pull Request: {4}",
        ///         Bitrise.Environment.Repository.GitRepositoryUrl,
        ///         Bitrise.Environment.Repository.GitBranch,
        ///         Bitrise.Environment.Repository.GitTag,
        ///         Bitrise.Environment.Repository.GitCommit,
        ///         Bitrise.Environment.Repository.PullRequest
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        public BitriseRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets Bitrise workflow information.
        /// </summary>
        /// <value>
        /// The workflow.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Workflow:
        ///         Workflow Id: {0}
        ///         Workflow Title: {1}",
        ///         BuildSystem.Bitrise.Environment.Workflow.WorkflowId,
        ///         BuildSystem.Bitrise.Environment.Workflow.WorkflowTitle
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        /// <para>Via Bitrise.</para>
        /// <example>
        /// <code>
        /// if (Bitrise.IsRunningOnBitrise)
        /// {
        ///     Information(
        ///         @"Workflow:
        ///         Workflow Id: {0}
        ///         Workflow Title: {1}",
        ///         Bitrise.Environment.Workflow.WorkflowId,
        ///         Bitrise.Environment.Workflow.WorkflowTitle
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Bitrise");
        /// }
        /// </code>
        /// </example>
        public BitriseWorkflowInfo Workflow { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseEnvironmentInfo(ICakeEnvironment environment) : base(environment)
        {
            Application = new BitriseApplicationInfo(environment);
            Build = new BitriseBuildInfo(environment);
            PullRequest = new BitrisePullRequestInfo(environment);
            Provisioning = new BitriseProvisioningInfo(environment);
            Repository = new BitriseRepositoryInfo(environment);
            Workflow = new BitriseWorkflowInfo(environment);
            Directory = new BitriseDirectoryInfo(environment);
        }
    }
}