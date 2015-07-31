using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.InspectCode
{
    /// <summary>
    /// Contains functionality related to Resharper's code inspection.
    /// </summary>
    [CakeAliasCategory("ReSharper")]
    public static class InspectCodeAliases
    {
        /// <summary>
        /// Analyses the specified solution with Resharper's InspectCode.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("InspectCode")]
        public static void InspectCode(this ICakeContext context, FilePath solution)
        {
            InspectCode(context, solution, new InspectCodeSettings());
        }

        /// <summary>
        /// Analyses the specified solution with Resharper's InspectCode,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("InspectCode")]
        public static void InspectCode(this ICakeContext context, FilePath solution, InspectCodeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new InspectCodeRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(solution, settings);
        }

        /// <summary>
        /// Runs ReSharper's InspectCode using the specified config file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configFile">The config file.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("InspectCode")]
        public static void InspectCodeFromConfig(this ICakeContext context, FilePath configFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new InspectCodeRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.RunFromConfig(configFile);
        }
    }
}