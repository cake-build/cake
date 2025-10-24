// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tools.DotNet.MSBuild;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Test
{
    /// <summary>
    /// .NET project tester.
    /// </summary>
    public sealed class DotNetTester : DotNetTool<DotNetTestSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetTester" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetTester(
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
        public void Test(string project, ProcessArgumentBuilder arguments, DotNetTestSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

            RunCommand(settings, GetArguments(project, arguments, settings));
        }

        private ProcessArgumentBuilder GetArguments(string project, ProcessArgumentBuilder arguments, DotNetTestSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("test");

            // Specific path?
            if (project != null)
            {
                // Handle path type for .NET 10+ compatibility
                switch (DeterminePathType(project, settings.PathType))
                {
                    case DotNetTestPathType.Project:
                        builder.Append("--project");
                        break;
                    case DotNetTestPathType.Solution:
                        builder.Append("--solution");
                        break;
                }

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

            // Sources
            if (settings.Sources != null)
            {
                foreach (var source in settings.Sources)
                {
                    builder.Append("--source");
                    builder.AppendQuoted(source);
                }
            }

            // Blame
            if (settings.Blame)
            {
                builder.Append("--blame");
            }

            if (settings.MSBuildSettings != null)
            {
                builder.AppendMSBuildSettings(settings.MSBuildSettings, _environment);
            }

            if (!arguments.IsNullOrEmpty())
            {
                builder.Append("--");
                arguments.CopyTo(builder);
            }

            return builder;
        }

        /// <summary>
        /// Determines the path type based on the project path and settings.
        /// </summary>
        /// <param name="project">The project path.</param>
        /// <param name="pathType">The configured path type.</param>
        /// <returns>The determined path type.</returns>
        private static DotNetTestPathType DeterminePathType(string project, DotNetTestPathType pathType)
        {
            return pathType switch
            {
                DotNetTestPathType.None => DotNetTestPathType.None,
                DotNetTestPathType.Project => DotNetTestPathType.Project,
                DotNetTestPathType.Solution => DotNetTestPathType.Solution,
                DotNetTestPathType.Auto => AutoDetectPathType(project),
                _ => DotNetTestPathType.None
            };
        }

        /// <summary>
        /// Auto-detects the path type based on file extension.
        /// </summary>
        /// <param name="project">The project path.</param>
        /// <returns>The detected path type.</returns>
        private static DotNetTestPathType AutoDetectPathType(string project)
        {
            var extension = FilePath.FromString(project).GetExtension().ToLowerInvariant();
            return extension switch
            {
                ".sln" => DotNetTestPathType.Solution,
                ".csproj" or ".vbproj" or ".fsproj" or ".vcxproj" => DotNetTestPathType.Project,
                _ => DotNetTestPathType.None // Default to legacy behavior for unknown extensions
            };
        }
    }
}
