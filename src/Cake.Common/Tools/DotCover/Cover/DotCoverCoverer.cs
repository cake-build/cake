// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover.Cover
{
    /// <summary>
    /// DotCover Coverer builder.
    /// </summary>
    public sealed class DotCoverCoverer : DotCoverCoverageTool<DotCoverCoverSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverCoverer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotCoverCoverer(
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
        /// <param name="context">The context.</param>
        /// <param name="action">The action.</param>
        /// <param name="outputPath">The output file path.</param>
        /// <param name="settings">The settings.</param>
        public void Cover(ICakeContext context,
            Action<ICakeContext> action,
            FilePath outputPath,
            DotCoverCoverSettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(action);
            ArgumentNullException.ThrowIfNull(outputPath);
            ArgumentNullException.ThrowIfNull(settings);

            // Run the tool.
            Run(settings, GetArguments(context, action, settings, outputPath));
        }

        private ProcessArgumentBuilder GetArguments(
            ICakeContext context,
            Action<ICakeContext> action,
            DotCoverCoverSettings settings,
            FilePath outputPath)
        {
            var builder = new ProcessArgumentBuilder();

            // Command name - always lowercase 'cover' for both formats
            builder.Append("cover");

            // Set configuration file if exists.
            GetConfigurationFileArgument(settings).CopyTo(builder);

            if (settings.UseLegacySyntax)
            {
                // Use legacy format
                GetTargetArguments(context, action).CopyTo(builder);
                // Set the output file - legacy format
                outputPath = outputPath.MakeAbsolute(_environment);
                builder.AppendSwitch("/Output", "=", outputPath.FullPath.Quote());
                // Get Coverage arguments - legacy format
                GetCoverageArguments(settings).CopyTo(builder);
                // Get base arguments - legacy format
                GetArguments(settings).CopyTo(builder);
            }
            else
            {
                // Use new format
                GetCoverTargetArguments(context, action).CopyTo(builder);
                // Set the output file - new format
                outputPath = outputPath.MakeAbsolute(_environment);
                builder.AppendSwitch("--snapshot-output", outputPath.FullPath.Quote());
                // Get Coverage arguments - new format
                GetCoverCoverageArguments(settings).CopyTo(builder);
                // New report options (only available in new format)
                if (settings.JsonReportOutput != null)
                {
                    builder.AppendSwitch("--json-report-output", settings.JsonReportOutput.MakeAbsolute(_environment).FullPath.Quote());
                }

                if (settings.JsonReportCoveringTestsScope.HasValue)
                {
                    builder.AppendSwitch("--json-report-covering-tests-scope", settings.JsonReportCoveringTestsScope.Value.ToString().ToLowerInvariant().Quote());
                }

                if (settings.XmlReportOutput != null)
                {
                    builder.AppendSwitch("--xml-report-output", settings.XmlReportOutput.MakeAbsolute(_environment).FullPath.Quote());
                }

                if (settings.XmlReportCoveringTestsScope.HasValue)
                {
                    builder.AppendSwitch("--xml-report-covering-tests-scope", settings.XmlReportCoveringTestsScope.Value.ToString().ToLowerInvariant().Quote());
                }

                if (settings.TemporaryDirectory != null)
                {
                    builder.AppendSwitch("--temporary-directory", settings.TemporaryDirectory.MakeAbsolute(_environment).FullPath.Quote());
                }

                if (settings.UseApi)
                {
                    builder.Append("--use-api");
                }

                if (settings.NoNGen)
                {
                    builder.Append("--no-ngen");
                }

                // Get base arguments - new format
                GetCoverArguments(settings).CopyTo(builder);
            }

            return builder;
        }

        /// <summary>
        /// Get arguments from coverage settings for Cover command (using new format).
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The process arguments.</returns>
        private ProcessArgumentBuilder GetCoverCoverageArguments(DotCoverCoverageSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // TargetWorkingDir - using new format for Cover command
            if (settings.TargetWorkingDir != null)
            {
                builder.AppendSwitch("--target-working-directory", settings.TargetWorkingDir.MakeAbsolute(_environment).FullPath.Quote());
            }

            // New filtering options (only available in new format)
            if (settings.ExcludeAssemblies.Count > 0)
            {
                var excludeAssemblies = string.Join(',', settings.ExcludeAssemblies);
                builder.AppendSwitch("--exclude-assemblies", excludeAssemblies.Quote());
            }

            if (settings.ExcludeAttributes.Count > 0)
            {
                var excludeAttributes = string.Join(',', settings.ExcludeAttributes);
                builder.AppendSwitch("--exclude-attributes", excludeAttributes.Quote());
            }

            if (settings.ExcludeProcesses.Count > 0)
            {
                var excludeProcesses = string.Join(',', settings.ExcludeProcesses);
                builder.AppendSwitch("--exclude-processes", excludeProcesses.Quote());
            }

            // Legacy filtering options (maintain backward compatibility with old format)
            // Scope
            if (settings.Scope.Count > 0)
            {
                var scope = string.Join(';', settings.Scope);
                builder.AppendSwitch("/Scope", "=", scope.Quote());
            }

            // Filters
            if (settings.Filters.Count > 0)
            {
                var filters = string.Join(';', settings.Filters);
                builder.AppendSwitch("/Filters", "=", filters.Quote());
            }

            // AttributeFilters
            if (settings.AttributeFilters.Count > 0)
            {
                var attributeFilters = string.Join(';', settings.AttributeFilters);
                builder.AppendSwitch("/AttributeFilters", "=", attributeFilters.Quote());
            }

            // ProcessFilters
            if (settings.ProcessFilters.Count > 0)
            {
                var processFilters = string.Join(';', settings.ProcessFilters);
                builder.AppendSwitch("/ProcessFilters", "=", processFilters.Quote());
            }

            // DisableDefaultFilters
            if (settings.DisableDefaultFilters)
            {
                builder.Append("/DisableDefaultFilters");
            }

            return builder;
        }

        /// <summary>
        /// Get arguments from global settings for Cover command (using new format).
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The process arguments.</returns>
        private ProcessArgumentBuilder GetCoverArguments(DotCoverSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            // LogFile - using new format for Cover command
            if (settings.LogFile != null)
            {
                var logFilePath = settings.LogFile.MakeAbsolute(_environment);
                builder.AppendSwitch("--log-file", logFilePath.FullPath.Quote());
            }

            return builder;
        }
    }
}