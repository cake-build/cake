using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// The Cake debugger.
    /// </summary>
    public sealed class CakeDebugger : ICakeDebugger
    {
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeDebugger"/> class.
        /// </summary>
        /// <param name="log">The log.</param>
        public CakeDebugger(ICakeLog log)
        {
            _log = log;
        }

        /// <summary>
        /// Waits for a debugger to attach.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        public void WaitForAttach(TimeSpan timeout)
        {
            var processId = Process.GetCurrentProcess().Id;
            _log.Information(Verbosity.Quiet, "Attach debugger to process {0} to continue...", processId);

            Task.Run(() =>
            {
                while (!Debugger.IsAttached)
                {
                    Thread.Sleep(100);
                }
            }).Wait(Timeout.InfiniteTimeSpan);

            _log.Debug("Debugger attached.");
        }
    }
}
