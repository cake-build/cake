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
        /// <param name="arguments">The arguments.</param>
        /// <param name="settings">The settings.</param>
        public void Test(string project, ProcessArgumentBuilder arguments, DotNetCoreTestSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(project, arguments, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, ProcessArgumentBuilder arguments, DotNetCoreTestSettings settings)
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

            // Logger
#pragma warning disable CS0618
            if (!string.IsNullOrWhiteSpace(settings.Logger))
            {
                builder.Append("--logger");
                builder.AppendQuoted(settings.Logger);
            }
#pragma warning restore CS0618

            // Loggers
            if (settings.Loggers != null)
            {
                foreach (var logger in settings.Loggers)
                {
                    builder.Append("--logger");
                    builder.AppendQuoted(logger);
                }
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

            // Collectors
            if (settings.Collectors != null)
            {
                foreach (var collector in settings.Collectors)
                {
                    builder.Append("--collect");
                    builder.AppendQuoted(collector);
                }
            }

            // Diagnostic file
            if (settings.DiagnosticFile != null)
            {
                builder.Append("--diag");
                builder.AppendQuoted(settings.DiagnosticFile.MakeAbsolute(_environment).FullPath);
            }

            // No Build
            if (settings.NoBuild)
            {
                builder.Append("--no-build");
            }

            // No Restore
            if (settings.NoRestore)
            {
                builder.Append("--no-restore");
            }

            // No Logo
            if (settings.NoLogo)
            {
                builder.Append("--nologo");
            }

            if (settings.ResultsDirectory != null)
            {
                builder.Append("--results-directory");
                builder.AppendQuoted(settings.ResultsDirectory.MakeAbsolute(_environment).FullPath);
            }

            if (settings.VSTestReportPath != null)
            {
                builder.AppendSwitchQuoted($"--logger trx;LogFileName", "=", settings.VSTestReportPath.MakeAbsolute(_environment).FullPath);
            }

            if (!string.IsNullOrEmpty(settings.Runtime))
            {
                builder.Append("--runtime");
                builder.Append(settings.Runtime);
            }

            if (!arguments.IsNullOrEmpty())
            {
                builder.Append("--");
                arguments.CopyTo(builder);
            }

            return builder;
        }
    }
}
