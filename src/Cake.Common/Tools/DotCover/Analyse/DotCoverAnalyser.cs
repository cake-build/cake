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
    public sealed class DotCoverAnalyser : DotCoverTool<DotCoverAnalyseSettings>
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
                throw new ArgumentNullException("context");
            }
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException("outputPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Run the tool using the interceptor.
            var interceptor = InterceptAction(context, action);

            // Run the tool.
            Run(settings, GetArguments(interceptor, settings, outputPath));
        }

        private static DotCoverContext InterceptAction(
            ICakeContext context,
            Action<ICakeContext> action)
        {
            var interceptor = new DotCoverContext(context);
            action(interceptor);

            // Validate arguments.
            if (interceptor.FilePath == null)
            {
                throw new CakeException("No tool was started.");
            }

            return interceptor;
        }

        private ProcessArgumentBuilder GetArguments(
            DotCoverContext context,
            DotCoverAnalyseSettings settings,
            FilePath outputPath)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("Analyse");

            // The target application to call.
            builder.AppendSwitch("/TargetExecutable", "=", context.FilePath.FullPath.Quote());

            // The arguments to the target application.
            if (context.Settings != null && context.Settings.Arguments != null)
            {
                var arguments = context.Settings.Arguments.Render();
                if (!string.IsNullOrWhiteSpace(arguments))
                {
                    arguments = arguments.Replace("\"", "\\\"");
                    builder.AppendSwitch("/TargetArguments", "=", arguments.Quote());
                }
            }

            // Set the output file.
            outputPath = outputPath.MakeAbsolute(_environment);
            builder.AppendSwitch("/Output", "=", outputPath.FullPath.Quote());

            // Set the report type, don't include the default value
            if (settings.ReportType != DotCoverReportType.XML)
            {
                builder.AppendSwitch("/ReportType", "=", settings.ReportType.ToString());
            }

            // TargetWorkingDir
            if (settings.TargetWorkingDir != null)
            {
                builder.AppendSwitch("/TargetWorkingDir", "=", settings.TargetWorkingDir.MakeAbsolute(_environment).FullPath.Quote());
            }

            // Scope
            if (settings.Scope.Count > 0)
            {
                var scope = string.Join(";", settings.Scope);
                builder.AppendSwitch("/Scope", "=", scope.Quote());
            }

            // Filters
            if (settings.Filters.Count > 0)
            {
                var filters = string.Join(";", settings.Filters);
                builder.AppendSwitch("/Filters", "=", filters.Quote());
            }

            // Filters
            if (settings.AttributeFilters.Count > 0)
            {
                var attributeFilters = string.Join(";", settings.AttributeFilters);
                builder.AppendSwitch("/AttributeFilters", "=", attributeFilters.Quote());
            }

            // DisableDefaultFilters
            if (settings.DisableDefaultFilters)
            {
                builder.Append("/DisableDefaultFilters");
            }

            return builder;
        }
    }
}
