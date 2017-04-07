// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.NuGet.Delete
{
    /// <summary>
    /// .NET Core nuget deleter, deletes or unlists a package from the server.
    /// </summary>
    public sealed class DotNetCoreNuGetDeleter : DotNetCoreTool<DotNetCoreNuGetDeleteSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreNuGetDeleter" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreNuGetDeleter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Delete the NuGet package using the specified name, version and settings.
        /// </summary>
        /// <param name="packageName">The name of the target package.</param>
        /// <param name="version">Version of the package.</param>
        /// <param name="settings">The settings.</param>
        public void Delete(string packageName, string version, DotNetCoreNuGetDeleteSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(packageName, version, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageName, string packageVersion, DotNetCoreNuGetDeleteSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("nuget delete");

            // Specific package and version?
            if (!string.IsNullOrWhiteSpace(packageName))
            {
                builder.Append(packageName);

                if (!string.IsNullOrWhiteSpace(packageVersion))
                {
                    builder.Append(packageVersion);
                }
            }

            // Source to delete package at
            if (!string.IsNullOrWhiteSpace(settings.Source))
            {
                builder.Append("--source");
                builder.AppendQuoted(settings.Source);
            }

            // Is it CI?
            if (settings.NonInteractive)
            {
                builder.Append("--non-interactive");
            }

            // API Key
            if (!string.IsNullOrEmpty(settings.ApiKey))
            {
                builder.Append("--api-key");
                builder.AppendQuoted(settings.ApiKey);
            }

            // Force English Output
            if (settings.ForceEnglishOutput)
            {
                builder.Append("--force-english-output");
            }

            return builder;
        }
    }
}