// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotCover.Analyse;
using Cake.Common.Tools.DotCover.Cover;
using Cake.Common.Tools.DotCover.Merge;
using Cake.Common.Tools.DotCover.Report;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://www.jetbrains.com/dotcover/">DotCover</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the appropriate settings class:
    /// <code>
    /// #tool "nuget:?package=JetBrains.dotCover.CommandLineTools"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("DotCover")]
    public static class DotCoverAliases
    {
        /// <summary>
        /// Runs <see href="https://www.jetbrains.com/dotcover/help/dotCover__Console_Runner_Commands.html#analyse">DotCover Analyse</see>
        /// for the specified action and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action to run DotCover for.</param>
        /// <param name="outputFile">The DotCover output file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotCoverAnalyse(tool => {
        ///   tool.XUnit2("./**/App.Tests.dll",
        ///     new XUnit2Settings {
        ///       ShadowCopy = false
        ///     });
        ///   },
        ///   new FilePath("./result.xml"),
        ///   new DotCoverAnalyseSettings()
        ///     .WithFilter("+:App")
        ///     .WithFilter("-:App.Tests"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Analyse")]
        [CakeNamespaceImport("Cake.Common.Tools.DotCover.Analyse")]
        public static void DotCoverAnalyse(
            this ICakeContext context,
            Action<ICakeContext> action,
            FilePath outputFile,
            DotCoverAnalyseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotCoverAnalyseSettings();
            }

            // Create the DotCover analyser.
            var analyser = new DotCoverAnalyser(
                context.FileSystem, context.Environment,
                context.ProcessRunner, context.Tools);

            // Run DotCover analyse.
            analyser.Analyse(context, action, outputFile, settings);
        }

        /// <summary>
        /// Runs <see href="https://www.jetbrains.com/dotcover/help/dotCover__Console_Runner_Commands.html#cover">DotCover Cover</see>
        /// for the specified action and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action to run DotCover for.</param>
        /// <param name="outputFile">The DotCover output file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotCoverCover(tool => {
        ///   tool.XUnit2("./**/App.Tests.dll",
        ///     new XUnit2Settings {
        ///       ShadowCopy = false
        ///     });
        ///   },
        ///   new FilePath("./result.dcvr"),
        ///   new DotCoverCoverSettings()
        ///     .WithFilter("+:App")
        ///     .WithFilter("-:App.Tests"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Cover")]
        [CakeNamespaceImport("Cake.Common.Tools.DotCover.Cover")]
        public static void DotCoverCover(
            this ICakeContext context,
            Action<ICakeContext> action,
            FilePath outputFile,
            DotCoverCoverSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            if (settings == null)
            {
                settings = new DotCoverCoverSettings();
            }

            // Create the DotCover coverer.
            var coverer = new DotCoverCoverer(
                context.FileSystem, context.Environment,
                context.ProcessRunner, context.Tools);

            // Run DotCover cover.
            coverer.Cover(context, action, outputFile, settings);
        }

        /// <summary>
        /// Runs <see href="https://www.jetbrains.com/dotcover/help/dotCover__Console_Runner_Commands.html#report">DotCover Report</see>
        /// for the specified action and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourceFile">The DotCover coverage snapshot file name.</param>
        /// <param name="outputFile">The DotCover output file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotCoverReport(new FilePath("./result.dcvr"),
        ///   new FilePath("./result.html"),
        ///   new DotCoverReportSettings {
        ///     ReportType = DotCoverReportType.HTML
        ///   });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Report")]
        [CakeNamespaceImport("Cake.Common.Tools.DotCover.Report")]
        public static void DotCoverReport(
            this ICakeContext context,
            FilePath sourceFile,
            FilePath outputFile,
            DotCoverReportSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (settings == null)
            {
                settings = new DotCoverReportSettings();
            }

            // Create the DotCover reporter.
            var reporter = new DotCoverReporter(
                context.FileSystem, context.Environment,
                context.ProcessRunner, context.Tools);

            // Run DotCover report.
            reporter.Report(sourceFile, outputFile, settings);
        }

        /// <summary>
        /// Runs <see href="https://www.jetbrains.com/dotcover/help/dotCover__Console_Runner_Commands.html#merge">DotCover Merge</see>
        /// for the specified action and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourceFiles">The list of DotCover coverage snapshot files.</param>
        /// <param name="outputFile">The merged output file.</param>
        /// <example>
        /// <code>
        /// DotCoverMerge(new[] {
        ///     new FilePath("./result1.dcvr"),
        ///     new FilePath("./result2.dcvr")
        ///   },
        ///   new FilePath("./merged.dcvr"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Merge")]
        [CakeNamespaceImport("Cake.Common.Tools.DotCover.Merge")]
        public static void DotCoverMerge(
            this ICakeContext context,
            IEnumerable<FilePath> sourceFiles,
            FilePath outputFile)
        {
            DotCoverMerge(context, sourceFiles, outputFile, new DotCoverMergeSettings());
        }

        /// <summary>
        /// Runs <see href="https://www.jetbrains.com/dotcover/help/dotCover__Console_Runner_Commands.html#merge">DotCover Merge</see>
        /// for the specified action and settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="sourceFiles">The list of DotCover coverage snapshot files.</param>
        /// <param name="outputFile">The merged output file.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// DotCoverMerge(new[] {
        ///     new FilePath("./result1.dcvr"),
        ///     new FilePath("./result2.dcvr")
        ///   },
        ///   new FilePath("./merged.dcvr"),
        ///   new DotCoverMergeSettings {
        ///     LogFile = new FilePath("./log.txt")
        ///   });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Merge")]
        [CakeNamespaceImport("Cake.Common.Tools.DotCover.Merge")]
        public static void DotCoverMerge(
            this ICakeContext context,
            IEnumerable<FilePath> sourceFiles,
            FilePath outputFile,
            DotCoverMergeSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (settings == null)
            {
                settings = new DotCoverMergeSettings();
            }

            // Create the DotCover merger.
            var merger = new DotCoverMerger(
                context.FileSystem, context.Environment,
                context.ProcessRunner, context.Tools);

            // Run DotCover report.
            merger.Merge(sourceFiles, outputFile, settings);
        }
    }
}