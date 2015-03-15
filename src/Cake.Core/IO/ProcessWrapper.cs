using System;
using System.Collections.Generic;
using System.Diagnostics;
using Cake.Core.Diagnostics;

namespace Cake.Core.IO
{
    internal sealed class ProcessWrapper : IProcess
    {
        private readonly Process _process;
        private readonly ICakeLog _log;
        private readonly Func<string, string> _filterOutput;

        public ProcessWrapper(Process process, ICakeLog log, Func<string, string> filterOutput)
        {
            _process = process;
            _log = log;
            _filterOutput = filterOutput ?? (source => "[REDACTED]");
        }

        public void WaitForExit()
        {
            _process.WaitForExit();
        }

        public bool WaitForExit(int milliseconds)
        {
            if (_process.WaitForExit(milliseconds))
            {
                return true;
            }
            _process.Refresh();
            if (!_process.HasExited)
            {
                _process.Kill();
            }
            return false;
        }

        public int GetExitCode()
        {
            return _process.ExitCode;
        }

        public IEnumerable<string> GetStandardOutput()
        {
            string line;
            while ((line = _process.StandardOutput.ReadLine()) != null)
            {
                _log.Debug("{0}", _filterOutput(line));
                yield return line;
            }
        }
    }
}