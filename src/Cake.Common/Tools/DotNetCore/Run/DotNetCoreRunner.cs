// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Run
{
    /// <summary>
    /// .NET Core project runner.
    /// </summary>
    public sealed class DotNetCoreRunner : DotNetCoreTool<DotNetCoreRunSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Runs the project using the specified path with arguments and settings.
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        public void Run(string project, ProcessArgumentBuilder arguments, DotNetCoreRunSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(project, arguments, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, ProcessArgumentBuilder arguments, DotNetCoreRunSettings settings)
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