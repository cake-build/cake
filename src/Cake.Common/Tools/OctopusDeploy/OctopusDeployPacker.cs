// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// The Octopus deploy package packer.
    /// </summary>
    public sealed class OctopusDeployPacker : OctopusDeployTool<OctopusPackSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="OctopusDeployPacker"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public OctopusDeployPacker(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Creates an Octopus deploy package with the specified ID.
        /// </summary>
        /// <param name="id">The package ID.</param>
        /// <param name="settings">The settings.</param>
        public void Pack(string id, OctopusPackSettings settings)
        {
            if (id == null)
            {
                throw new ArgumentNullException(nameof(id));
            }

            var arguments = GetArguments(id, settings);
            Run(settings, arguments);
        }

        private ProcessArgumentBuilder GetArguments(string id, OctopusPackSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("pack");
            builder.Append($"--id {id}");

            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                builder.AppendSwitch("--version", settings.Version);
            }

            if (settings.OutFolder != null)
            {
                builder.AppendSwitchQuoted("--outFolder", settings.OutFolder.MakeAbsolute(Environment).FullPath);
            }

            if (settings.BasePath != null)
            {
                builder.AppendSwitchQuoted("--basePath", settings.BasePath.MakeAbsolute(Environment).FullPath);
            }

            if (!string.IsNullOrWhiteSpace(settings.Author))
            {
                builder.AppendSwitchQuoted("--author", settings.Author);
            }

            if (!string.IsNullOrWhiteSpace(settings.Title))
            {
                builder.AppendSwitchQuoted("--title", settings.Title);
            }

            if (!string.IsNullOrWhiteSpace(settings.Description))
            {
                builder.AppendSwitchQuoted("--description", settings.Description);
            }

            if (!string.IsNullOrWhiteSpace(settings.ReleaseNotes))
            {
                builder.AppendSwitchQuoted("--releaseNotes", settings.ReleaseNotes);
            }

            if (settings.ReleaseNotesFile != null)
            {
                builder.AppendSwitchQuoted("--releaseNotesFile", settings.ReleaseNotesFile.MakeAbsolute(Environment).FullPath);
            }

            if (settings.Include != null)
            {
                foreach (var include in settings.Include)
                {
                    builder.AppendSwitchQuoted("--include", include);
                }
            }

            if (settings.Overwrite)
            {
                builder.Append("--overwrite");
            }

            if (settings.Format == OctopusPackFormat.Zip)
            {
                builder.AppendSwitch("--format", "Zip");
            }

            return builder;
        }
    }
}
