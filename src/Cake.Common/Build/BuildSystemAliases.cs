// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.AzurePipelines;
using Cake.Common.Build.Bamboo;
using Cake.Common.Build.BitbucketPipelines;
using Cake.Common.Build.Bitrise;
using Cake.Common.Build.ContinuaCI;
using Cake.Common.Build.GitHubActions;
using Cake.Common.Build.GitLabCI;
using Cake.Common.Build.GoCD;
using Cake.Common.Build.Jenkins;
using Cake.Common.Build.MyGet;
using Cake.Common.Build.TeamCity;
using Cake.Common.Build.TravisCI;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Build
{
    /// <summary>
    /// Contains functionality related to build systems.
    /// </summary>
    [CakeAliasCategory("Build System")]
    public static class BuildSystemAliases
    {
        /// <summary>
        /// Gets a <see cref="Build.BuildSystem"/> instance that can
        /// be used to query for information about the current build system.
        /// </summary>
        /// <example>
        /// <code>
        /// var isLocal = BuildSystem.IsLocalBuild;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.BuildSystem"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        public static BuildSystem BuildSystem(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var appVeyorProvider = new AppVeyorProvider(context.Environment, context.ProcessRunner, context.Log);
            var teamCityProvider = new TeamCityProvider(context.Environment, context.FileSystem, new BuildSystemServiceMessageWriter());
            var myGetProvider = new MyGetProvider(context.Environment, new BuildSystemServiceMessageWriter());
            var bambooProvider = new BambooProvider(context.Environment);
            var continuaCIProvider = new ContinuaCIProvider(context.Environment, new BuildSystemServiceMessageWriter());
            var jenkinsProvider = new JenkinsProvider(context.Environment);
            var bitriseProvider = new BitriseProvider(context.Environment, context.ProcessRunner);
            var travisCIProvider = new TravisCIProvider(context.Environment, new BuildSystemServiceMessageWriter());
            var bitbucketPipelinesProvider = new BitbucketPipelinesProvider(context.Environment);
            var goCDProvider = new GoCDProvider(context.Environment, context.Log);
            var gitLabCIProvider = new GitLabCIProvider(context.Environment);
            var gitHubActionsProvider = new GitHubActionsProvider(context.Environment, context.FileSystem);
            var azurePipelinesProvider = new AzurePipelinesProvider(context.Environment, new BuildSystemServiceMessageWriter());

            return new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider, bitbucketPipelinesProvider, goCDProvider, gitLabCIProvider, gitHubActionsProvider, azurePipelinesProvider);
        }

        /// <summary>
        /// Gets a <see cref="AppVeyorProvider"/> instance that can
        /// be used to manipulate the AppVeyor environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isAppVeyorBuild = AppVeyor.IsRunningOnAppVeyor;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.AppVeyor"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.AppVeyor")]
        [CakeNamespaceImport("Cake.Common.Build.AppVeyor.Data")]
        public static IAppVeyorProvider AppVeyor(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.AppVeyor;
        }

        /// <summary>
        /// Gets a <see cref="TeamCityProvider"/> instance that can
        /// be used to manipulate the TeamCity environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isTeamCityBuild = TeamCity.IsRunningOnTeamCity;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.TeamCity"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.TeamCity")]
        public static ITeamCityProvider TeamCity(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.TeamCity;
        }

        /// <summary>
        /// Gets a <see cref="MyGetProvider"/> instance that can
        /// be used to manipulate the MyGet environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isMyGetBuild = MyGet.IsRunningOnMyGet;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.MyGet"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.MyGet")]
        public static IMyGetProvider MyGet(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.MyGet;
        }

        /// <summary>
        /// Gets a <see cref="BambooProvider"/> instance that can
        /// be used to manipulate the Bamboo environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isBambooBuild = Bamboo.IsRunningOnBamboo;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.Bamboo"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.Bamboo")]
        [CakeNamespaceImport("Cake.Common.Build.Bamboo.Data")]
        public static IBambooProvider Bamboo(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.Bamboo;
        }

        /// <summary>
        /// Gets a <see cref="ContinuaCIProvider"/> instance that can
        /// be used to manipulate the Continua CI environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isContinuaCIBuild = ContinuaCI.IsRunningContinuaCI;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.ContinuaCI"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.ContinuaCI")]
        [CakeNamespaceImport("Cake.Common.Build.ContinuaCI.Data")]
        public static IContinuaCIProvider ContinuaCI(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.ContinuaCI;
        }

        /// <summary>
        /// Gets a <see cref="JenkinsProvider"/> instance that can be used to
        /// obtain information from the Jenkins environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isJenkinsBuild = Jenkins.IsRunningOnJenkins;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.Jenkins"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.Jenkins")]
        [CakeNamespaceImport("Cake.Common.Build.Jenkins.Data")]
        public static IJenkinsProvider Jenkins(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.Jenkins;
        }

        /// <summary>
        /// Gets a <see cref="BitriseProvider"/> instance that can be used to
        /// obtain information from the Bitrise environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isBitriseBuild = Bitrise.IsRunningOnBitrise;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.Bitrise"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.Bitrise")]
        [CakeNamespaceImport("Cake.Common.Build.Bitrise.Data")]
        public static IBitriseProvider Bitrise(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.Bitrise;
        }

        /// <summary>
        /// Gets a <see cref="TravisCIProvider"/> instance that can be used to
        /// obtain information from the Travis CI environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isTravisCIBuild = TravisCI.IsRunningOnTravisCI;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.TravisCI"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.TravisCI")]
        [CakeNamespaceImport("Cake.Common.Build.TravisCI.Data")]
        public static ITravisCIProvider TravisCI(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.TravisCI;
        }

        /// <summary>
        /// Gets a <see cref="BitbucketPipelinesProvider"/> instance that can be used to
        /// obtain information from the Bitbucket Pipelines environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isBitbucketPipelinesBuild = BitbucketPipelines.IsRunningOnBitbucketPipelines;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.BitbucketPipelines"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.BitbucketPipelines")]
        [CakeNamespaceImport("Cake.Common.Build.BitbucketPipelines.Data")]
        public static IBitbucketPipelinesProvider BitbucketPipelines(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.BitbucketPipelines;
        }

        /// <summary>
        /// Gets a <see cref="GoCDProvider"/> instance that can be used to
        /// obtain information from the Go.CD environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isGoCDBuild = GoCD.IsRunningOnGoCD;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.GoCD"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.GoCD")]
        [CakeNamespaceImport("Cake.Common.Build.GoCD.Data")]
        public static IGoCDProvider GoCD(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.GoCD;
        }

        /// <summary>
        /// Gets a <see cref="GitLabCIProvider"/> instance that can be used to
        /// obtain information from the GitLab CI environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isGitLabCIBuild = GitLabCI.IsRunningOnGitLabCI;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.GitLabCI"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.GitLabCI")]
        [CakeNamespaceImport("Cake.Common.Build.GitLabCI.Data")]
        public static IGitLabCIProvider GitLabCI(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.GitLabCI;
        }

        /// <summary>
        /// Gets a <see cref="GitHubActionsProvider"/> instance that can be used to
        /// obtain information from the GitHub Actions environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isGitHubActionsBuild = GitHubActions.IsRunningOnGitHubActions;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.GitHubActions"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.GitHubActions")]
        [CakeNamespaceImport("Cake.Common.Build.GitHubActions.Data")]
        public static IGitHubActionsProvider GitHubActions(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.GitHubActions;
        }

        /// <summary>
        /// Gets a <see cref="AzurePipelinesProvider"/> instance that can be used to
        /// obtain information from the Azure Pipelines environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isAzurePipelines = AzurePipelines.IsRunningOnAzurePipelines;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Build.AzurePipelines"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.AzurePipelines")]
        [CakeNamespaceImport("Cake.Common.Build.AzurePipelines.Data")]
        public static IAzurePipelinesProvider AzurePipelines(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var buildSystem = context.BuildSystem();
            return buildSystem.AzurePipelines;
        }
    }
}