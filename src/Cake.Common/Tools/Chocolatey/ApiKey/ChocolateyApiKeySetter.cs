// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.ApiKey
{
    /// <summary>
    /// The Chocolatey package pinner used to pin Chocolatey packages.
    /// </summary>
    public sealed class ChocolateyApiKeySetter : ChocolateyTool<ChocolateyApiKeySettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyApiKeySetter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyApiKeySetter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Pins Chocolatey packages using the specified package id and settings.
        /// </summary>
        /// <param name="source">The Server URL where the API key is valid.</param>
        /// <param name="settings">The settings.</param>
        public void Set(string source, ChocolateyApiKeySettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(source))
            {
                throw new ArgumentNullException(nameof(source));
            }

            Run(settings, GetArguments(source, settings));
        }

        private ProcessArgumentBuilder GetArguments(string source, ChocolateyApiKeySettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("apikey");

            builder.AppendSwitchQuoted("--source", separator, source);

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            if (!string.IsNullOrEmpty(settings.ApiKey))
            {
                builder.AppendSwitchQuoted("--api-key", separator, settings.ApiKey);
            }

            if (settings.Remove)
            {
                builder.Append("--remove");
            }

            return builder;
        }
    }
}