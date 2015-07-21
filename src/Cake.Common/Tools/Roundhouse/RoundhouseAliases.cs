using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.Roundhouse
{
    /// <summary>
    /// Contains functionality to execute Roundhouse tasks.
    /// </summary>
    [CakeAliasCategory("Roundhouse")]
    public static class RoundhouseAliases
    {
        /// <summary>
        /// Executes Roundhouse with the given configured settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Migrate(this ICakeContext context, RoundhouseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new RoundhouseRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(settings);
        }

        /// <summary>
        /// Executes Roundhouse migration to drop the database using the provided settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void Drop(this ICakeContext context, RoundhouseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new RoundhouseRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(settings, true);
        }
    }
}