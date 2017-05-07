// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Restore
{
    /// <summary>
    /// .NET Core project restorer.
    /// </summary>
    public sealed class DotNetCoreRestorer : DotNetCoreTool<DotNetCoreRestoreSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreRestorer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="log">The cake log.</param>
        public DotNetCoreRestorer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            ICakeLog log) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
            _log = log;
        }

        /// <summary>
        /// Restore the project using the specified path and settings.
        /// </summary>
        /// <param name="root">List of projects and project folders to restore. Each value can be: a path to a project.json or global.json file, or a folder to recursively search for project.json files.</param>
        /// <param name="settings">The settings.</param>
        public void Restore(string root, DotNetCoreRestoreSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(root, settings));
        }

        private ProcessArgumentBuilder GetArguments(string root, DotNetCoreRestoreSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("restore");

            // Specific root?
            if (root != null)
            {
                builder.AppendQuoted(root);
            }

            // Runtime
            if (!string.IsNullOrEmpty(settings.Runtime))
            {
                builder.Append("--runtime");
                builder.Append(settings.Runtime);
            }

            // Output directory
            if (settings.PackagesDirectory != null)
            {
                builder.Append("--packages");
                builder.AppendQuoted(settings.PackagesDirectory.MakeAbsolute(_environment).FullPath);
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

            // Config file
            if (settings.ConfigFile != null)
            {
                builder.Append("--configfile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            // Ignore failed sources
            if (settings.NoCache)
            {
                builder.Append("--no-cache");
            }

            // Disable parallel
            if (settings.DisableParallel)
            {
                builder.Append("--disable-parallel");
            }

            // Ignore failed sources
            if (settings.IgnoreFailedSources)
            {
                builder.Append("--ignore-failed-sources");
            }

            // Ignore project to project references
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
