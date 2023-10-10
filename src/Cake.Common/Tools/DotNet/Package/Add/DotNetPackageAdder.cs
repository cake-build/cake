// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Package.Add
{
    /// <summary>
    /// .NET package adder.
    /// </summary>
    public sealed class DotNetPackageAdder : DotNetTool<DotNetPackageAddSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetPackageAdder" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetPackageAdder(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Adds or updates a package reference in a project file.
        /// </summary>
        /// <param name="packageName">The package reference to add.</param>
        /// <param name="project">The target project file path. If not specified, the command searches the current directory for one.</param>
        /// <param name="settings">The settings.</param>
        public void Add(string packageName, string project, DotNetPackageAddSettings settings)
        {
            if (packageName == null)
            {
                throw new ArgumentNullException(nameof(packageName));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(packageName, project, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageName, string project, DotNetPackageAddSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("add");

            // Project path
            if (project != null)
            {
                builder.AppendQuoted(project);
            }

            // Package Name
            builder.AppendSwitch("package", packageName);

            // Framework
            if (!string.IsNullOrEmpty(settings.Framework))
            {
                builder.AppendSwitch("--framework", settings.Framework);
            }

            // Interactive
            if (settings.Interactive)
            {
                builder.Append("--interactive");
            }

            // No Restore
            if (settings.NoRestore)
            {
                builder.Append("--no-restore");
            }

            // Package Directory
            if (settings.PackageDirectory != null)
            {
                builder.Append("--package-directory");
                builder.AppendQuoted(settings.PackageDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Prerelease
            if (settings.Prerelease)
            {
                builder.Append("--prerelease");
            }

            // Source
            if (!string.IsNullOrEmpty(settings.Source))
            {
                builder.AppendSwitchQuoted("--source", settings.Source);
            }

            // Version
            if (!string.IsNullOrEmpty(settings.Version))
            {
                builder.AppendSwitchQuoted("--version", settings.Version);
            }

            return builder;
        }
    }
}
