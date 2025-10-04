// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.MSBuild
{
    /// <summary>
    /// .NET Core project builder.
    /// </summary>
    public sealed class DotNetMSBuildBuilder : DotNetTool<DotNetMSBuildSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetMSBuildBuilder" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetMSBuildBuilder(
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
        /// <param name="standardOutputAction">The action to invoke with the standard output.</param>
        public void Build(string projectOrDirectory, DotNetMSBuildSettings settings, Action<IEnumerable<string>> standardOutputAction)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(
                settings,
                GetArguments(projectOrDirectory, settings),
                standardOutputAction == null ? null : new ProcessSettings { RedirectStandardOutput = true },
                standardOutputAction == null ? null : new Action<IProcess>(process => standardOutputAction(process.GetStandardOutput())));
        }

        private ProcessArgumentBuilder GetArguments(string projectOrDirectory, DotNetMSBuildSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("msbuild");

            // add msbuild settings
            builder.AppendMSBuildSettings(settings, _environment, false);

            // Specific path?
            if (projectOrDirectory != null)
            {
                builder.AppendQuoted(projectOrDirectory);
            }

            return builder;
        }
    }
}
