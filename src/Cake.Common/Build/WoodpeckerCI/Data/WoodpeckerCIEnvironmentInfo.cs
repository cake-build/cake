// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.WoodpeckerCI.Data
{
    /// <summary>
    /// Provides WoodpeckerCI environment information for the current build.
    /// </summary>
    public sealed class WoodpeckerCIEnvironmentInfo : WoodpeckerCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCIEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public WoodpeckerCIEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Repository = new WoodpeckerCIRepositoryInfo(environment);
            Commit = new WoodpeckerCICommitInfo(environment);
            Pipeline = new WoodpeckerCIPipelineInfo(environment);
            Workflow = new WoodpeckerCIWorkflowInfo(environment);
            Step = new WoodpeckerCIStepInfo(environment);
            System = new WoodpeckerCISystemInfo(environment);
            Forge = new WoodpeckerCIForgeInfo(environment);
        }

        /// <summary>
        /// Gets the CI environment name.
        /// </summary>
        /// <value>
        /// The CI environment name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"CI Environment: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.CI
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"CI Environment: {0}",
        ///         WoodpeckerCI.Environment.CI
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public string CI => GetEnvironmentString("CI");

        /// <summary>
        /// Gets the workspace path.
        /// </summary>
        /// <value>
        /// The workspace path.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Workspace: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.Workspace
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Workspace: {0}",
        ///         WoodpeckerCI.Environment.Workspace
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public DirectoryPath Workspace => GetEnvironmentDirectoryPath("CI_WORKSPACE");

        /// <summary>
        /// Gets WoodpeckerCI repository information.
        /// </summary>
        /// <value>
        /// The WoodpeckerCI repository information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Repo: {0}
        ///         Owner: {1}
        ///         Name: {2}",
        ///         BuildSystem.WoodpeckerCI.Environment.Repository.Repo,
        ///         BuildSystem.WoodpeckerCI.Environment.Repository.RepoOwner,
        ///         BuildSystem.WoodpeckerCI.Environment.Repository.RepoName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Repository:
        ///         Repo: {0}
        ///         Owner: {1}
        ///         Name: {2}",
        ///         WoodpeckerCI.Environment.Repository.Repo,
        ///         WoodpeckerCI.Environment.Repository.RepoOwner,
        ///         WoodpeckerCI.Environment.Repository.RepoName
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public WoodpeckerCIRepositoryInfo Repository { get; }

        /// <summary>
        /// Gets WoodpeckerCI commit information.
        /// </summary>
        /// <value>
        /// The WoodpeckerCI commit information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Commit:
        ///         SHA: {0}
        ///         Branch: {1}
        ///         Message: {2}",
        ///         BuildSystem.WoodpeckerCI.Environment.Commit.Sha,
        ///         BuildSystem.WoodpeckerCI.Environment.Commit.Branch,
        ///         BuildSystem.WoodpeckerCI.Environment.Commit.Message
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Commit:
        ///         SHA: {0}
        ///         Branch: {1}
        ///         Message: {2}",
        ///         WoodpeckerCI.Environment.Commit.Sha,
        ///         WoodpeckerCI.Environment.Commit.Branch,
        ///         WoodpeckerCI.Environment.Commit.Message
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public WoodpeckerCICommitInfo Commit { get; }

        /// <summary>
        /// Gets WoodpeckerCI pipeline information.
        /// </summary>
        /// <value>
        /// The WoodpeckerCI pipeline information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Pipeline:
        ///         Number: {0}
        ///         Event: {1}",
        ///         BuildSystem.WoodpeckerCI.Environment.Pipeline.Number,
        ///         BuildSystem.WoodpeckerCI.Environment.Pipeline.Event
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Pipeline:
        ///         Number: {0}
        ///         Event: {1}",
        ///         WoodpeckerCI.Environment.Pipeline.Number,
        ///         WoodpeckerCI.Environment.Pipeline.Event
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public WoodpeckerCIPipelineInfo Pipeline { get; }

        /// <summary>
        /// Gets WoodpeckerCI workflow information.
        /// </summary>
        /// <value>
        /// The WoodpeckerCI workflow information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Workflow: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.Workflow.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Workflow: {0}",
        ///         WoodpeckerCI.Environment.Workflow.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public WoodpeckerCIWorkflowInfo Workflow { get; }

        /// <summary>
        /// Gets WoodpeckerCI step information.
        /// </summary>
        /// <value>
        /// The WoodpeckerCI step information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Step:
        ///         Name: {0}
        ///         Number: {1}",
        ///         BuildSystem.WoodpeckerCI.Environment.Step.Name,
        ///         BuildSystem.WoodpeckerCI.Environment.Step.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Step:
        ///         Name: {0}
        ///         Number: {1}",
        ///         WoodpeckerCI.Environment.Step.Name,
        ///         WoodpeckerCI.Environment.Step.Number
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public WoodpeckerCIStepInfo Step { get; }

        /// <summary>
        /// Gets WoodpeckerCI system information.
        /// </summary>
        /// <value>
        /// The WoodpeckerCI system information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"System:
        ///         Name: {0}
        ///         Version: {1}",
        ///         BuildSystem.WoodpeckerCI.Environment.System.Name,
        ///         BuildSystem.WoodpeckerCI.Environment.System.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"System:
        ///         Name: {0}
        ///         Version: {1}",
        ///         WoodpeckerCI.Environment.System.Name,
        ///         WoodpeckerCI.Environment.System.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public WoodpeckerCISystemInfo System { get; }

        /// <summary>
        /// Gets WoodpeckerCI forge information.
        /// </summary>
        /// <value>
        /// The WoodpeckerCI forge information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Forge:
        ///         Type: {0}
        ///         URL: {1}",
        ///         BuildSystem.WoodpeckerCI.Environment.Forge.Type,
        ///         BuildSystem.WoodpeckerCI.Environment.Forge.Url
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Forge:
        ///         Type: {0}
        ///         URL: {1}",
        ///         WoodpeckerCI.Environment.Forge.Type,
        ///         WoodpeckerCI.Environment.Forge.Url
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public WoodpeckerCIForgeInfo Forge { get; }

        private DirectoryPath GetEnvironmentDirectoryPath(string variable)
        {
            var value = GetEnvironmentString(variable);
            return !string.IsNullOrWhiteSpace(value) ? new DirectoryPath(value) : null;
        }
    }
}
