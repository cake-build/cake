// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.XBuild
{
    /// <summary>
    /// <para>Contains functionality related to <see href="http://www.mono-project.com/docs/tools+libraries/tools/xbuild/">XBuild</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, XBuild (which is part of Mono) will already have to be installed on the machine the
    /// Cake Script is being executed.
    /// </para>
    /// </summary>
    [CakeAliasCategory("XBuild")]
    public static class XBuildAliases
    {
        /// <summary>
        /// Builds the specified solution using XBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution to build.</param>
        [CakeMethodAlias]
        public static void XBuild(this ICakeContext context, FilePath solution)
        {
            XBuild(context, solution, settings => { });
        }

        /// <summary>
        /// Builds the specified solution using XBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution to build.</param>
        /// <param name="configurator">The settings configurator.</param>
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

            var settings = new XBuildSettings();
            configurator(settings);

            // Perform the build.
            XBuild(context, solution, settings);
        }

        /// <summary>
        /// Builds the specified solution using XBuild.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution to build.</param>
        /// <param name="settings">The settings.</param>
        [CakeMethodAlias]
        public static void XBuild(this ICakeContext context, FilePath solution, XBuildSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var runner = new XBuildRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(solution, settings);
        }
    }
}
