// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Tool
{
    /// <summary>
    /// .NET Core Extensibility Commands Runner.
    /// </summary>
    public sealed class DotNetCoreToolRunner : DotNetCoreTool<DotNetCoreSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreToolRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreToolRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Execute an assembly using arguments and settings.
        /// </summary>
        /// <param name="projectPath">The target project path.</param>
        /// <param name="command">The command to execute.</param>
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        public void Execute(FilePath projectPath, string command, ProcessArgumentBuilder arguments, DotNetCoreToolSettings settings)
        {
            if (projectPath == null)
            {
                throw new ArgumentNullException(nameof(projectPath));
            }

            if (string.IsNullOrWhiteSpace(command))
            {
                throw new ArgumentNullException(nameof(command));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var processSettings = new ProcessSettings();
            processSettings.WorkingDirectory = projectPath.GetDirectory();

            RunCommand(settings, GetArguments(command, arguments, settings), processSettings);
        }

        private ProcessArgumentBuilder GetArguments(string command, ProcessArgumentBuilder arguments, DotNetCoreToolSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append(command);

            // Arguments
            if (!arguments.IsNullOrEmpty())
            {
                arguments.CopyTo(builder);
            }

            return builder;
        }
    }
}
