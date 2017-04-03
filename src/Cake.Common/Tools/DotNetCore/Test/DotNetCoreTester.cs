// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Test
{
    /// <summary>
    /// .NET Core project tester.
    /// </summary>
    public sealed class DotNetCoreTester : DotNetCoreTool<DotNetCoreTestSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreTester" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreTester(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Tests the project using the specified path with arguments and settings.
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public void Test(string project, DotNetCoreTestSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(project, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, DotNetCoreTestSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("test");

            // Specific path?
            if (project != null)
            {
                builder.AppendQuoted(project);
            }

            // Settings
            if (settings.Settings != null)
            {
                builder.Append("--settings");
                builder.AppendQuoted(settings.Settings.MakeAbsolute(_environment).FullPath);
            }

            // Filter
            if (!string.IsNullOrWhiteSpace(settings.Filter))
            {
                builder.Append("--filter");
                builder.AppendQuoted(settings.Filter);
            }

            // Settings
            if (settings.TestAdapterPath != null)
            {
                builder.Append("--test-adapter-path");
                builder.AppendQuoted(settings.TestAdapterPath.MakeAbsolute(_environment).FullPath);
            }

            // Filter
            if (!string.IsNullOrWhiteSpace(settings.Logger))
            {
                builder.Append("--logger");
                builder.AppendQuoted(settings.Logger);
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("--output");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Frameworks
            if (!string.IsNullOrEmpty(settings.Framework))
            {
                builder.Append("--framework");
                builder.Append(settings.Framework);
            }

            // Configuration
            if (!string.IsNullOrEmpty(settings.Configuration))
            {
                builder.Append("--configuration");
                builder.Append(settings.Configuration);
            }

            // Output directory
            if (settings.DiagnosticFile != null)
            {
                builder.Append("--diag");
                builder.AppendQuoted(settings.DiagnosticFile.MakeAbsolute(_environment).FullPath);
            }

            if (settings.NoBuild)
            {
                builder.Append("--no-build");
            }

            return builder;
        }
    }
}
