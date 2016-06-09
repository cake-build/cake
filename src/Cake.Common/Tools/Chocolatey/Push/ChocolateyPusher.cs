// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;
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
            if (packageFilePath == null)
            {
                throw new ArgumentNullException("packageFilePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(packageFilePath, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath packageFilePath, ChocolateyPushSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("push");

            builder.AppendQuoted(packageFilePath.MakeAbsolute(_environment).FullPath);

            if (settings.Source != null)
            {
                builder.Append("-s");
                builder.AppendQuoted(settings.Source);
            }

            if (settings.Timeout != null)
            {
                builder.Append("-t");
                builder.Append(Convert.ToInt32(settings.Timeout.Value.TotalSeconds).ToString(CultureInfo.InvariantCulture));
            }

            if (settings.ApiKey != null)
            {
                builder.Append("-k");
                builder.AppendSecret(settings.ApiKey);
            }

            // Debug
            if (settings.Debug)
            {
                builder.Append("-d");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("-v");
            }

            // Always say yes, so as to not show interactive prompt
            builder.Append("-y");

            // Force
            if (settings.Force)
            {
                builder.Append("-f");
            }

            // Noop
            if (settings.Noop)
            {
                builder.Append("--noop");
            }

            // Limit Output
            if (settings.LimitOutput)
            {
                builder.Append("-r");
            }

            // Execution Timeout
            if (settings.ExecutionTimeout != 0)
            {
                builder.Append("--execution-timeout");
                builder.AppendQuoted(settings.ExecutionTimeout.ToString(CultureInfo.InvariantCulture));
            }

            // Cache Location
            if (!string.IsNullOrWhiteSpace(settings.CacheLocation))
            {
                builder.Append("-c");
                builder.AppendQuoted(settings.CacheLocation);
            }

            // Allow Unofficial
            if (settings.AllowUnofficial)
            {
                builder.Append("--allowunofficial");
            }

            return builder;
        }
    }
}
