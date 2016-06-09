// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DNU.Restore
{
    /// <summary>
    /// DNU NuGet package restorer.
    /// </summary>
    public sealed class DNURestorer : DNUTool<DNURestoreSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DNURestorer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DNURestorer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Restores NuGet packages using the specified path and settings.
        /// </summary>
        /// <param name="path">The project path.</param>
        /// <param name="settings">The settings.</param>
        public void Restore(FilePath path, DNURestoreSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(path, settings));
        }

        private ProcessArgumentBuilder GetArguments(FilePath path, DNURestoreSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("restore");

            // Specific project path?
            if (path != null)
            {
                builder.AppendQuoted(path.MakeAbsolute(_environment).FullPath);
            }

            // List of package sources
            if (settings.Sources != null && settings.Sources.Count > 0)
            {
                foreach (var source in settings.Sources)
                {
                    builder.Append("--source");
                    builder.AppendQuoted(source);
                }
            }

            // List of fallback package sources
            if (settings.FallbackSources != null && settings.FallbackSources.Count > 0)
            {
                foreach (var fallbacksource in settings.FallbackSources)
                {
                    builder.Append("--fallbacksource");
                    builder.AppendQuoted(fallbacksource);
                }
            }

            // Proxy
            if (settings.Proxy != null)
            {
                builder.Append("--proxy");
                builder.AppendQuoted(settings.Proxy);
            }

            // No Cache?
            if (settings.NoCache)
            {
                builder.Append("--no-cache");
            }

            // Packages
            if (settings.Packages != null)
            {
                builder.Append("--packages");
                builder.AppendQuoted(settings.Packages.MakeAbsolute(_environment).FullPath);
            }

            // Ignore failed sources?
            if (settings.IgnoreFailedSources)
            {
                builder.Append("--ignore-failed-sources");
            }

            // Quiet?
            if (settings.Quiet)
            {
                builder.Append("--quiet");
            }

            // Parallel?
            if (settings.Parallel)
            {
                builder.Append("--parallel");
            }

            // Locked?
            if (settings.Locked.HasValue)
            {
                switch (settings.Locked)
                {
                    case DNULocked.Lock:
                        builder.Append("--lock");
                        break;
                    case DNULocked.Unlock:
                        builder.Append("--unlock");
                        break;
                }
            }

            // List of runtime identifiers
            if (settings.Runtimes != null && settings.Runtimes.Count > 0)
            {
                foreach (var runtime in settings.Runtimes)
                {
                    builder.Append("--runtime");
                    builder.AppendQuoted(runtime);
                }
            }

            return builder;
        }
    }
}
