// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Push
{
    /// <summary>
    /// The NuGet package pusher.
    /// </summary>
    public sealed class NuGetPusher : NuGetTool<NuGetPushSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPusher"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The NuGet tool resolver.</param>
        public NuGetPusher(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            INuGetToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Pushes a NuGet package to a NuGet server and publishes it.
        /// </summary>
        /// <param name="packageFilePath">The package file path.</param>
        /// <param name="settings">The settings.</param>
        public void Push(FilePath packageFilePath, NuGetPushSettings settings)
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

        private ProcessArgumentBuilder GetArguments(FilePath packageFilePath, NuGetPushSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("push");

            builder.AppendQuoted(packageFilePath.MakeAbsolute(_environment).FullPath);

            if (settings.ApiKey != null)
            {
                builder.AppendSecret(settings.ApiKey);
            }

            builder.Append("-NonInteractive");

            if (settings.ConfigFile != null)
            {
                builder.Append("-ConfigFile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            if (settings.Source != null)
            {
                builder.Append("-Source");
                builder.AppendQuoted(settings.Source);
            }

            if (settings.Timeout != null)
            {
                builder.Append("-Timeout");
                builder.Append(Convert.ToInt32(settings.Timeout.Value.TotalSeconds).ToString(CultureInfo.InvariantCulture));
            }

            if (settings.Verbosity != null)
            {
                builder.Append("-Verbosity");
                builder.Append(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            return builder;
        }
    }
}
