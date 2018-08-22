// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover.Analyse
{
    /// <summary>
    /// DotCover Analyser builder.
    /// </summary>
    public sealed class DotCoverAnalyser : DotCoverCoverageTool<DotCoverAnalyseSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverAnalyser" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotCoverAnalyser(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs DotCover Analyse with the specified settings.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="action">The action.</param>
        /// <param name="outputPath">The output file path.</param>
        /// <param name="settings">The settings.</param>
        public void Analyse(ICakeContext context,
            Action<ICakeContext> action,
            FilePath outputPath,
            DotCoverAnalyseSettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (action == null)
            {
                throw new ArgumentNullException(nameof(action));
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException(nameof(outputPath));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            // Run the tool.
            Run(settings, GetArguments(context, action, settings, outputPath));
        }

        private ProcessArgumentBuilder GetArguments(
            ICakeContext context,
            Action<ICakeContext> action,
            DotCoverAnalyseSettings settings,
            FilePath outputPath)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("Analyse");

            // Set configuration file if exists.
            GetConfigurationFileArgument(settings).CopyTo(builder);

            // Get Target executable arguments
            GetTargetArguments(context, action).CopyTo(builder);

            // Set the output file.
            outputPath = outputPath.MakeAbsolute(_environment);
            builder.AppendSwitch("/Output", "=", outputPath.FullPath.Quote());

            // Set the report type, don't include the default value
            if (settings.ReportType != DotCoverReportType.XML)
            {
                builder.AppendSwitch("/ReportType", "=", settings.ReportType.ToString());
            }

            // Get Coverage arguments
            GetCoverageArguments(settings).CopyTo(builder);

            // Get base arguments
            GetArguments(settings).CopyTo(builder);

            return builder;
        }
    }
}