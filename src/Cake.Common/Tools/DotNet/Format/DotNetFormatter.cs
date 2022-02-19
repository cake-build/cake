// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNet.Format
{
    /// <summary>
    /// .NET project formatter.
    /// </summary>
    public sealed class DotNetFormatter : DotNetTool<DotNetFormatSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetFormatter" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetFormatter(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Format the project or solution using the specified path and settings.
        /// </summary>
        /// <param name="root">The target project or solution path.</param>
        /// <param name="subcommand">The sub command.</param>
        /// <param name="settings">The settings.</param>
        public void Format(string root, string subcommand, DotNetFormatSettings settings)
        {
            if (root == null)
            {
                throw new ArgumentNullException(nameof(root));
            }

            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetArguments(root, subcommand, settings));
        }

        private ProcessArgumentBuilder GetArguments(string root, string subcommand, DotNetFormatSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("format");

            // Subcommand
            if (!string.IsNullOrWhiteSpace(subcommand))
            {
                builder.Append(subcommand);
            }

            // Specific path?
            if (root != null)
            {
                builder.AppendQuoted(root);
            }

            // Diagnostics
            if (settings.Diagnostics != null && settings.Diagnostics.Any())
            {
                builder.AppendSwitch("--diagnostics", string.Join(" ", settings.Diagnostics));
            }

            // Severity
            if (settings.Severity.HasValue)
            {
                builder.Append("--severity");
                builder.Append(GetSeverityValue(settings.Severity.Value));
            }

            // No Restore
            if (settings.NoRestore)
            {
                builder.Append("--no-restore");
            }

            // Verify No Changes
            if (settings.VerifyNoChanges)
            {
                builder.Append("--verify-no-changes");
            }

            // Include
            if (settings.Include != null && settings.Include.Any())
            {
                builder.AppendSwitch("--include", string.Join(" ", settings.Include));
            }

            // Exclude
            if (settings.Exclude != null && settings.Exclude.Any())
            {
                builder.AppendSwitch("--exclude", string.Join(" ", settings.Exclude));
            }

            // Include Generated
            if (settings.IncludeGenerated)
            {
                builder.Append("--include-generated");
            }

            // Binary Log
            if (settings.BinaryLog != null)
            {
                builder.AppendSwitchQuoted($"--binarylog", settings.BinaryLog.MakeAbsolute(_environment).FullPath);
            }

            // Report
            if (settings.Report != null)
            {
                builder.AppendSwitchQuoted($"--report", settings.Report.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }

        private static string GetSeverityValue(DotNetFormatSeverity value)
        {
            return value switch
            {
                DotNetFormatSeverity.Info => "info",
                DotNetFormatSeverity.Warning => "warn",
                DotNetFormatSeverity.Error => "error",
                _ => throw new InvalidOperationException($"Unknown severity value '{value}'"),
            };
        }
    }
}
