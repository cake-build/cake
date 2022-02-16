// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization.Json;
using System.Text;
using System.Text.RegularExpressions;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// The GitVersion runner.
    /// </summary>
    public sealed class GitVersionRunner : Tool<GitVersionSettings>
    {
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitVersionRunner"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="log">The log.</param>
        public GitVersionRunner(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools,
            ICakeLog log) : base(fileSystem, environment, processRunner, tools)
        {
            _log = log;
        }

        /// <summary>
        /// Runs GitVersion and processes the results.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>A task with the GitVersion results.</returns>
        public GitVersion Run(GitVersionSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            if (settings.OutputType != GitVersionOutput.BuildServer)
            {
                var output = string.Empty;
                Run(settings, GetArguments(settings), new ProcessSettings { RedirectStandardOutput = true }, process =>
                {
                    output = string.Join("\n", process.GetStandardOutput());
                    if (_log.Verbosity < Verbosity.Diagnostic)
                    {
                        var errors = Regex.Matches(output, @"( *ERROR:? [^\n]*)\n([^\n]*)").Cast<Match>()
                            .SelectMany(match => new[] { match.Groups[1].Value, match.Groups[2].Value });
                        foreach (var error in errors)
                        {
                            _log.Error(error);
                        }
                    }
                });

                var jsonSerializer = new DataContractJsonSerializer(typeof(GitVersionInternal));
                using (var jsonStream = new MemoryStream(Encoding.UTF8.GetBytes(output)))
                {
                    return (jsonSerializer.ReadObject(jsonStream) as GitVersionInternal)?.GitVersion;
                }
            }

            Run(settings, GetArguments(settings));

            return new GitVersion();
        }

        private ProcessArgumentBuilder GetArguments(GitVersionSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            if (settings.OutputType.HasValue)
            {
                switch (settings.OutputType.Value)
                {
                    case GitVersionOutput.Json:
                        builder.Append("-output");
                        builder.Append("json");
                        break;
                    case GitVersionOutput.BuildServer:
                        builder.Append("-output");
                        builder.Append("buildserver");
                        break;
                }
            }

            if (!string.IsNullOrWhiteSpace(settings.ShowVariable))
            {
                builder.Append("-showvariable");
                builder.Append(settings.ShowVariable);
            }

            if (!string.IsNullOrWhiteSpace(settings.UserName))
            {
                builder.Append("-u");
                builder.AppendQuoted(settings.UserName);

                builder.Append("-p");
                builder.AppendQuotedSecret(settings.Password);
            }

            if (settings.UpdateAssemblyInfo)
            {
                builder.Append("-updateassemblyinfo");

                if (settings.UpdateAssemblyInfoFilePath != null)
                {
                    builder.AppendQuoted(settings.UpdateAssemblyInfoFilePath.FullPath);
                }
            }

            if (settings.RepositoryPath != null)
            {
                builder.Append("-targetpath");
                builder.AppendQuoted(settings.RepositoryPath.FullPath);
            }
            else if (!string.IsNullOrWhiteSpace(settings.Url))
            {
                builder.Append("-url");
                builder.AppendQuoted(settings.Url);

                if (!string.IsNullOrWhiteSpace(settings.Branch))
                {
                    builder.Append("-b");
                    builder.Append(settings.Branch);
                }
                else
                {
                    _log.Warning("If you leave the branch name for GitVersion unset, it will fallback to the default branch for the repository.");
                }

                if (!string.IsNullOrWhiteSpace(settings.Commit))
                {
                    builder.Append("-c");
                    builder.AppendQuoted(settings.Commit);
                }

                if (settings.DynamicRepositoryPath != null)
                {
                    builder.Append("-dynamicRepoLocation");
                    builder.AppendQuoted(settings.DynamicRepositoryPath.FullPath);
                }
            }

            if (settings.LogFilePath != null)
            {
                builder.Append("-l");
                builder.AppendQuoted(settings.LogFilePath.FullPath);
            }

            if (settings.NoFetch)
            {
                builder.Append("-nofetch");
            }

            if (settings.Verbosity.HasValue)
            {
                switch (settings.Verbosity.Value)
                {
                    case GitVersionVerbosity.Quiet:
                        builder.Append("-verbosity");
                        builder.Append(nameof(Verbosity.Quiet));
                        break;
                    case GitVersionVerbosity.Diagnostic:
                        builder.Append("-verbosity");
                        builder.Append(nameof(Verbosity.Diagnostic));
                        break;
                    case GitVersionVerbosity.Verbose:
                        builder.Append("-verbosity");
                        builder.Append(nameof(Verbosity.Verbose));
                        break;
                    case GitVersionVerbosity.Normal:
                        builder.Append("-verbosity");
                        builder.Append(nameof(Verbosity.Normal));
                        break;
                    case GitVersionVerbosity.Minimal:
                        builder.Append("-verbosity");
                        builder.Append(nameof(Verbosity.Minimal));
                        break;
                }
            }
            else
            {
                switch (_log.Verbosity)
                {
                    case Verbosity.Quiet:
                        builder.Append("-verbosity");
                        builder.Append(nameof(Verbosity.Quiet));
                        break;
                    case Verbosity.Diagnostic:
                        builder.Append("-verbosity");
                        builder.Append(nameof(Verbosity.Diagnostic));
                        break;
                    case Verbosity.Verbose:
                        builder.Append("-verbosity");
                        builder.Append(nameof(Verbosity.Verbose));
                        break;
                    case Verbosity.Minimal:
                        builder.Append("-verbosity");
                        builder.Append(nameof(Verbosity.Minimal));
                        break;
                }
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return "GitVersion";
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "GitVersion.exe", "dotnet-gitversion", "dotnet-gitversion.exe" };
        }
    }
}
