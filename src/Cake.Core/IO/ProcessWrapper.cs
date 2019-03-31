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
        private readonly Func<string, string> _filterError;
        private readonly Func<string, string> _filterOutput;
        private readonly ConcurrentQueue<string> _consoleErrorQueue;
        private readonly ConcurrentQueue<string> _consoleOutputQueue;
        private readonly Func<string, string> _standardOutputHandler;
        private readonly Func<string, string> _standardErrorHandler;

        public ProcessWrapper(Process process, ICakeLog log, Func<string, string> filterOutput, Func<string, string> standardOutputHandler,
            Func<string, string> filterError, Func<string, string> standardErrorHandler)
        {
            _process = process;
            _log = log;
            _filterOutput = filterOutput ?? (source => "[REDACTED]");
            _consoleOutputQueue = new ConcurrentQueue<string>();
            _standardOutputHandler = standardOutputHandler ?? (output => output);
            _filterError = filterError ?? (source => "[REDACTED]");
            _consoleErrorQueue = new ConcurrentQueue<string>();
            _standardErrorHandler = standardErrorHandler ?? (output => output);
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

        internal void StandardErrorReceived(string standardError)
        {
            var redirectedError = _standardErrorHandler(standardError);

            if (redirectedError != null)
            {
                _consoleErrorQueue.Enqueue(redirectedError);
            }
        }

        public IEnumerable<string> GetStandardError()
        {
            while (!_consoleErrorQueue.IsEmpty || !_process.HasExited)
            {
                string line;
                if (!_consoleErrorQueue.TryDequeue(out line))
                {
                    continue;
                }
                _log.Debug(log => log("{0}", _filterError(line)));
                yield return line;
            }
        }

        internal void StandardOutputReceived(string standardOutput)
        {
            var redirectedOutput = _standardOutputHandler(standardOutput);

            if (redirectedOutput != null)
            {
                _consoleOutputQueue.Enqueue(redirectedOutput);
            }
        }

        public IEnumerable<string> GetStandardOutput()
        {
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