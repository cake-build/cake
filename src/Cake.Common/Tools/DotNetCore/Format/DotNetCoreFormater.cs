// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Common.Tools.DotNetCore.Format
{
    public sealed class DotNetCoreFormater : DotNetCoreTool<DotNetCoreFormatSettings>
    {
        private readonly ICakeEnvironment _environment;

        public DotNetCoreFormater(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            _environment = environment;
        }

        public void Format(string project, DotNetCoreFormatSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetFormatArguments(project, settings));
        }

        public void Whitespace(string project, DotNetCoreFormatSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetWhitespaceArguments(project, settings));
        }

        public void Style(string project, DotNetCoreFormatSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            RunCommand(settings, GetStyleArguments(project, settings));
        }

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
                foreach (var diagnostic in settings.Diagnostics)
                {
                    builder.Append("--diagnostics");
                    builder.AppendSwitch(" ", diagnostic); ///////
                }
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
                foreach (var include in settings.Include)
                {
                    builder.Append("--include");
                    builder.AppendSwitch(" ", include); ///////
                }
            }

            // Exclude
            if (settings.Exclude != null)
            {
                foreach (var exclude in settings.Exclude)
                {
                    builder.Append("--exclude");
                    builder.AppendSwitch(" ", exclude); ///////
                }
            }

            // Include Generated
            if (settings.IncludeGenerated)
            {
                builder.Append("--include-generated");
            }

            // Binary Log
            if (settings.BinaryLog != null)
            {
                foreach (var binaryLog in settings.BinaryLog)
                {
                    builder.Append("--binarylog");
                    builder.AppendSwitch(" ", binaryLog); ///////
                }
            }

            // Report
            if (!string.IsNullOrWhiteSpace(settings.Report))
            {
                builder.Append("--report");
                builder.AppendQuoted(settings.Report);
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
                foreach (var source in settings.Include)
                {
                    builder.Append("--include");
                    builder.AppendQuoted(source); ///////
                }
            }

            // Exclude
            if (settings.Exclude != null)
            {
                foreach (var source in settings.Exclude)
                {
                    builder.Append("--exclude");
                    builder.AppendQuoted(source); ///////
                }
            }

            // Include Generated
            if (settings.IncludeGenerated)
            {
                builder.Append("--include-generated");
            }

            // Binary Log
            if (settings.BinaryLog != null)
            {
                foreach (var source in settings.BinaryLog)
                {
                    builder.Append("--binarylog");
                    builder.AppendQuoted(source); ///////
                }
            }

            // Report
            if (!string.IsNullOrWhiteSpace(settings.Report))
            {
                builder.Append("--report");
                builder.AppendQuoted(settings.Report);
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

            // Diagnostics
            //if (settings.Diagnostics != null)
            //{
            //    foreach (var source in settings.Diagnostics)
            //    {
            //        builder.Append("--diagnostics");
            //        builder.AppendQuoted(source); ///////
            //    }
            //}

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
                foreach (var source in settings.Include)
                {
                    builder.Append("--include");
                    builder.AppendQuoted(source); ///////
                }
            }

            // Exclude
            if (settings.Exclude != null)
            {
                foreach (var source in settings.Exclude)
                {
                    builder.Append("--exclude");
                    builder.AppendQuoted(source); ///////
                }
            }

            // Include Generated
            if (settings.IncludeGenerated)
            {
                builder.Append("--include-generated");
            }

            // Binary Log
            if (settings.BinaryLog != null)
            {
                foreach (var source in settings.BinaryLog)
                {
                    builder.Append("--binarylog");
                    builder.AppendQuoted(source); ///////
                }
            }

            // Report
            if (!string.IsNullOrWhiteSpace(settings.Report))
            {
                builder.Append("--report");
                builder.AppendQuoted(settings.Report);
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
                foreach (var source in settings.Diagnostics)
                {
                    builder.Append("--diagnostics");
                    builder.AppendQuoted(source); ///////
                }
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
                foreach (var source in settings.Include)
                {
                    builder.Append("--include");
                    builder.AppendQuoted(source); ///////
                }
            }

            // Exclude
            if (settings.Exclude != null)
            {
                foreach (var source in settings.Exclude)
                {
                    builder.Append("--exclude");
                    builder.AppendQuoted(source); ///////
                }
            }

            // Include Generated
            if (settings.IncludeGenerated)
            {
                builder.Append("--include-generated");
            }

            // Binary Log
            if (settings.BinaryLog != null)
            {
                foreach (var source in settings.BinaryLog)
                {
                    builder.Append("--binarylog");
                    builder.AppendQuoted(source); ///////
                }
            }

            // Report
            if (!string.IsNullOrWhiteSpace(settings.Report))
            {
                builder.Append("--report");
                builder.AppendQuoted(settings.Report);
            }

            return builder;
        }
    }
}
