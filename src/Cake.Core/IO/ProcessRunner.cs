// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Concurrent;
using System.Diagnostics;
using Cake.Core.Diagnostics;
using Cake.Core.Polyfill;

namespace Cake.Core.IO
{
    /// <summary>
    /// Responsible for starting processes.
    /// </summary>
    public sealed class ProcessRunner : IProcessRunner
    {
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessRunner" /> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public ProcessRunner(ICakeEnvironment environment, ICakeLog log)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        /// <summary>
        /// Starts a process using the specified information.
        /// </summary>
        /// <param name="filePath">The file name such as an application or document with which to start the process.</param>
        /// <param name="settings">The information about the process to start.</param>
        /// <returns>A process handle.</returns>
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

            // Get the fileName
            var fileName = _environment.Platform.IsUnix() ? filePath.FullPath : filePath.FullPath.Quote();

            // Get the arguments.
            var arguments = settings.Arguments ?? new ProcessArgumentBuilder();

            if (!settings.Silent)
            {
                // Log the filename and arguments.
                var message = string.Concat(fileName, " ", arguments.RenderSafe().TrimEnd());
                _log.Verbose(Verbosity.Diagnostic, "Executing: {0}", message);
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

            // Start and return the process.
            var process = Process.Start(info);
            if (process == null)
            {
                return null;
            }

            var consoleOutputQueue = settings.RedirectStandardOutput
                ? SubscribeStandardConsoleOutputQueue(process)
                : null;

            var consoleErrorQueue = settings.RedirectStandardError
                ? SubscribeStandardConsoleErrorQueue(process)
                : null;

            return new ProcessWrapper(process, _log, arguments.FilterUnsafe,
                consoleOutputQueue, arguments.FilterUnsafe, consoleErrorQueue);
        }

        private static ConcurrentQueue<string> SubscribeStandardConsoleErrorQueue(Process process)
        {
            var consoleErrorQueue = new ConcurrentQueue<string>();
            process.ErrorDataReceived += (s, e) =>
            {
                if (e.Data != null)
                {
                    consoleErrorQueue.Enqueue(e.Data);
                }
            };
            process.BeginErrorReadLine();
            return consoleErrorQueue;
        }

        private static ConcurrentQueue<string> SubscribeStandardConsoleOutputQueue(Process process)
        {
            var consoleOutputQueue = new ConcurrentQueue<string>();
            process.OutputDataReceived += (s, e) =>
            {
                if (e.Data != null)
                {
                    consoleOutputQueue.Enqueue(e.Data);
                }
            };
            process.BeginOutputReadLine();
            return consoleOutputQueue;
        }
    }
}