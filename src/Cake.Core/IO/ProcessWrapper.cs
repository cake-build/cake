// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Concurrent;
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
        private readonly ConcurrentQueue<string> _consoleOutputQueue;

        public ProcessWrapper(Process process, ICakeLog log, Func<string, string> filterOutput, ConcurrentQueue<string> consoleOutputQueue)
        {
            _process = process;
            _log = log;
            _filterOutput = filterOutput ?? (source => "[REDACTED]");
            _consoleOutputQueue = consoleOutputQueue;
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
            if (_consoleOutputQueue == null)
            {
                yield break;
            }
            while (!_consoleOutputQueue.IsEmpty || !_process.HasExited)
            {
                string line;
                if (!_consoleOutputQueue.TryDequeue(out line))
                {
                    continue;
                }
                _log.Debug(log => log("{0}", _filterOutput(line)));
                yield return line;
            }
        }

        public void Kill()
        {
            _process.Kill();
            _process.WaitForExit();
        }

        public void Dispose()
        {
            _process.Dispose();
        }
    }
}
