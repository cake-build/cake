// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Config
{
    /// <summary>
    /// The Chocolatey configuration setter.
    /// </summary>
    public sealed class ChocolateyConfigSetter : ChocolateyTool<ChocolateyConfigSettings>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyConfigSetter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyConfigSetter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
        }

        /// <summary>
        /// Sets Chocolatey configuration parameters using the settings.
        /// </summary>
        /// <param name="name">The name of the config parameter.</param>
        /// <param name="value">The value to assign to the parameter.</param>
        /// <param name="settings">The settings.</param>
        public void Set(string name, string value, ChocolateyConfigSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (string.IsNullOrWhiteSpace(name))
            {
                throw new ArgumentNullException(nameof(name));
            }

            if (string.IsNullOrWhiteSpace(value))
            {
                throw new ArgumentNullException(nameof(value));
            }

            Run(settings, GetArguments(name, value, settings));
        }

        private ProcessArgumentBuilder GetArguments(string name, string value, ChocolateyConfigSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("config");
            builder.Append("set");

            builder.AppendSwitchQuoted("--name", separator, name);

            builder.AppendSwitchQuoted("--value", separator, value);

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            return builder;
        }
    }
}