// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DNU.Build
{
    /// <summary>
    /// DNU project builder.
    /// </summary>
    public sealed class DNUBuilder : DNUTool<DNUBuildSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DNUBuilder" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DNUBuilder(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools)
            : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Build the project using the specified path and settings.
        /// </summary>
        /// <param name="path">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Build(string path, DNUBuildSettings settings)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(path, settings));
        }

        private ProcessArgumentBuilder GetArguments(string path, DNUBuildSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("build");

            // Specific path?
            if (path != null)
            {
                builder.AppendQuoted(path);
            }

            // List of frameworks
            if (settings.Frameworks != null && settings.Frameworks.Count > 0)
            {
                foreach (var framework in settings.Frameworks)
                {
                    builder.Append("--framework");
                    builder.AppendQuoted(framework);
                }
            }

            // List of configurations
            if (settings.Configurations != null && settings.Configurations.Count > 0)
            {
                foreach (var configuration in settings.Configurations)
                {
                    builder.Append("--configuration");
                    builder.AppendQuoted(configuration);
                }
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--out");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Quiet?
            if (settings.Quiet)
            {
                builder.Append("--quiet");
            }

            return builder;
        }
    }
}
