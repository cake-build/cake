// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Pack
{
    /// <summary>
    /// .NET Core project packer.
    /// </summary>
    public sealed class DotNetCorePacker : DotNetCoreTool<DotNetCorePackSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCorePacker" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCorePacker(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Pack the project using the specified path and settings.
        /// </summary>
        /// <param name="project">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Pack(string project, DotNetCorePackSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(project, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, DotNetCorePackSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("pack");

            // Specific path?
            if (project != null)
            {
                builder.AppendQuoted(project);
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--output");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // No build
            if (settings.NoBuild)
            {
                builder.Append("--no-build");
            }

            // Include symbols
            if (settings.IncludeSymbols)
            {
                builder.Append("--include-symbols");
            }

            // Include source
            if (settings.IncludeSource)
            {
                builder.Append("--include-source");
            }

            // Configuration
            if (!string.IsNullOrEmpty(settings.Configuration))
            {
                builder.Append("--configuration");
                builder.Append(settings.Configuration);
            }

            // Version suffix
            if (!string.IsNullOrEmpty(settings.VersionSuffix))
            {
                builder.Append("--version-suffix");
                builder.Append(settings.VersionSuffix);
            }

            // Serviceable
            if (settings.Serviceable)
            {
                builder.Append("--serviceable");
            }

            if (settings.MSBuildSettings != null)
            {
                builder.AppendMSBuildSettings(settings.MSBuildSettings, _environment);
            }

            return builder;
        }
    }
}
