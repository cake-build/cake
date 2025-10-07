﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Run
{
    /// <summary>
    /// .NET project runner.
    /// </summary>
    public sealed class DotNetRunner : DotNetTool<DotNetRunSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Runs the project using the specified path with arguments and settings.
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        public void Run(string project, ProcessArgumentBuilder arguments, DotNetRunSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            RunCommand(settings, GetArguments(project, arguments, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, ProcessArgumentBuilder arguments, DotNetRunSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("run");

            // Specific path?
            if (project != null)
            {
                builder.Append("--project");
                builder.AppendQuoted(project);
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

            // No Restore
            if (settings.NoRestore)
            {
                builder.Append("--no-restore");
            }

            // No Build
            if (settings.NoBuild)
            {
                builder.Append("--no-build");
            }

            // Runtime
            if (!string.IsNullOrEmpty(settings.Runtime))
            {
                builder.Append("--runtime");
                builder.Append(settings.Runtime);
            }

            // Sources
            if (settings.Sources != null)
            {
                foreach (var source in settings.Sources)
                {
                    builder.Append("--source");
                    builder.AppendQuoted(source);
                }
            }

            // Roll Forward Policy
            if (!(settings.RollForward is null))
            {
                builder.Append("--roll-forward");
                builder.Append(settings.RollForward.Value.ToString("F"));
            }

            // MSBuild Settings
            if (settings.MSBuildSettings != null)
            {
                builder.AppendMSBuildSettings(settings.MSBuildSettings, _environment);
            }

            // Arguments
            if (!arguments.IsNullOrEmpty())
            {
                builder.Append("--");
                arguments.CopyTo(builder);
            }

            return builder;
        }
    }
}