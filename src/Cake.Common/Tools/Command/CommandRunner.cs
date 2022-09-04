// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Command
{
    /// <summary>
    /// The generic command runner.
    /// </summary>
    public sealed class CommandRunner : Tool<CommandSettings>
    {
        private CommandSettings Settings { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommandRunner"/> class.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="tools">The globber.</param>
        public CommandRunner(
            CommandSettings settings,
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IToolLocator tools) : base(fileSystem, environment, processRunner, tools)
        {
            Settings = settings ?? throw new ArgumentNullException(nameof(settings));
        }

        /// <inheritdoc/>
        protected override IEnumerable<string> GetToolExecutableNames()
            => Settings.ToolExecutableNames;

        /// <inheritdoc/>
        protected override string GetToolName()
            => Settings.ToolName;

        /// <summary>
        /// Runs the command using the specified settings.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        public void RunCommand(ProcessArgumentBuilder arguments)
            => RunCommand(arguments, null, null);

        /// <summary>
        /// Runs the command using the specified settings.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <returns>The exit code.</returns>
        public int RunCommand(ProcessArgumentBuilder arguments, out string standardOutput)
        => RunCommand(
                arguments,
                out standardOutput,
                out _,
                new ProcessSettings
                {
                    RedirectStandardOutput = true
                });

        /// <summary>
        /// Runs the command using the specified settings.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="standardOutput">The standard output.</param>
        /// <param name="standardError">The standard error output.</param>
        /// <returns>The exit code.</returns>
        public int RunCommand(ProcessArgumentBuilder arguments, out string standardOutput, out string standardError)
            => RunCommand(
                arguments,
                out standardOutput,
                out standardError,
                new ProcessSettings
                {
                    RedirectStandardOutput = true,
                    RedirectStandardError = true
                });

        private int RunCommand(ProcessArgumentBuilder arguments, out string standardOutput, out string standardError, ProcessSettings processSettings)
        {
            IEnumerable<string>
                standardOutputResult = null, standardErrorResult = null;
            int returnCode = -1;

            RunCommand(arguments, processSettings,
                process =>
                {
                    standardOutputResult = processSettings.RedirectStandardOutput ? process.GetStandardOutput() : Array.Empty<string>();
                    standardErrorResult = processSettings.RedirectStandardError ? process.GetStandardError() : Array.Empty<string>();
                    returnCode = process.GetExitCode();
                });

            standardOutput = string.Join(Environment.NewLine, standardOutputResult);
            standardError = string.Join(Environment.NewLine, standardErrorResult);

            return returnCode;
        }

        /// <summary>
        /// Runs the command using the specified settings.
        /// </summary>
        /// <param name="arguments">The arguments.</param>
        /// <param name="processSettings">The process settings.</param>
        /// <param name="postAction">If specified called after process exit.</param>
        private void RunCommand(ProcessArgumentBuilder arguments, ProcessSettings processSettings, Action<IProcess> postAction)
        {
            if (arguments is null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (string.IsNullOrWhiteSpace(Settings.ToolName))
            {
                throw new ArgumentNullException(nameof(Settings.ToolName));
            }

            if (Settings.ToolExecutableNames == null || !Settings.ToolExecutableNames.Any())
            {
                throw new ArgumentNullException(nameof(Settings.ToolExecutableNames));
            }

            Run(Settings, arguments, processSettings, postAction);
        }
    }
}
