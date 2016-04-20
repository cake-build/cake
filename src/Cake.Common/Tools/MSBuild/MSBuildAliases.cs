﻿using System;
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
        /// <example>
        /// <code>
        /// MSBuild("./src/Cake.sln");
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// MSBuild("./src/Cake.sln", configurator =>
        ///     configurator.SetConfiguration("Debug")
        ///         .SetVerbosity(Verbosity.Minimal)
        ///         .UseToolVersion(MSBuildToolVersion.VS2015)
        ///         .SetMSBuildPlatform(MSBuildPlatform.x86)
        ///         .SetPlatformTarget(PlatformTarget.MSIL));
        /// </code>
        /// </example>
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
        /// <example>
        /// <code>
        /// MSBuild("./src/Cake.sln", new MSBuildSettings {
        ///     Verbosity = Verbosity.Minimal,
        ///     ToolVersion = MSBuildToolVersion.VS2015,
        ///     Configuration = "Release",
        ///     PlatformTarget = PlatformTarget.MSIL
        ///     });
        /// </code>
        /// </example>
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