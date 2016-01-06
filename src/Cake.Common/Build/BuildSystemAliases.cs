using System;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Build.Bamboo;
using Cake.Common.Build.MyGet;
using Cake.Common.Build.TeamCity;
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
            var teamCityProvider = new TeamCityProvider(context.Environment);
            var myGetProvider = new MyGetProvider(context.Environment);
            var bambooProvider = new BambooProvider(context.Environment);
            return new BuildSystem(appVeyorProvider, teamCityProvider, myGetProvider, bambooProvider);
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
    }
}