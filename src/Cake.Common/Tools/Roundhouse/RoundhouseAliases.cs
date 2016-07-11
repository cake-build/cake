// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common.Tools.Roundhouse
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/chucknorris/roundhouse">RoundhousE</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="RoundhouseSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=roundhouse"
    /// </code>
    /// </para>
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
        public static void RoundhouseMigrate(this ICakeContext context, RoundhouseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new RoundhouseRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings);
        }

        /// <summary>
        /// Executes Roundhouse migration to drop the database using the provided settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void RoundhouseDrop(this ICakeContext context, RoundhouseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var runner = new RoundhouseRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(settings, true);
        }
    }
}
