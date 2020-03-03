// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides Azure Pipelines Environment information for the current build.
    /// </summary>
    public sealed class AzurePipelinesEnvironmentInfo : AzurePipelinesInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AzurePipelinesEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Repository = new AzurePipelinesRepositoryInfo(environment);
            BuildDefinition = new AzurePipelinesDefinitionInfo(environment);
            Build = new AzurePipelinesBuildInfo(environment);
            PullRequest = new AzurePipelinesPullRequestInfo(environment);
            Agent = new AzurePipelinesAgentInfo(environment);
            TeamProject = new AzurePipelinesTeamProjectInfo(environment);
        }

        /// <summary>
        /// Gets Azure Pipelines repository information
        /// </summary>
        /// <value>
        /// The Azure Pipelines repository information.
        /// </value>
        /// <para>Via BuildSystem</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         SourceVersion: {1}
        ///         Shelveset: {2}",
        ///         BuildSystem.TFBuild.Environment.Repository.Branch,
        ///         BuildSystem.TFBuild.Environment.Repository.SourceVersion,
        ///         BuildSystem.TFBuild.Environment.Repository.Shelveset
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TFBuild</para>
        /// <example>
        /// <code>
        /// if (TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         SourceVersion: {1}
        ///         Shelveset: {2}",
        ///         TFBuild.Environment.Repository.Branch,
        ///         TFBuild.Environment.Repository.SourceVersion,
        ///         TFBuild.Environment.Repository.Shelveset
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets TF Build Definition information.
        /// </summary>
        /// <value>
        /// The TF Build Definition.
        /// </value>
        /// <para>Via BuildSystem</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"BuildDefinition:
        ///         Id: {0}
        ///         Name: {1}
        ///         Version: {2}",
        ///         BuildSystem.TFBuild.Environment.BuildDefinition.Id,
        ///         BuildSystem.TFBuild.Environment.BuildDefinition.Name,
        ///         BuildSystem.TFBuild.Environment.BuildDefinition.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TFBuild</para>
        /// <example>
        /// <code>
        /// if (TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"BuildDefinition:
        ///         Id: {0}
        ///         Name: {1}
        ///         Version: {2}",
        ///         TFBuild.Environment.BuildDefinition.Id,
        ///         TFBuild.Environment.BuildDefinition.Name,
        ///         TFBuild.Environment.BuildDefinition.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesDefinitionInfo BuildDefinition { get; }

        /// <summary>
        /// Gets TF Build information.
        /// </summary>
        /// <value>
        /// The TF Build.
        /// </value>
        /// <para>Via BuildSystem</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Id: {0}
        ///         Number: {1}
        ///         QueuedBy: {2}",
        ///         BuildSystem.TFBuild.Environment.Build.Id,
        ///         BuildSystem.TFBuild.Environment.Build.Number,
        ///         BuildSystem.TFBuild.Environment.Build.QueuedBy
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TFBuild</para>
        /// <example>
        /// <code>
        /// if (TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Id: {0}
        ///         Number: {1}
        ///         QueuedBy: {2}",
        ///         TFBuild.Environment.Build.Id,
        ///         TFBuild.Environment.Build.Number,
        ///         TFBuild.Environment.Build.QueuedBy
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesBuildInfo Build { get; }

        /// <summary>
        /// Gets TF Build pull request information.
        /// </summary>
        /// <value>
        /// The TF Build pull request information.
        /// </value>
        /// <para>Via BuildSystem</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}
        ///         Number: {2}",
        ///         BuildSystem.TFBuild.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.TFBuild.Environment.PullRequest.Id,
        ///         BuildSystem.TFBuild.Environment.PullRequest.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TFBuild</para>
        /// <example>
        /// <code>
        /// if (TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}
        ///         Number: {2}",
        ///         TFBuild.Environment.PullRequest.IsPullRequest,
        ///         TFBuild.Environment.PullRequest.Id,
        ///         TFBuild.Environment.PullRequest.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Gets TF Team Project information.
        /// </summary>
        /// <value>
        /// The TF Team Project.
        /// </value>
        /// <para>Via BuildSystem</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"TeamProject:
        ///         Id: {0}
        ///         Name: {1}",
        ///         BuildSystem.TFBuild.Environment.TeamProject.Id,
        ///         BuildSystem.TFBuild.Environment.TeamProject.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TFBuild</para>
        /// <example>
        /// <code>
        /// if (TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"TeamProject:
        ///         Id: {0}
        ///         Name: {1}",
        ///         TFBuild.Environment.TeamProject.Id,
        ///         TFBuild.Environment.TeamProject.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesTeamProjectInfo TeamProject { get; }

        /// <summary>
        /// Gets TF Build agent information.
        /// </summary>
        /// <value>
        /// The TF Build agent.
        /// </value>
        /// <para>Via BuildSystem</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"Agent:
        ///         Id: {0}
        ///         Name: {1}",
        ///         BuildSystem.TFBuild.Environment.Agent.Id,
        ///         BuildSystem.TFBuild.Environment.Agent.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TFBuild</para>
        /// <example>
        /// <code>
        /// if (TFBuild.IsRunningOnTFBuild)
        /// {
        ///     Information(
        ///         @"Agent:
        ///         Id: {0}
        ///         Name: {1}",
        ///         TFBuild.Environment.Agent.Id,
        ///         TFBuild.Environment.Agent.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TFBuild");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesAgentInfo Agent { get; }
    }
}
