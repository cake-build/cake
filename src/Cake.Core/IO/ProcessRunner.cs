// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.Polyfill;
using Cake.Core.Tooling;

namespace Cake.Core.IO
{
    /// <summary>
    /// Responsible for starting processes.
    /// </summary>
    public sealed class ProcessRunner : IProcessRunner
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly IToolLocator _tools;
        private readonly ICakeConfiguration _configuration;
        private readonly bool _noMonoCoersion;
        private readonly bool _showCommandLine;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessRunner" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="configuration">The tool configuration.</param>
        public ProcessRunner(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log, IToolLocator tools, ICakeConfiguration configuration)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _log = log ?? throw new ArgumentNullException(nameof(log));
            _tools = tools ?? throw new ArgumentNullException(nameof(tools));
            _configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));

            var noMonoCoersion = configuration.GetValue(Constants.Settings.NoMonoCoersion);
            _noMonoCoersion = noMonoCoersion != null && noMonoCoersion.Equals("true", StringComparison.OrdinalIgnoreCase);
            var showCommandLine = configuration.GetValue(Constants.Settings.ShowProcessCommandLine);
            _showCommandLine = showCommandLine != null && showCommandLine.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        /// <inheritdoc/>
        public IProcess Start(FilePath filePath, ProcessSettings settings)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            ProcessStartInfo info = GetProcessStartInfo(filePath, settings, out Func<string, string> filterUnsafe);

            // The process we are about to execute can modify the Console Mode and disable features such as ANSI support.
            // We're capturing the current Console Mode (if any) here so that we can reset it to its original value later.
            // See https://github.com/cake-build/cake/issues/3018 for an example.
            var consoleMode = ConsoleMode.GetCurrent(_environment);

            // Start and return the process.
            var process = new Process
            {
                StartInfo = info,
                EnableRaisingEvents = true
            };

            process.Exited += (_, __) => consoleMode.Reset();

            process.Start();

            var processWrapper = new ProcessWrapper(process, _log, filterUnsafe, settings.RedirectedStandardOutputHandler,
                filterUnsafe, settings.RedirectedStandardErrorHandler);

            if (settings.RedirectStandardOutput)
            {
                SubscribeStandardOutput(process, processWrapper);
            }
            if (settings.RedirectStandardError)
            {
                SubscribeStandardError(process, processWrapper);
            }

            return processWrapper;
        }

        internal ProcessStartInfo GetProcessStartInfo(FilePath filePath, ProcessSettings settings, out Func<string, string> filterUnsafe)
        {
            // Get the fileName
            var fileName = _environment.Platform.IsUnix() ? filePath.FullPath : filePath.FullPath.Quote();

            // Get the arguments.
            var arguments = settings.Arguments ?? new ProcessArgumentBuilder();
            filterUnsafe = arguments.FilterUnsafe;

            if (!_noMonoCoersion &&
                _environment.Platform.IsUnix() &&
                _environment.Runtime.IsCoreClr &&
                fileName.EndsWith(".exe", StringComparison.OrdinalIgnoreCase) &&
                _fileSystem.GetFile(fileName).IsClrAssembly())
            {
                FilePath monoPath = _tools.Resolve("mono");
                if (monoPath != null)
                {
                    if (!settings.Silent)
                    {
                        _log.Verbose(Verbosity.Diagnostic, "{0} is a .NET Framework executable, will try execute using Mono.", fileName);
                    }
                    arguments.PrependQuoted(fileName);
                    fileName = monoPath.FullPath;
                }
                else
                {
                    if (!settings.Silent)
                    {
                        _log.Verbose(Verbosity.Diagnostic, "{0} is a .NET Framework executable, you might need to install Mono for it to execute successfully.", fileName);
                    }
                }
            }

            if (!settings.Silent)
            {
                // Log the filename and arguments.
                var message = string.Concat(fileName, " ", arguments.RenderSafe().TrimEnd());
                _log.Verbose(_showCommandLine ? _log.Verbosity : Verbosity.Diagnostic, "Executing: {0}", message);
            }

            // Create the process start info.
            var info = new ProcessStartInfo(fileName)
            {
                Arguments = arguments.Render(),
                UseShellExecute = false,
                RedirectStandardError = settings.RedirectStandardError,
                RedirectStandardOutput = settings.RedirectStandardOutput
            };

            // Allow working directory?
            if (!settings.NoWorkingDirectory)
            {
                var workingDirectory = settings.WorkingDirectory ?? _environment.WorkingDirectory;
                info.WorkingDirectory = workingDirectory.MakeAbsolute(_environment).FullPath;
            }

            // Add environment variables
            ProcessHelper.SetEnvironmentVariable(info, "CAKE", "True");
            ProcessHelper.SetEnvironmentVariable(info, "CAKE_VERSION", _environment.Runtime.CakeVersion.ToString(3));
            if (settings.EnvironmentVariables != null)
            {
                foreach (var environmentVariable in settings.EnvironmentVariables)
                {
                    ProcessHelper.SetEnvironmentVariable(info, environmentVariable.Key, environmentVariable.Value);
                }
            }

            return info;
        }

        private static void SubscribeStandardError(Process process, ProcessWrapper processWrapper)
        {
            process.ErrorDataReceived += (s, e) =>
            {
                processWrapper.StandardErrorReceived(e.Data);
            };
            process.BeginErrorReadLine();
        }

        private static void SubscribeStandardOutput(Process process, ProcessWrapper processWrapper)
        {
            process.OutputDataReceived += (s, e) =>
            {
                processWrapper.StandardOutputReceived(e.Data);
            };
            process.BeginOutputReadLine();
        }
    }
}