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
            if (sourceFile == null)
            {
                throw new ArgumentNullException("sourceFile");
            }
            if (outputFile == null)
            {
                throw new ArgumentNullException("outputFile");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Run the tool.
            Run(settings, GetArguments(sourceFile, outputFile, settings));
        }

        private ProcessArgumentBuilder GetArguments(
            FilePath sourceFile,
            FilePath outputFile,
            DotCoverReportSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("Report");

            // Set configuration file if exists.
            GetConfigurationFileArgument(settings).CopyTo(builder);

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

            // Get Global settings
            GetArguments(settings).CopyTo(builder);

            return builder;
        }
    }
}
