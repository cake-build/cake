// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.NuGet.Delete
{
    /// <summary>
    /// The NuGet package pusher.
    /// </summary>
    public sealed class NuGetDeleter : NuGetTool<NuGetDeleteSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetDeleter"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The NuGet tool resolver.</param>
        /// <param name="log">The logger.</param>
        public NuGetDeleter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            INuGetToolResolver resolver,
            ICakeLog log) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _environment = environment;
            _log = log;
        }

        /// <summary>
        /// Deletes or unlists a package from a package source.
        /// </summary>
        /// <param name="packageID">The package ID (name).</param>
        /// <param name="packageVersion">The package version.</param>
        /// <param name="settings">The settings.</param>
        public void Delete(string packageID, string packageVersion, NuGetDeleteSettings settings)
        {
            if (string.IsNullOrWhiteSpace(packageID))
            {
                throw new ArgumentNullException(nameof(packageID));
            }
            if (string.IsNullOrWhiteSpace(packageVersion))
            {
                throw new ArgumentNullException(nameof(packageVersion));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            Run(settings, GetArguments(packageID, packageVersion, settings));
        }

        private ProcessArgumentBuilder GetArguments(string packageID, string packageVersion, NuGetDeleteSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("delete");

            builder.Append(packageID);

            builder.Append(packageVersion);

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
            else
            {
                _log.Verbose("No Source property has been set.  Depending on your configuration, this may cause problems.");
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