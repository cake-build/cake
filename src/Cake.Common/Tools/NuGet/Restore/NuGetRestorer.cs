// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Restore
{
    /// <summary>
    /// The NuGet package restorer used to restore solution packages.
    /// </summary>
    public sealed class NuGetRestorer : NuGetTool<NuGetRestoreSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetRestorer"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The NuGet tool resolver</param>
        public NuGetRestorer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            INuGetToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Restores NuGet packages using the specified settings.
        /// </summary>
        /// <param name="targetFilePath">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Restore(FilePath targetFilePath, NuGetRestoreSettings settings)
        {
            if (targetFilePath == null)
            {
                throw new ArgumentNullException("targetFilePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(targetFilePath, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath targetFilePath, NuGetRestoreSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("restore");
            builder.AppendQuoted(targetFilePath.MakeAbsolute(_environment).FullPath);

            // RequireConsent?
            if (settings.RequireConsent)
            {
                builder.Append("-RequireConsent");
            }

            // Packages Directory.
            if (settings.PackagesDirectory != null)
            {
                builder.Append("-PackagesDirectory");
                builder.AppendQuoted(settings.PackagesDirectory.MakeAbsolute(_environment).FullPath);
            }

            // List of package sources.
            if (settings.Source != null && settings.Source.Count > 0)
            {
                builder.Append("-Source");
                builder.AppendQuoted(string.Join(";", settings.Source));
            }

            // List of package fallback sources.
            if (settings.FallbackSource != null && settings.FallbackSource.Count > 0)
            {
                builder.Append("-FallbackSource");
                builder.AppendQuoted(string.Join(";", settings.FallbackSource));
            }

            // No Cache?
            if (settings.NoCache)
            {
                builder.Append("-NoCache");
            }

            // Disable Parallel Processing?
            if (settings.DisableParallelProcessing)
            {
                builder.Append("-DisableParallelProcessing");
            }

            // Verbosity?
            if (settings.Verbosity.HasValue)
            {
                builder.Append("-Verbosity");
                builder.Append(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            // Configuration file.
            if (settings.ConfigFile != null)
            {
                builder.Append("-ConfigFile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            // MSBuildVersion?
            if (settings.MSBuildVersion.HasValue)
            {
                builder.Append("-MSBuildVersion");
                builder.Append(settings.MSBuildVersion.Value.ToString("D"));
            }

            builder.Append("-NonInteractive");

            return builder;
        }
    }
}
