// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.OpenCover
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/opencover/opencover">OpenCover</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="OpenCoverSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=OpenCover"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("OpenCover")]
    public static class OpenCoverAliases
    {
        /// <summary>
        /// Runs <see href="https://github.com/OpenCover/opencover">OpenCover</see>
        /// for the specified action and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action to run OpenCover for.</param>
        /// <param name="outputFile">The OpenCover output file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// OpenCover(tool => {
        ///   tool.XUnit2("./**/App.Tests.dll",
        ///     new XUnit2Settings {
        ///       ShadowCopy = false
        ///     });
        ///   },
        ///   new FilePath("./result.xml"),
        ///   new OpenCoverSettings()
        ///     .WithFilter("+[App]*")
        ///     .WithFilter("-[App.Tests]*"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void OpenCover(
            this ICakeContext context,
            Action<ICakeContext> action,
            FilePath outputFile,
            OpenCoverSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            // Create the OpenCover runner.
            var runner = new OpenCoverRunner(
                context.FileSystem, context.Environment,
                context.ProcessRunner, context.Tools);

            // Run OpenCover.
            runner.Run(context, action, outputFile, settings);
        }
    }
}
