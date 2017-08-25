// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Init
{
    /// <summary>
    /// The NuGet package init tool copies all the packages from the source to the hierarchical destination.
    /// </summary>
    public sealed class NuGetIniter : NuGetTool<NuGetInitSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetIniter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The NuGet tool resolver.</param>
        public NuGetIniter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            INuGetToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
        }

        /// <summary>
        /// Init adds all the packages from the source to the hierarchical destination.
        /// </summary>
        /// <param name="sourcePackageSourcePath">Package source to be copied from.</param>
        /// <param name="destinationPackageSourcePath">Package destination to be copied to.</param>
        /// <param name="settings">The settings.</param>
        public void Init(string sourcePackageSourcePath, string destinationPackageSourcePath,
            NuGetInitSettings settings)
        {
            if (string.IsNullOrWhiteSpace(sourcePackageSourcePath))
            {
                throw new ArgumentNullException(nameof(sourcePackageSourcePath));
            }
            if (string.IsNullOrWhiteSpace(destinationPackageSourcePath))
            {
                throw new ArgumentNullException(nameof(destinationPackageSourcePath));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            var sourcePackagePath = sourcePackageSourcePath;
            var destinationPackagePath = destinationPackageSourcePath;

            Run(settings, GetArguments(sourcePackagePath, destinationPackagePath, settings));
        }

        private ProcessArgumentBuilder GetArguments(string sourcePackageSourcePath, string destinationPackageSourcePath, NuGetInitSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("init");
            builder.AppendQuoted(sourcePackageSourcePath);
            builder.AppendQuoted(destinationPackageSourcePath);

            // Expand package?
            if (settings.Expand)
            {
                builder.Append("-Expand");
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

            builder.Append("-NonInteractive");

            return builder;
        }
    }
}
