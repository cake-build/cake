﻿using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NuGet.Push
{
    /// <summary>
    /// The NuGet package pusher.
    /// </summary>
    public sealed class NuGetPusher : Tool<NuGetPushSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IToolResolver _nugetToolResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPusher"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="nugetToolResolver">The NuGet tool resolver.</param>
        public NuGetPusher(IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, IGlobber globber, IToolResolver nugetToolResolver)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
            _nugetToolResolver = nugetToolResolver;
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

            Run(settings, GetArguments(packageFilePath, settings), settings.ToolPath);
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

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return _nugetToolResolver.Name;
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "NuGet.exe", "nuget.exe" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(NuGetPushSettings settings)
        {
            var path = _nugetToolResolver.ResolveToolPath();
            if (path != null)
            {
                return new[] { path };
            }

            return Enumerable.Empty<FilePath>();
        }
    }
}
