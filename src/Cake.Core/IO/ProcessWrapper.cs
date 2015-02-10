using System.Collections.Generic;
using System.Diagnostics;
using Cake.Core.Diagnostics;

namespace Cake.Core.IO
{
    internal sealed class ProcessWrapper : IProcess
    {
        private readonly Process _process;
        private readonly ICakeLog _log;

        public ProcessWrapper(Process process, ICakeLog log)
        {
            _process = process;
            _log = log;
        }

        public void WaitForExit()
        {
            _process.WaitForExit();
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
                _log.Verbose("{0}", line);
                yield return line;
            }
        }
    }
}