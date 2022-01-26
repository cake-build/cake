// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Store
{
    /// <summary>
    /// .NET assemblies storer.
    /// </summary>
    public sealed class DotNetStorer : DotNetTool<DotNetStoreSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetStorer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetStorer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Stores the specified assemblies in the runtime package store.
        /// </summary>
        /// <param name="manifestFiles">A list of manifest files to store.</param>
        /// <param name="framework">The specific framework.</param>
        /// <param name="runtime">The target runtime.</param>
        /// <param name="settings">The settings.</param>
        public void Store(IEnumerable<FilePath> manifestFiles, string framework, string runtime, DotNetStoreSettings settings)
        {
            if (manifestFiles == null || !manifestFiles.Any())
            {
                throw new ArgumentNullException(nameof(manifestFiles));
            }

            if (string.IsNullOrWhiteSpace(framework))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(framework));
            }

            if (string.IsNullOrWhiteSpace(runtime))
            {
                throw new ArgumentException("Value cannot be null or whitespace.", nameof(runtime));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(manifestFiles, framework, runtime, settings));
        }

        private ProcessArgumentBuilder GetArguments(IEnumerable<FilePath> manifestFiles, string framework, string runtime, DotNetStoreSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("store");

            // Specific path?
            foreach (var manifest in manifestFiles)
            {
                builder.Append("--manifest");
                builder.AppendQuoted(manifest.MakeAbsolute(_environment).FullPath);
            }

            // Framework
            builder.Append("--framework");
            builder.Append(framework);

            // Runtime
            builder.Append("--runtime");
            builder.Append(runtime);

            // Runtime
            if (!string.IsNullOrEmpty(settings.FrameworkVersion))
            {
                builder.Append("--framework-version");
                builder.Append(settings.FrameworkVersion);
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--output");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Skip Optimization
            if (settings.SkipOptimization)
            {
                builder.Append("--skip-optimization");
            }

            // Skip Symbols
            if (settings.SkipSymbols)
            {
                builder.Append("--skip-symbols");
            }

            // Use Current Runtime Identifier
            if (settings.UseCurrentRuntime)
            {
                builder.Append("--use-current-runtime");
            }

            return builder;
        }
    }
}
