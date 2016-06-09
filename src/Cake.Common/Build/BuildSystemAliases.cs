// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.Bamboo;
using Cake.Common.Build.Bitrise;
using Cake.Common.Build.ContinuaCI;
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
        /// Gets a <see cref="Cake.Common.Build.BuildSystem"/> instance that can
        /// be used to query for information about the current build system.
        /// </summary>
        /// <example>
        /// <code>
        /// var isLocal = BuildSystem.IsLocalBuild;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.BuildSystem"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        public static BuildSystem BuildSystem(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var appVeyorProvider = new AppVeyorProvider(context.Environment, context.ProcessRunner);
            var teamCityProvider = new TeamCityProvider(context.Environment, context.Log);
            var myGetProvider = new MyGetProvider(context.Environment);
            var bambooProvider = new BambooProvider(context.Environment);
            var continuaCIProvider = new ContinuaCIProvider(context.Environment);
            var jenkinsProvider = new JenkinsProvider(context.Environment);
            var bitriseProvider = new BitriseProvider(context.Environment);
            var travisCIProvider = new TravisCIProvider(context.Environment, context.Log);

            return new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider, continuaCIProvider, jenkinsProvider, bitriseProvider, travisCIProvider);
        }

        /// <summary>
        /// Gets a <see cref="Cake.Common.Build.AppVeyor.AppVeyorProvider"/> instance that can
        /// be used to manipulate the AppVeyor environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isAppVeyorBuild = AppVeyor.IsRunningOnAppVeyor;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.AppVeyor"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.AppVeyor")]
        [CakeNamespaceImport("Cake.Common.Build.AppVeyor.Data")]
        public static IAppVeyorProvider AppVeyor(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var buildSystem = context.BuildSystem();
            return buildSystem.AppVeyor;
        }

        /// <summary>
        /// Gets a <see cref="Cake.Common.Build.TeamCity.TeamCityProvider"/> instance that can
        /// be used to manipulate the TeamCity environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isTeamCityBuild = TeamCity.IsRunningOnTeamCity;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.TeamCity"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        public static ITeamCityProvider TeamCity(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var buildSystem = context.BuildSystem();
            return buildSystem.TeamCity;
        }

        /// <summary>
        /// Gets a <see cref="Cake.Common.Build.MyGet.MyGetProvider"/> instance that can
        /// be used to manipulate the MyGet environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isMyGetBuild = MyGet.IsRunningOnMyGet;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.MyGet"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        public static IMyGetProvider MyGet(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var buildSystem = context.BuildSystem();
            return buildSystem.MyGet;
        }

        /// <summary>
        /// Gets a <see cref="Cake.Common.Build.Bamboo.BambooProvider"/> instance that can
        /// be used to manipulate the Bamboo environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isBambooBuild = Bamboo.IsRunningBamboo;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.Bamboo"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        public static IBambooProvider Bamboo(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var buildSystem = context.BuildSystem();
            return buildSystem.Bamboo;
        }

        /// <summary>
        /// Gets a <see cref="Cake.Common.Build.ContinuaCI.ContinuaCIProvider"/> instance that can
        /// be used to manipulate the Continua CI environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isContinuaCIBuild = ContinuaCI.IsRunningContinuaCI;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.ContinuaCI"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.ContinuaCI")]
        [CakeNamespaceImport("Cake.Common.Build.ContinuaCI.Data")]
        public static IContinuaCIProvider ContinuaCI(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var buildSystem = context.BuildSystem();
            return buildSystem.ContinuaCI;
        }

        /// <summary>
        /// Get a <see cref="JenkinsProvider"/> instance that can be user to
        /// obtain information from the Jenkins environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isJenkinsBuild = Jenkins.IsRunningOnJenkins;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.Jenkins"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.Jenkins")]
        [CakeNamespaceImport("Cake.Common.Build.Jenkins.Data")]
        public static IJenkinsProvider Jenkins(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var buildSystem = context.BuildSystem();
            return buildSystem.Jenkins;
        }

        /// <summary>
        /// Get a <see cref="BitriseProvider"/> instance that can be user to
        /// obtain information from the Bitrise environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isBitriseBuild = Bitrise.IsRunningOnBitrise;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.Bitrise"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.Bitrise")]
        [CakeNamespaceImport("Cake.Common.Build.Bitrise.Data")]
        public static IBitriseProvider Bitrise(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var buildSystem = context.BuildSystem();
            return buildSystem.Bitrise;
        }

        /// <summary>
        /// Get a <see cref="TravisCIProvider"/> instance that can be user to
        /// obtain information from the Travis CI environment.
        /// </summary>
        /// <example>
        /// <code>
        /// var isTravisCIBuild = TravisCI.IsRunningOnTravisCI;
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>A <see cref="Cake.Common.Build.TravisCI"/> instance.</returns>
        [CakePropertyAlias(Cache = true)]
        [CakeNamespaceImport("Cake.Common.Build.TravisCI")]
        [CakeNamespaceImport("Cake.Common.Build.TravisCI.Data")]
        public static ITravisCIProvider TravisCI(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            var buildSystem = context.BuildSystem();
            return buildSystem.TravisCI;
        }
    }
}
