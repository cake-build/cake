// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.ReportGenerator
{
    /// <summary>
    /// <para>Contains functionality related to <see href="https://github.com/danielpalme/ReportGenerator">ReportGenerator</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from nuget.org, or specify the ToolPath within the <see cref="ReportGeneratorSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=ReportGenerator"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("ReportGenerator")]
    public static class ReportGeneratorAliases
    {
        /// <summary>
        /// Converts the coverage report specified by the glob pattern into human readable form.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The glob pattern.</param>
        /// <param name="targetDir">The output directory.</param>
        /// <example>
        /// <code>
        /// ReportGenerator("c:/temp/coverage/*.xml", "c:/temp/output");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportGenerator(this ICakeContext context, GlobPattern pattern, DirectoryPath targetDir)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var reports = context.Globber.GetFiles(pattern);
            ReportGenerator(context, reports, targetDir);
        }

        /// <summary>
        /// Converts the coverage report specified by the glob pattern into human readable form using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The glob pattern.</param>
        /// <param name="targetDir">The output directory.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ReportGenerator("c:/temp/coverage/*.xml", "c:/temp/output", new ReportGeneratorSettings(){
        ///     ToolPath = "c:/tools/reportgenerator.exe"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportGenerator(this ICakeContext context, GlobPattern pattern, DirectoryPath targetDir, ReportGeneratorSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            var reports = context.Globber.GetFiles(pattern);
            ReportGenerator(context, reports, targetDir, settings);
        }

        /// <summary>
        /// Converts the specified coverage report into human readable form.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="report">The coverage report.</param>
        /// <param name="targetDir">The output directory.</param>
        /// <example>
        /// <code>
        /// ReportGenerator("c:/temp/coverage/report.xml", "c:/temp/output");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportGenerator(this ICakeContext context, FilePath report, DirectoryPath targetDir)
        {
            ReportGenerator(context, report, targetDir, new ReportGeneratorSettings());
        }

        /// <summary>
        /// Converts the specified coverage report into human readable form using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="report">The coverage report.</param>
        /// <param name="targetDir">The output directory.</param>
        /// <param name="settings">The settings.</param>
        /// <example>
        /// <code>
        /// ReportGenerator("c:/temp/coverage.xml", "c:/temp/output", new ReportGeneratorSettings(){
        ///     ToolPath = "c:/tools/reportgenerator.exe"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportGenerator(this ICakeContext context, FilePath report, DirectoryPath targetDir, ReportGeneratorSettings settings)
        {
            ReportGenerator(context, new[] { report }, targetDir, settings);
        }

        /// <summary>
        /// Converts the specified coverage reports into human readable form.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reports">The coverage reports.</param>
        /// <param name="targetDir">The output directory.</param>
        /// <example>
        /// <code>
        /// ReportGenerator(new[] { "c:/temp/coverage1.xml", "c:/temp/coverage2.xml" }, "c:/temp/output");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportGenerator(this ICakeContext context, IEnumerable<FilePath> reports, DirectoryPath targetDir)
        {
            ReportGenerator(context, reports, targetDir, new ReportGeneratorSettings());
        }

        /// <summary>
        /// Converts the specified coverage reports into human readable form using the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="reports">The coverage reports.</param>
        /// <param name="targetDir">The output directory.</param>
        /// <param name="settings">The settings.</param>>
        /// <example>
        /// <code>
        /// ReportGenerator(new[] { "c:/temp/coverage1.xml", "c:/temp/coverage2.xml" }, "c:/temp/output", new ReportGeneratorSettings(){
        ///     ToolPath = "c:/tools/reportgenerator.exe"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportGenerator(this ICakeContext context, IEnumerable<FilePath> reports, DirectoryPath targetDir, ReportGeneratorSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new ReportGeneratorRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(reports, targetDir, settings);
        }
    }
}