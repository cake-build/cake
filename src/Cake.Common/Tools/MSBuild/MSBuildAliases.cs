using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Contains functionality related to MSBuild.
    /// </summary>
    [CakeAliasCategory("MSBuild")]
    public static class MSBuildAliases
    {
        /// <summary>
        /// Builds the specified solution using MSBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        [CakeMethodAlias]
        public static void MSBuild(this ICakeContext context, FilePath solution)
        {
            MSBuild(context, solution, settings => { });
        }

        /// <summary>
        /// Builds the specified solution using MSBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution to build.</param>
        /// <param name="configurator">The settings configurator.</param>
        [CakeMethodAlias]
        public static void MSBuild(this ICakeContext context, FilePath solution, Action<MSBuildSettings> configurator)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (configurator == null)
            {
                throw new ArgumentNullException("configurator");
            }

            var settings = new MSBuildSettings();
            configurator(settings);

            // Perform the build.
            MSBuild(context, solution, settings);
        }

        /// <summary>
        /// Builds the specified solution using MSBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution to build.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void MSBuild(this ICakeContext context, FilePath solution, MSBuildSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var runner = new MSBuildRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(solution, settings);
        }
    }
}