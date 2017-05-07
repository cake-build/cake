// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// .NET Core project builder.
    /// </summary>
    public sealed class DotNetCoreMSBuildBuilder : DotNetCoreTool<DotNetCoreMSBuildSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreMSBuildBuilder" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreMSBuildBuilder(
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
        /// <param name="projectOrDirectory">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public void Build(string projectOrDirectory, DotNetCoreMSBuildSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(projectOrDirectory, settings));
        }

        private ProcessArgumentBuilder GetArguments(string projectOrDirectory, DotNetCoreMSBuildSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("msbuild");

            // add msbuild settings
            builder.AppendMSBuildSettings(settings, _environment);

            // Specific path?
            if (projectOrDirectory != null)
            {
                builder.AppendQuoted(projectOrDirectory);
            }

            return builder;
        }
    }
}
