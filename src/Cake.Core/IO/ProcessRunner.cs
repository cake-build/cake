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
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessRunner"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public ProcessRunner(ICakeLog log)
        {
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _log = log;
        }

        /// <summary>
        /// Starts a process using the specified information.
        /// </summary>
        /// <param name="info">The information about the process to start.</param>
        /// <returns>A process handle.</returns>
        public IProcess Start(ProcessStartInfo info)
        {
            if (info == null)
            {
                throw new ArgumentNullException("info");
            }
            if (string.IsNullOrWhiteSpace(info.FileName))
            {
                throw new CakeException("Cannot start process since no filename has been set.");
            }
            
            // Log the filename and arguments.
            _log.Verbose(Verbosity.Diagnostic, "Executing: {0}", string.Concat(info.FileName, " ", info.Arguments).Trim());

            // Start the process.
            var process = Process.Start(info);
            return process == null ? null : new ProcessWrapper(process);
        }
    }
}
