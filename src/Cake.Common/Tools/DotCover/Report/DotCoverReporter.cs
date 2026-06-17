// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover.Report
{
    /// <summary>
    /// DotCover Report reporter.
    /// </summary>
    public sealed class DotCoverReporter : DotCoverTool<DotCoverReportSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverReporter" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotCoverReporter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs DotCover Cover with the specified settings.
        /// </summary>
        /// <param name="sourceFile">The DotCover coverage snapshot file name.</param>
        /// <param name="outputFile">The DotCover output file.</param>
        /// <param name="settings">The settings.</param>
        public void Report(
            FilePath sourceFile,
            FilePath outputFile,
            DotCoverReportSettings settings)
        {
            ArgumentNullException.ThrowIfNull(sourceFile);
            ArgumentNullException.ThrowIfNull(settings);

            if (settings.UseLegacySyntax)
            {
                ArgumentNullException.ThrowIfNull(outputFile);
            }

            // Run the tool.
            Run(settings, GetArguments(sourceFile, settings, outputFile));
        }

        /// <summary>
        /// Runs DotCover Cover with the specified settings.
        /// </summary>
        /// <param name="sourceFile">The DotCover coverage snapshot file name.</param>
        /// <param name="settings">The settings.</param>
        public void Report(
            FilePath sourceFile,
            DotCoverReportSettings settings)
        {
            ArgumentNullException.ThrowIfNull(sourceFile);
            ArgumentNullException.ThrowIfNull(settings);

            // Run the tool.
            Run(settings, GetArguments(sourceFile, settings));
        }

        private ProcessArgumentBuilder GetArguments(
            FilePath sourceFile,
            DotCoverReportSettings settings, FilePath outputFile = null)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("report");

            // Set configuration file if exists.
            GetConfigurationFileArgument(settings).CopyTo(builder);

            if (settings.UseLegacySyntax)
            {
                GenerateLegacyArguments(sourceFile, outputFile, settings, builder);
            }
            else
            {
                GenerateArguments(sourceFile, settings, builder);
            }

            // Get Global settings
            GetArguments(settings).CopyTo(builder);

            return builder;
        }

        private void GenerateArguments(FilePath sourceFile, DotCoverReportSettings settings, ProcessArgumentBuilder builder)
        {
            // Set the Source file.
            builder.AppendSwitch("--snapshot-source", sourceFile.MakeAbsolute(_environment).FullPath.Quote());

            // Set Json report output
            if (settings.JsonReportOutput != null)
            {
                builder.AppendSwitch("--json-report-output", settings.JsonReportOutput.MakeAbsolute(_environment).FullPath.Quote());
            }

            // Set test scope, ignore default value
            if (settings.JsonReportCoveringTestsScope.HasValue)
            {
                builder.AppendSwitch("--json-report-covering-tests-scope", settings.JsonReportCoveringTestsScope.Value.ToString().ToLowerInvariant().Quote());
            }

            // Set Xml report output
            if (settings.XmlReportOutput != null)
            {
                builder.AppendSwitch("--xml-report-output", settings.XmlReportOutput.MakeAbsolute(_environment).FullPath.Quote());
            }

            // Set test scope, ignore default value
            if (settings.XmlReportCoveringTestsScope.HasValue)
            {
                builder.AppendSwitch("--xml-report-covering-tests-scope", settings.XmlReportCoveringTestsScope.Value.ToString().ToLowerInvariant().Quote());
            }
        }

        private void GenerateLegacyArguments(FilePath sourceFile, FilePath outputFile, DotCoverReportSettings settings, ProcessArgumentBuilder builder)
        {
            // Set the Source file.
            sourceFile = sourceFile.MakeAbsolute(_environment);
            builder.AppendSwitch("/Source", "=", sourceFile.FullPath.Quote());

            // Set the Output file.
            outputFile = outputFile.MakeAbsolute(_environment);
            builder.AppendSwitch("/Output", "=", outputFile.FullPath.Quote());

            // Set the report type, don't include the default value
            if (settings.ReportType != DotCoverReportType.XML)
            {
                builder.AppendSwitch("/ReportType", "=", settings.ReportType.ToString());
            }
        }
    }
}
