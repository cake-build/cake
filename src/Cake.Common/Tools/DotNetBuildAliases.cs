// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Tools.MSBuild;
using Cake.Common.Tools.XBuild;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools
{
    /// <summary>
    /// Contains functionality to run either MSBuild on Windows or XBuild on Mac/Linux/Unix.
    /// </summary>
    [CakeAliasCategory("DotNetBuild")]
    public static class DotNetBuildAliases
    {
        /// <summary>
        /// Builds the specified solution using MSBuild or XBuild.
        /// </summary>
        /// <example>
        /// <code>
        /// DotNetBuild("./project/project.sln");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        [CakeMethodAlias]
        public static void DotNetBuild(this ICakeContext context, FilePath solution)
        {
            DotNetBuild(context, solution, settings => { });
        }

        /// <summary>
        /// Builds the specified solution using MSBuild or XBuild.
        /// </summary>
        /// <example>
        /// <code>
        /// DotNetBuild("./project/project.sln", settings =>
        ///     settings.SetConfiguration("Debug")
        ///         .SetVerbosity(Core.Diagnostics.Verbosity.Minimal)
        ///         .WithTarget("Build")
        ///         .WithProperty("TreatWarningsAsErrors","true")
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="solution">The solution.</param>
        /// <param name="configurator">The configurator.</param>
        [CakeMethodAlias]
        public static void DotNetBuild(this ICakeContext context, FilePath solution, Action<DotNetBuildSettings> configurator)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (configurator == null)
            {
                throw new ArgumentNullException("configurator");
            }

            // Create the settings using the delegate.
            var dotNetSettings = new DotNetBuildSettings(solution);
            configurator(dotNetSettings);

            // Running on Mac/Linux/Unix?
            if (context.Environment.IsUnix())
            {
                // Use XBuild.
                XBuildAliases.XBuild(context, solution, settings =>
                {
                    settings.Configuration = dotNetSettings.Configuration;
                    settings.Verbosity = dotNetSettings.Verbosity;

                    foreach (var target in dotNetSettings.Targets)
                    {
                        settings.Targets.Add(target);
                    }

                    foreach (var property in dotNetSettings.Properties)
                    {
                        settings.Properties.Add(property);
                    }
                });
            }
            else
            {
                // Use MSBuild.
                MSBuildAliases.MSBuild(context, solution, settings =>
                {
                    settings.Configuration = dotNetSettings.Configuration;
                    settings.Verbosity = dotNetSettings.Verbosity;

                    foreach (var target in dotNetSettings.Targets)
                    {
                        settings.Targets.Add(target);
                    }

                    foreach (var property in dotNetSettings.Properties)
                    {
                        settings.Properties.Add(property);
                    }
                });
            }
        }
    }
}
