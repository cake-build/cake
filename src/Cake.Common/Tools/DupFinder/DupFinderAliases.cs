// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.DupFinder
{
    /// <summary>
    /// <para>Contains functionality related to ReSharper's <see href="https://www.jetbrains.com/help/resharper/2016.1/dupFinder.html">dupFinder</see> tool.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the <see cref="DupFinderSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=JetBrains.ReSharper.CommandLineTools"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("ReSharper")]
    public static class DupFinderAliases
    {
        /// <summary>
        /// Analyses the specified file with ReSharper's DupFinder.
        /// The file can either be a solution/project or a source file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to analyze.</param>
        /// <example>
        /// <code>
        /// DupFinder("./src/MySolution.sln");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, FilePath file)
        {
            DupFinder(context, new[] { file });
        }

        /// <summary>
        /// Analyses the specified file with ReSharper's DupFinder using the specified settings.
        /// The file can either be a solution/project or a source file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="file">The file to analyze.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var buildOutputDirectory = Directory("./.build");
        /// var resharperReportsDirectory = buildOutputDirectory + Directory("_ReSharperReports");
        /// var rootDirectoryPath = MakeAbsolute(Context.Environment.WorkingDirectory);
        ///
        /// DupFinder("./src/MySolution.sln", new DupFinderSettings {
        ///     ShowStats = true,
        ///     ShowText = true,
        ///     ExcludePattern = new String[]
        ///     {
        ///         rootDirectoryPath + "/**/*Designer.cs",
        ///     },
        ///     OutputFile = resharperReportsDirectory + File("dupfinder-output.xml"),
        ///     ThrowExceptionOnFindingDuplicates = true
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, FilePath file, DupFinderSettings settings)
        {
            DupFinder(context, new[] { file }, settings);
        }

        /// <summary>
        /// Analyses the specified projects with ReSharper's DupFinder.
        /// The files can either be solutions and projects or a source files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="files">The files to analyze.</param>
        /// <example>
        /// <code>
        /// var projects = GetFiles("./src/**/*.csproj");
        /// DupFinder(projects);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, IEnumerable<FilePath> files)
        {
            DupFinder(context, files, new DupFinderSettings());
        }

        /// <summary>
        /// Analyses the specified projects with ReSharper's DupFinder using the specified settings.
        /// The files can either be solutions and projects or a source files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="files">The files to analyze.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var buildOutputDirectory = Directory("./.build");
        /// var resharperReportsDirectory = buildOutputDirectory + Directory("_ReSharperReports");
        /// var rootDirectoryPath = MakeAbsolute(Context.Environment.WorkingDirectory);
        ///
        /// var projects = GetFiles("./src/**/*.csproj");
        /// DupFinder(projects, new DupFinderSettings {
        ///     ShowStats = true,
        ///     ShowText = true,
        ///     ExcludePattern = new String[]
        ///     {
        ///         rootDirectoryPath + "/**/*Designer.cs",
        ///     },
        ///     OutputFile = resharperReportsDirectory + File("dupfinder-output.xml"),
        ///     ThrowExceptionOnFindingDuplicates = true
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, IEnumerable<FilePath> files, DupFinderSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new DupFinderRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);
            runner.Run(files, settings);
        }

        /// <summary>
        /// Analyses all files matching the specified pattern with ReSharper's DupFinder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <example>
        /// <code>
        /// DupFinder("*.cs");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, GlobPattern pattern)
        {
            DupFinder(context, pattern, new DupFinderSettings());
        }

        /// <summary>
        /// Analyses all files matching the specified pattern with ReSharper's DupFinder,
        /// using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// var buildOutputDirectory = Directory("./.build");
        /// var resharperReportsDirectory = buildOutputDirectory + Directory("_ReSharperReports");
        ///
        /// DupFinder("*.cs", new DupFinderSettings {
        ///     OutputFile = resharperReportsDirectory + File("dupfinder-output.xml"),
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinder(this ICakeContext context, GlobPattern pattern, DupFinderSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var sourceFiles = context.Globber.GetFiles(pattern).ToArray();
            if (sourceFiles.Length == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }

            var runner = new DupFinderRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);
            runner.Run(sourceFiles, settings);
        }

        /// <summary>
        /// Runs ReSharper's DupFinder using the provided config file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="configFile">The config file.</param>
        /// <example>
        /// <code>
        /// DupFinderFromConfig("./src/dupfinder.config");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("DupFinder")]
        public static void DupFinderFromConfig(this ICakeContext context, FilePath configFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new DupFinderRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools, context.Log);
            runner.RunFromConfig(configFile);
        }
    }
}