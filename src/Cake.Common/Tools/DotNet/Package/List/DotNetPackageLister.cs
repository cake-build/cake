// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Text.Json;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Package.List
{
    /// <summary>
    /// .NET package lister.
    /// </summary>
    public sealed class DotNetPackageLister : DotNetTool<DotNetPackageListSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetPackageLister" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetPackageLister(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Lists the package references for a project or solution.
        /// </summary>
        /// <param name="project">The project or solution file to operate on. If not specified, the command searches the current directory for one. If more than one solution or project is found, an error is thrown.</param>
        /// <param name="settings">The settings.</param>
        /// <returns>A task with the GitVersion results.</returns>
        public DotNetPackageList List(string project, DotNetPackageListSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var output = string.Empty;
            Run(settings, GetArguments(project, settings), new ProcessSettings { RedirectStandardOutput = true }, process =>
            {
                output = string.Join('\n', process.GetStandardOutput());
            });

            return JsonSerializer.Deserialize<DotNetPackageList>(output, new JsonSerializerOptions
            {
                PropertyNameCaseInsensitive = true
            });
        }

        private ProcessArgumentBuilder GetArguments(string project, DotNetPackageListSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("list");

            // Project path
            if (project != null)
            {
                builder.AppendQuoted(project);
            }

            builder.Append("package");

            // Config File
            if (settings.ConfigFile != null)
            {
                builder.AppendSwitchQuoted("--config", settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            // Deprecated
            if (settings.Deprecated)
            {
                builder.Append("--deprecated");
            }

            // Framework
            if (!string.IsNullOrEmpty(settings.Framework))
            {
                builder.AppendSwitch("--framework", settings.Framework);
            }

            // Highest Minor
            if (settings.HighestMinor)
            {
                builder.Append("--highest-minor");
            }

            // Highest Patch
            if (settings.HighestPatch)
            {
                builder.Append("--highest-patch");
            }

            // Prerelease
            if (settings.Prerelease)
            {
                builder.Append("--include-prerelease");
            }

            // Transitive
            if (settings.Transitive)
            {
                builder.Append("--include-transitive");
            }

            // Interactive
            if (settings.Interactive)
            {
                builder.Append("--interactive");
            }

            // Outdated
            if (settings.Outdated)
            {
                builder.Append("--outdated");
            }

            // Source
            if (settings.Source != null && settings.Source.Any())
            {
                foreach (var source in settings.Source)
                {
                    builder.AppendSwitchQuoted("--source", source);
                }
            }

            // Vulnerable
            if (settings.Vulnerable)
            {
                builder.Append("--vulnerable");
            }

            // Format
            builder.Append("--format json");

            // Version
            builder.Append("--output-version 1");

            return builder;
        }
    }
}
