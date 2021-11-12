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
    /// .NET Core project formatter.
    /// </summary>
    public sealed class DotNetFormater : DotNetTool<DotNetFormatSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetFormater" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetFormater(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        /// <summary>
        /// Format the project or solution code to match editorconfig settings using the specified path and settings.
        /// </summary>
        /// <param name="root">The target project path.</param>
        /// <param name="subcommand">The subcommand.</param>
        /// <param name="settings">The settings.</param>
        public void Format(string root, string subcommand, DotNetFormatSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetFormatArguments(root, subcommand, settings));
        }

        private ProcessArgumentBuilder GetFormatArguments(string root, string subcommand, DotNetFormatSettings settings)
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
                builder.Append(settings.Severity.ToString().ToLower());
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
    }
}
