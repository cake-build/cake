// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Push
{
    /// <summary>
    /// The Chocolatey package pusher.
    /// </summary>
    public sealed class ChocolateyPusher : ChocolateyTool<ChocolateyPushSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyPusher"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyPusher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Pushes a Chocolatey package to a Chocolatey server and publishes it.
        /// </summary>
        /// <param name="packageFilePath">The package file path.</param>
        /// <param name="settings">The settings.</param>
        public void Push(FilePath packageFilePath, ChocolateyPushSettings settings)
        {
            ArgumentNullException.ThrowIfNull(packageFilePath);

            ArgumentNullException.ThrowIfNull(settings);

            Run(settings, GetArguments(packageFilePath, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath packageFilePath, ChocolateyPushSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("push");

            builder.AppendQuoted(packageFilePath.MakeAbsolute(_environment).FullPath);

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            // Source
            if (settings.Source != null)
            {
                builder.AppendSwitchQuoted("--source", separator, settings.Source);
            }

            // Api Key
            if (settings.ApiKey != null)
            {
                builder.AppendSwitchQuoted("--api-key", separator, settings.ApiKey);
            }

            // Client Code
            if (!string.IsNullOrEmpty(settings.ClientCode))
            {
                builder.AppendSwitchQuoted("--client-code", separator, settings.ClientCode);
            }

            // Redirect URL
            if (!string.IsNullOrEmpty(settings.RedirectUrl))
            {
                builder.AppendSwitchQuoted("--redirect-url", separator, settings.RedirectUrl);
            }

            // Endpoint
            if (!string.IsNullOrEmpty(settings.EndPoint))
            {
                builder.AppendSwitchQuoted("--endpoint", separator, settings.EndPoint);
            }

            // Skip Cleanup
            if (settings.SkipCleanup)
            {
                builder.Append("--skip-cleanup");
            }

            return builder;
        }
    }
}