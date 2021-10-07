// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotNetCore.Format
{
    /// <summary>
    /// .NET Core project formatter.
    /// </summary>
    public sealed class DotNetCoreFormater : DotNetCoreTool<DotNetCoreFormatSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreFormater" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        public DotNetCoreFormater(
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
        /// <param name="project">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public void Format(string project, DotNetCoreFormatSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetFormatArguments(project, settings));
        }

        /// <summary>
        /// Format the project or solution code to match editorconfig settings for whitespace using the specified path and settings.
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public void Whitespace(string project, DotNetCoreFormatSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetWhitespaceArguments(project, settings));
        }

        /// <summary>
        /// Format the project or solution code to match editorconfig settings for code style using the specified path and settings.
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public void Style(string project, DotNetCoreFormatSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetStyleArguments(project, settings));
        }

        /// <summary>
        /// Format the project or solution code to match editorconfig settings for analyzers using the specified path and settings.
        /// </summary>
        /// <param name="project">The target project path.</param>
        /// <param name="settings">The settings.</param>
        public void Analyzers(string project, DotNetCoreFormatSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetAnalyzersArguments(project, settings));
        }

        private ProcessArgumentBuilder GetFormatArguments(string path, DotNetCoreFormatSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("format");

            // Specific path?
            if (path != null)
            {
                builder.AppendQuoted(path);
            }

            // Diagnostics
            if (settings.Diagnostics != null)
            {
                builder.AppendSwitch("--diagnostics", ":", string.Join(" ", settings.Diagnostics));
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
            if (settings.Include != null)
            {
                builder.AppendSwitch("--include", ":", string.Join(" ", settings.Include));
            }

            // Exclude
            if (settings.Exclude != null)
            {
                builder.AppendSwitch("--exclude", ":", string.Join(" ", settings.Exclude));
            }

            // Include Generated
            if (settings.IncludeGenerated)
            {
                builder.Append("--include-generated");
            }

            // Binary Log
            if (settings.BinaryLog != null)
            {
                builder.AppendSwitch("--binarylog", ":", string.Join(" ", settings.BinaryLog));
            }

            // Report
            if (settings.Report != null)
            {
                builder.AppendSwitchQuoted($"--report", "=", settings.Report.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }

        private ProcessArgumentBuilder GetWhitespaceArguments(string path, DotNetCoreFormatSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("format");

            // Specific path?
            if (path != null)
            {
                builder.AppendQuoted(path);
            }

            builder.Append("whitespace");

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
            if (settings.Include != null)
            {
                builder.AppendSwitch("--include", ":", string.Join(" ", settings.Include));
            }

            // Exclude
            if (settings.Exclude != null)
            {
                builder.AppendSwitch("--exclude", ":", string.Join(" ", settings.Exclude));
            }

            // Include Generated
            if (settings.IncludeGenerated)
            {
                builder.Append("--include-generated");
            }

            // Binary Log
            if (settings.BinaryLog != null)
            {
                builder.AppendSwitch("--binarylog", ":", string.Join(" ", settings.BinaryLog));
            }

            // Report
            if (settings.Report != null)
            {
                builder.AppendSwitchQuoted($"--report", "=", settings.Report.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }

        private ProcessArgumentBuilder GetStyleArguments(string path, DotNetCoreFormatSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("format");

            // Specific path?
            if (path != null)
            {
                builder.AppendQuoted(path);
            }

            builder.Append("style");

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
            if (settings.Include != null)
            {
                builder.AppendSwitch("--include", ":", string.Join(" ", settings.Include));
            }

            // Exclude
            if (settings.Exclude != null)
            {
                builder.AppendSwitch("--exclude", ":", string.Join(" ", settings.Exclude));
            }

            // Include Generated
            if (settings.IncludeGenerated)
            {
                builder.Append("--include-generated");
            }

            // Binary Log
            if (settings.BinaryLog != null)
            {
                builder.AppendSwitch("--binarylog", ":", string.Join(" ", settings.BinaryLog));
            }

            // Report
            if (settings.Report != null)
            {
                builder.AppendSwitchQuoted($"--report", "=", settings.Report.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }

        private ProcessArgumentBuilder GetAnalyzersArguments(string path, DotNetCoreFormatSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("format");

            // Specific path?
            if (path != null)
            {
                builder.AppendQuoted(path);
            }

            builder.Append("analzers");

            // Diagnostics
            if (settings.Diagnostics != null)
            {
                builder.AppendSwitch("--diagnostics", ":", string.Join(" ", settings.Diagnostics));
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
            if (settings.Include != null)
            {
                builder.AppendSwitch("--include", ":", string.Join(" ", settings.Include));
            }

            // Exclude
            if (settings.Exclude != null)
            {
                builder.AppendSwitch("--exclude", ":", string.Join(" ", settings.Exclude));
            }

            // Include Generated
            if (settings.IncludeGenerated)
            {
                builder.Append("--include-generated");
            }

            // Binary Log
            if (settings.BinaryLog != null)
            {
                builder.AppendSwitch("--binarylog", ":", string.Join(" ", settings.BinaryLog));
            }

            // Report
            if (settings.Report != null)
            {
                builder.AppendSwitchQuoted($"--report", "=", settings.Report.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }
    }
}
