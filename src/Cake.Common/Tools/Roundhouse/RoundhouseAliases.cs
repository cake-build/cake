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
        public static void Roundhouse(this ICakeContext context, RoundhouseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new RoundhouseRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Globber);
            runner.Run(settings);
        }
    }
}