// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNetCore.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Publish
{
    /// <summary>
    /// .NET Core project runner.
    /// </summary>
    public sealed class DotNetCorePublisher : DotNetCoreTool<DotNetCorePublishSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCorePublisher" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCorePublisher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Publish the project using the specified path and settings.
        /// </summary>
        /// <param name="path">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Publish(string path, DotNetCorePublishSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(path, settings));
        }

        private ProcessArgumentBuilder GetArguments(string path, DotNetCorePublishSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("publish");

            // Specific path?
            if (path != null)
            {
                builder.AppendQuoted(path);
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--output");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Runtime
            if (!string.IsNullOrEmpty(settings.Runtime))
            {
                builder.Append("--runtime");
                builder.Append(settings.Runtime);
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

            // Version suffix
            if (!string.IsNullOrEmpty(settings.VersionSuffix))
            {
                builder.Append("--version-suffix");
                builder.Append(settings.VersionSuffix);
            }

            if (settings.MSBuildSettings != null)
            {
                builder.AppendMSBuildSettings(settings.MSBuildSettings, _environment);
            }

            return builder;
        }
    }
}
