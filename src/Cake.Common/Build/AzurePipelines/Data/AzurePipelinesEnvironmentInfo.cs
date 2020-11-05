// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.AzurePipelines.Data
{
    /// <summary>
    /// Provides Azure Pipelines environment information for the current build.
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
        /// Gets Azure Pipelines repository information.
        /// </summary>
        /// <value>
        /// The Azure Pipelines repository information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         SourceVersion: {1}
        ///         Shelveset: {2}",
        ///         BuildSystem.AzurePipelines.Environment.Repository.Branch,
        ///         BuildSystem.AzurePipelines.Environment.Repository.SourceVersion,
        ///         BuildSystem.AzurePipelines.Environment.Repository.Shelveset
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Branch: {0}
        ///         SourceVersion: {1}
        ///         Shelveset: {2}",
        ///         AzurePipelines.Environment.Repository.Branch,
        ///         AzurePipelines.Environment.Repository.SourceVersion,
        ///         AzurePipelines.Environment.Repository.Shelveset
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets Azure Pipelines Build Definition information.
        /// </summary>
        /// <value>
        /// The Azure Pipelines Build Definition.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"BuildDefinition:
        ///         Id: {0}
        ///         Name: {1}
        ///         Version: {2}",
        ///         BuildSystem.AzurePipelines.Environment.BuildDefinition.Id,
        ///         BuildSystem.AzurePipelines.Environment.BuildDefinition.Name,
        ///         BuildSystem.AzurePipelines.Environment.BuildDefinition.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AzurePipelines");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"BuildDefinition:
        ///         Id: {0}
        ///         Name: {1}
        ///         Version: {2}",
        ///         AzurePipelines.Environment.BuildDefinition.Id,
        ///         AzurePipelines.Environment.BuildDefinition.Name,
        ///         AzurePipelines.Environment.BuildDefinition.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on AzurePipelines");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesDefinitionInfo BuildDefinition { get; }

        /// <summary>
        /// Gets Azure Pipelines Build information.
        /// </summary>
        /// <value>
        /// The Azure Pipelines Build.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Id: {0}
        ///         Number: {1}
        ///         QueuedBy: {2}",
        ///         BuildSystem.AzurePipelines.Environment.Build.Id,
        ///         BuildSystem.AzurePipelines.Environment.Build.Number,
        ///         BuildSystem.AzurePipelines.Environment.Build.QueuedBy
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Id: {0}
        ///         Number: {1}
        ///         QueuedBy: {2}",
        ///         AzurePipelines.Environment.Build.Id,
        ///         AzurePipelines.Environment.Build.Number,
        ///         AzurePipelines.Environment.Build.QueuedBy
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesBuildInfo Build { get; }

        /// <summary>
        /// Gets Azure Pipelines pull request information.
        /// </summary>
        /// <value>
        /// The Azure Pipelines pull request information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}
        ///         Number: {2}",
        ///         BuildSystem.AzurePipelines.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.AzurePipelines.Environment.PullRequest.Id,
        ///         BuildSystem.AzurePipelines.Environment.PullRequest.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"PullRequest:
        ///         IsPullRequest: {0}
        ///         Id: {1}
        ///         Number: {2}",
        ///         AzurePipelines.Environment.PullRequest.IsPullRequest,
        ///         AzurePipelines.Environment.PullRequest.Id,
        ///         AzurePipelines.Environment.PullRequest.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesPullRequestInfo PullRequest { get; }

        /// <summary>
        /// Gets Azure Pipeline Team Project information.
        /// </summary>
        /// <value>
        /// The Azure Pipelines Team Project.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"TeamProject:
        ///         Id: {0}
        ///         Name: {1}",
        ///         BuildSystem.AzurePipelines.Environment.TeamProject.Id,
        ///         BuildSystem.AzurePipelines.Environment.TeamProject.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"TeamProject:
        ///         Id: {0}
        ///         Name: {1}",
        ///         AzurePipelines.Environment.TeamProject.Id,
        ///         AzurePipelines.Environment.TeamProject.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesTeamProjectInfo TeamProject { get; }

        /// <summary>
        /// Gets Azure Pipelines agent information.
        /// </summary>
        /// <value>
        /// The Azure Pipelines agent.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"Agent:
        ///         Id: {0}
        ///         Name: {1}",
        ///         BuildSystem.AzurePipelines.Environment.Agent.Id,
        ///         BuildSystem.AzurePipelines.Environment.Agent.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        /// <para>Via AzurePipelines.</para>
        /// <example>
        /// <code>
        /// if (AzurePipelines.IsRunningOnAzurePipelines)
        /// {
        ///     Information(
        ///         @"Agent:
        ///         Id: {0}
        ///         Name: {1}",
        ///         AzurePipelines.Environment.Agent.Id,
        ///         AzurePipelines.Environment.Agent.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on Azure Pipelines");
        /// }
        /// </code>
        /// </example>
        public AzurePipelinesAgentInfo Agent { get; }
    }
}
