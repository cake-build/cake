// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
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
            ArgumentNullException.ThrowIfNull(settings);

            Run(settings, GetArguments(settings));
        }

        private ProcessArgumentBuilder GetArguments(ChocolateyExportSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("export");

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            // Output File Path
            if (settings.OutputFilePath != null)
            {
                builder.AppendSwitchQuoted("--output-file-path", separator, settings.OutputFilePath.FullPath);
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