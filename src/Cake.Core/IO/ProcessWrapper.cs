using System.Collections.Generic;
using System.Diagnostics;

namespace Cake.Core.IO
{
    internal sealed class ProcessWrapper : IProcess
    {
        private readonly Process _process;

        public ProcessWrapper(Process process)
        {
            _process = process;
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
            while ((line=_process.StandardOutput.ReadLine())!=null)
            {
                yield return line;
            }
        }
    }
}