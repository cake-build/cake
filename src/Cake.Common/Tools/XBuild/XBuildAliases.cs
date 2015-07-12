using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.XBuild
{
    /// <summary>
    /// Contains functionality related to MSBuild.
    /// </summary>
    [CakeAliasCategory("XBuild")]
    public static class XBuildAliases
    {
        /// <summary>
        /// Builds the specified solution using MSBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        [CakeMethodAlias]
        public static void XBuild(this ICakeContext context, FilePath solution)
        {
            XBuild(context, solution, settings => { });
        }

        /// <summary>
        /// Builds the specified solution using MSBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        /// <param name="configurator">The configurator.</param>
        [CakeMethodAlias]
        public static void XBuild(this ICakeContext context, FilePath solution, Action<XBuildSettings> configurator)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (configurator == null)
            {
                throw new ArgumentNullException("configurator");
            }

            var settings = new XBuildSettings(solution);
            configurator(settings);

            var runner = new XBuildRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(settings);
        }
    }
}
