// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.NuGet.Push;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.NuGet.Push
{
    /// <summary>
    /// .NET Core nuget pusher. Pushes a package and its symbols to the server.
    /// </summary>
    public sealed class DotNetCoreNuGetPusher : DotNetCoreTool<DotNetNuGetPushSettings>
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
        public void Push(string packageName, DotNetNuGetPushSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(packageName, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageName, DotNetNuGetPushSettings settings)
        {
            if (string.IsNullOrWhiteSpace(packageName))
            {
                throw new ArgumentNullException(nameof(packageName));
            }

            var builder = CreateArgumentBuilder(settings);

            builder.Append("nuget push");

            // Specific package
            builder.AppendQuoted(packageName);

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
                builder.AppendQuotedSecret(settings.ApiKey);
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
                builder.AppendQuotedSecret(settings.SymbolApiKey);
            }

            // No service endpoint
            if (settings.NoServiceEndpoint)
            {
                builder.Append("--no-service-endpoint");
            }

            // Interactive
            if (settings.Interactive)
            {
                builder.Append("--interactive");
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

            // skip duplicate
            if (settings.SkipDuplicate)
            {
                builder.Append("--skip-duplicate");
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