// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Features
{
    /// <summary>
    /// The Chocolatey feature toggler used to enable/disable Chocolatey Features.
    /// </summary>
    public sealed class ChocolateyFeatureToggler : ChocolateyTool<ChocolateyFeatureSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyFeatureToggler"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyFeatureToggler(
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
        /// <param name="name">The name of the feature.</param>
        /// <param name="settings">The settings.</param>
        public void EnableFeature(string name, ChocolateyFeatureSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Run(settings, GetArguments(true, name, settings));
        }

        /// <summary>
        /// Pins Chocolatey packages using the specified package id and settings.
        /// </summary>
        /// <param name="name">The name of the feature.</param>
        /// <param name="settings">The settings.</param>
        public void DisableFeature(string name, ChocolateyFeatureSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Run(settings, GetArguments(false, name, settings));
        }

        private ProcessArgumentBuilder GetArguments(bool enableDisableToggle, string name, ChocolateyFeatureSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("feature");

            builder.Append(enableDisableToggle ? "enable" : "disable");

            builder.AppendSwitchQuoted("--name", separator, name);

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            return builder;
        }
    }
}