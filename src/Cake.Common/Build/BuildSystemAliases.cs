using System;
using Cake.Common.Build.AppVeyor;
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
        /// Gets a <see cref="BuildSystem"/> instance that can
        /// be used to query for information about the current build system.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [CakePropertyAlias(Cache = true)]
        public static BuildSystem BuildSystem(this ICakeContext context)
        {
            return new BuildSystem(AppVeyor(context));
        }

        /// <summary>
        /// Gets a <see cref="AppVeyor"/> instance that can
        /// be used to manipulate the AppVeyor environment.
        /// </summary>
        /// <param name="context"></param>
        /// <returns></returns>
        [CakePropertyAlias(Cache = true)]
        public static AppVeyorProvider AppVeyor(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return new AppVeyorProvider(context.Environment, context.ProcessRunner);
        }
    }
}
