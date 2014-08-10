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
    public static class MSBuildExtensions
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
        /// <param name="solution">The solution.</param>
        /// <param name="configurator">The configurator.</param>
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

            var settings = new MSBuildSettings(solution);
            configurator(settings);

            var runner = new MSBuildRunner(context.FileSystem, context.Environment, context.ProcessRunner);
            runner.Run(settings);
        }
    }
}
