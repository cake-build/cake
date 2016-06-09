// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreRestorer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreRestorer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
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
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(root, settings));
        }

        private ProcessArgumentBuilder GetArguments(string root, DotNetCoreRestoreSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("restore");

            // Specific root?
            if (root != null)
            {
                builder.Append(root);
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

            // List of fallback package sources
            if (settings.FallbackSources != null)
            {
                foreach (var source in settings.FallbackSources)
                {
                    builder.Append("--fallbacksource");
                    builder.AppendQuoted(source);
                }
            }

            // Config file
            if (settings.ConfigFile != null)
            {
                builder.Append("--configfile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            // List of runtime identifiers
            if (settings.InferRuntimes != null)
            {
                foreach (var runtime in settings.InferRuntimes)
                {
                    builder.Append("--infer-runtimes");
                    builder.AppendQuoted(runtime);
                }
            }

            // Quiet
            if (settings.Quiet)
            {
                builder.Append("--quiet");
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

            // Force english output
            if (settings.ForceEnglishOutput)
            {
                builder.Append("--force-english-output");
            }

            // Verbosity
            if (settings.Verbosity.HasValue)
            {
                builder.Append("--verbosity");
                builder.Append(settings.Verbosity.ToString());
            }

            return builder;
        }
    }
}
