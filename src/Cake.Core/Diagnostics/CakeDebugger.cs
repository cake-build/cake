using System;
using System.Diagnostics;
using System.Threading;
using System.Threading.Tasks;

namespace Cake.Core.Diagnostics
{
    internal sealed class CakeDebugger : ICakeDebugger
    {
        private readonly ICakeLog _log;

        public CakeDebugger(ICakeLog log)
        {
            _log = log;
        }

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
