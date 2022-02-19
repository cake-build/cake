// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Export
{
    /// <summary>
    /// The Chocolatey package exporter used to export Chocolatey packages.
    /// </summary>
    public sealed class ChocolateyExporter : ChocolateyTool<ChocolateyExportSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyExporter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyExporter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Exports the currently installed Chocolatey packages using the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Export(ChocolateyExportSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(ChocolateyExportSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("export");

            // Debug
            if (settings.Debug)
            {
                builder.Append("-d");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("-v");
            }

            // Always say yes, so as to not show interactive prompt
            builder.Append("-y");

            // Force
            if (settings.Force)
            {
                builder.Append("-f");
            }

            // Noop
            if (settings.Noop)
            {
                builder.Append("--noop");
            }

            // Limit Output
            if (settings.LimitOutput)
            {
                builder.Append("-r");
            }

            // Execution Timeout
            if (settings.ExecutionTimeout != 0)
            {
                builder.Append("--execution-timeout");
                builder.AppendQuoted(settings.ExecutionTimeout.ToString(CultureInfo.InvariantCulture));
            }

            // Cache Location
            if (!string.IsNullOrWhiteSpace(settings.CacheLocation))
            {
                builder.Append("-c");
                builder.AppendQuoted(settings.CacheLocation);
            }

            // Allow Unofficial
            if (settings.AllowUnofficial)
            {
                builder.Append("--allowunofficial");
            }

            // Output File Path
            if (settings.OutputFilePath != null)
            {
                builder.Append("--output-file-path");
                builder.AppendQuoted(settings.OutputFilePath.FullPath);
            }

            // Include Version Numbers
            if (settings.IncludeVersionNumbers)
            {
                builder.Append("--include-version-numbers");
            }

            return builder;
        }
    }
}