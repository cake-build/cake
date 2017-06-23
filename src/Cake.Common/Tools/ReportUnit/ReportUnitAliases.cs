// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.Tools.ReportUnit
{
    /// <summary>
    /// <para>Contains functionality related to <see href="http://reportunit.relevantcodes.com/">ReportUnit</see>.</para>
    /// <para>
    /// In order to use the commands for this alias, include the following in your build.cake file to download and
    /// install from NuGet.org, or specify the ToolPath within the <see cref="ReportUnitSettings" /> class:
    /// <code>
    /// #tool "nuget:?package=ReportUnit"
    /// </code>
    /// </para>
    /// </summary>
    [CakeAliasCategory("ReportUnit")]
    public static class ReportUnitAliases
    {
        /// <summary>
        /// Converts the reports in the specified directory into human readable form.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inputFolder">The input folder.</param>
        /// <example>
        /// <para>Provide only an input folder, which will causes ReportUnit to search entire directory for report files.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// ReportUnit("c:/temp");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportUnit(this ICakeContext context, DirectoryPath inputFolder)
        {
            ReportUnit(context, inputFolder, null, new ReportUnitSettings());
        }

        /// <summary>
        /// Converts the reports in the specified directory into human readable form.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inputFolder">The input folder.</param>
        /// <param name="settings">The ReportUnit settings.</param>
        /// <example>
        /// <para>Provide an input folder and custom ToolPath, which will causes ReportUnit to search entire directory for report files.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// ReportUnit("c:/temp", new ReportUnitSettings(){
        ///     ToolPath = "c:/tools/reportunit.exe"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportUnit(this ICakeContext context, DirectoryPath inputFolder, ReportUnitSettings settings)
        {
            ReportUnit(context, inputFolder, null, settings);
        }

        /// <summary>
        /// Converts the reports in the specified directory into human readable form and outputs to specified folder.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inputFolder">The input folder.</param>
        /// <param name="outputFolder">The output folder.</param>
        /// <param name="settings">The ReportUnit settings.</param>
        /// <example>
        /// <para>Provide both input and output folder, which will causes ReportUnit to search entire directory for report files, and output the results to specified location.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// ReportUnit("c:/temp/input", "c:/temp/output");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportUnit(this ICakeContext context, DirectoryPath inputFolder, DirectoryPath outputFolder, ReportUnitSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new ReportUnitRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(inputFolder, outputFolder, settings);
        }

        /// <summary>
        /// Converts the single specified report into human readable form and outputs to specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inputFile">The input file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <example>
        /// <para>Provide both input and output file, which will causes ReportUnit to transform only the specific file, and output to the specified location.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// ReportUnit("c:/temp/input", "c:/temp/output");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportUnit(this ICakeContext context, FilePath inputFile, FilePath outputFile)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new ReportUnitRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(inputFile, outputFile, new ReportUnitSettings());
        }

        /// <summary>
        /// Converts the single specified report into human readable form and outputs to specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="inputFile">The input file.</param>
        /// <param name="outputFile">The output file.</param>
        /// <param name="settings">The ReportUnit settings.</param>
        /// <example>
        /// <para>Provide both input and output file, which will causes ReportUnit to transform only the specific file, and output to the specified location.  Also use a custom path for the reportunit.exe.</para>
        /// <para>Cake task:</para>
        /// <code>
        /// ReportUnit("c:/temp/input", "c:/temp/output", new ReportUnitSettings(){
        ///     ToolPath = "c:/tools/reportunit.exe"
        /// });
        /// </code>
        /// </example>
        [CakeMethodAlias]
        public static void ReportUnit(this ICakeContext context, FilePath inputFile, FilePath outputFile, ReportUnitSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }

            var runner = new ReportUnitRunner(context.FileSystem, context.Environment, context.ProcessRunner, context.Tools);
            runner.Run(inputFile, outputFile, settings);
        }
    }
}
