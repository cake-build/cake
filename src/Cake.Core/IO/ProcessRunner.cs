using System;
using System.Diagnostics;
using Cake.Core.Diagnostics;

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
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _environment = environment;
            _log = log;
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
                throw new ArgumentNullException("filePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            // Get the arguments.
            var arguments = settings.Arguments ?? new ProcessArgumentBuilder();
            
            // Log the filename and arguments.
            var message = string.Concat(filePath, " ", arguments.RenderSafe().TrimEnd());
            _log.Verbose(Verbosity.Diagnostic, "Executing: {0}", message);

            // Get the working directory.
            var workingDirectory = settings.WorkingDirectory ?? _environment.WorkingDirectory;
            settings.WorkingDirectory = workingDirectory.MakeAbsolute(_environment);

            // Create the process start info.
            var info = new ProcessStartInfo(filePath.FullPath)
            {
                Arguments = arguments.Render(),
                WorkingDirectory = workingDirectory.FullPath,
                UseShellExecute = false,
                RedirectStandardOutput = settings.RedirectStandardOutput
            };

            // Start and return the process.
            var process = Process.Start(info);
            return process == null ? null : new ProcessWrapper(process, _log);
        }
    }
}
