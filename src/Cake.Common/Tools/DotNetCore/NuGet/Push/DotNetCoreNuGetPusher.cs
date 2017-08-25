// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.NuGet.Push
{
    /// <summary>
    /// .NET Core nuget pusher, pushes a package and it's symbols to the server.
    /// </summary>
    public sealed class DotNetCoreNuGetPusher : DotNetCoreTool<DotNetCoreNuGetPushSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreNuGetPusher" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreNuGetPusher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
        }

        /// <summary>
        /// Push one or more NuGet package using the specified name, version and settings.
        /// </summary>
        /// <param name="packageName">The name of the target package.</param>
        /// <param name="settings">The settings.</param>
        public void Push(string packageName, DotNetCoreNuGetPushSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(packageName, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageName, DotNetCoreNuGetPushSettings settings)
        {
            if (string.IsNullOrWhiteSpace(packageName))
            {
                throw new ArgumentNullException(nameof(packageName));
            }

            var builder = CreateArgumentBuilder(settings);

            builder.Append("nuget push");

            // Specific package
            builder.Append(packageName);

            // Where to push package to
            if (!string.IsNullOrWhiteSpace(settings.Source))
            {
                builder.Append("--source");
                builder.Append(settings.Source);
            }

            // api key for source
            if (!string.IsNullOrWhiteSpace(settings.ApiKey))
            {
                builder.Append("--api-key");
                builder.Append(settings.ApiKey);
            }

            // Where to push symbol package to
            if (!string.IsNullOrWhiteSpace(settings.SymbolSource))
            {
                builder.Append("--symbol-source");
                builder.Append(settings.SymbolSource);
            }

            // api key for symbol source
            if (!string.IsNullOrWhiteSpace(settings.SymbolApiKey))
            {
                builder.Append("--symbol-api-key");
                builder.Append(settings.SymbolApiKey);
            }

            // Timeout
            if (settings.Timeout.HasValue)
            {
                builder.Append("--timeout");
                builder.Append(settings.Timeout.Value.ToString());
            }

            // Disable buffering
            if (settings.DisableBuffering)
            {
                builder.Append("--disable-buffering");
            }

            // push symbol package?
            if (settings.IgnoreSymbols)
            {
                builder.Append("--no-symbols");
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