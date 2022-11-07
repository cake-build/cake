// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Pin
{
    /// <summary>
    /// The Chocolatey package pinner used to pin Chocolatey packages.
    /// </summary>
    public sealed class ChocolateyPinner : ChocolateyTool<ChocolateyPinSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyPinner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyPinner(
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
        /// <param name="name">The name of the package.</param>
        /// <param name="settings">The settings.</param>
        public void Pin(string name, ChocolateyPinSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            Run(settings, GetArguments(name, settings));
        }

        private ProcessArgumentBuilder GetArguments(string name, ChocolateyPinSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("pin");
            builder.Append("add");

            builder.AppendSwitchQuoted("--name", separator, name);

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            // Version
            if (settings.Version != null)
            {
                builder.AppendSwitchQuoted("--version", separator, settings.Version);
            }

            // Pin Reason
            if (!string.IsNullOrEmpty(settings.PinReason))
            {
                builder.AppendSwitchQuoted("--pin-reason", separator, settings.PinReason);
            }

            return builder;
        }
    }
}