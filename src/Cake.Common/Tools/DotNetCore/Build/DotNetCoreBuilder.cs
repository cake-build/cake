// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Build
{
    /// <summary>
    /// .NET Core project builder.
    /// </summary>
    public sealed class DotNetCoreBuilder : DotNetCoreTool<DotNetCoreBuildSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreBuilder" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreBuilder(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Build the project using the specified path and settings.
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public void Build(string project, DotNetCoreBuildSettings settings)
        {
            if (project == null)
            {
                throw new ArgumentNullException(nameof(project));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(project, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, DotNetCoreBuildSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("build");

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

            // Runtime
            if (!string.IsNullOrEmpty(settings.Runtime))
            {
                builder.Append("--runtime");
                builder.Append(settings.Runtime);
            }

            // Framework
            if (!string.IsNullOrEmpty(settings.Framework))
            {
                builder.Append("--framework");
                builder.Append(settings.Framework);
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

            // No Incremental
            if (settings.NoIncremental)
            {
                builder.Append("--no-incremental");
            }

            // No Incremental
            if (settings.NoDependencies)
            {
                builder.Append("--no-dependencies");
            }

            if (settings.MSBuildSettings != null)
            {
                builder.AppendMSBuildSettings(settings.MSBuildSettings, _environment);
            }

            return builder;
        }
    }
}
