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

        private Action<ProcessExitedEventArgs> _processExitedCallback;
        private Action<ProcessOutputReceivedEventArgs> _standardErrorReceivedCallback;
        private Action<ProcessOutputReceivedEventArgs> _standardOutputReceivedCallback;

        public ProcessWrapper(Process process, ICakeLog log, Func<string, string> filterOutput)
        {
            _log = log;
            _filterOutput = filterOutput ?? (source => "[REDACTED]");
            _process = process;
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

            if (HasExited)
            {
                _process.Kill();
            }
            return false;
        }

        public int GetExitCode()
        {
            try
            {
                return _process.ExitCode;
            }
            catch (InvalidOperationException)
            {
                return -1;
            }
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

        public IEnumerable<string> GetStandardError()
        {
            string line;
            while ((line = _process.StandardError.ReadLine()) != null)
            {
                _log.Warning("{0}", _filterOutput(line));
                yield return line;
            }
        }

        public int ProcessId
        {
            get
            {
                return _process.Id;
            }
        }

        public void Kill()
        {
            if (!HasExited)
            {
                _process.Kill();
                _process.WaitForExit();
            }
        }

        public bool HasExited
        {
            get
            {
                _process.Refresh();
                return _process.HasExited;
            }
        }

        private void Process_Exited(object sender, EventArgs args)
        {
            if (_processExitedCallback != null)
            {
                _processExitedCallback(new ProcessExitedEventArgs(GetExitCode()));
            }
        }

        public void HandleExited(Action<ProcessExitedEventArgs> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (_processExitedCallback == null)
            {
                if (!_process.EnableRaisingEvents)
                {
                    _process.EnableRaisingEvents = true;
                }
                _process.Exited += Process_Exited;
            }

            _processExitedCallback = action;

            if (HasExited)
            {
                Process_Exited(_process, new EventArgs());
            }
        }

        public void HandleErrorOutput(Action<ProcessOutputReceivedEventArgs> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (_standardErrorReceivedCallback == null)
            {
                _process.ErrorDataReceived += Process_ErrorDataReceived;
                _process.BeginErrorReadLine();
            }

            _standardErrorReceivedCallback = action;
        }

        public void HandleStandardOutput(Action<ProcessOutputReceivedEventArgs> action)
        {
            if (action == null)
            {
                throw new ArgumentNullException("action");
            }

            if (_standardOutputReceivedCallback == null)
            {
                _process.OutputDataReceived += Process_OutputDataReceived;
                _process.BeginOutputReadLine();
            }

            _standardOutputReceivedCallback = action;
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && _standardErrorReceivedCallback != null)
            {
                _standardErrorReceivedCallback(new ProcessOutputReceivedEventArgs(e.Data));
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null && _standardOutputReceivedCallback != null)
            {
                _standardOutputReceivedCallback(new ProcessOutputReceivedEventArgs(e.Data));
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>This also closes the underlying process handle, but does not stop the process if it is still running.</remarks>
        public void Dispose()
        {
            _process.Exited -= Process_Exited;
            if (_process.StartInfo.RedirectStandardError && _standardErrorReceivedCallback != null)
            {
                _process.CancelErrorRead();
                _process.ErrorDataReceived -= Process_ErrorDataReceived;
            }
            if (_process.StartInfo.RedirectStandardOutput && _standardOutputReceivedCallback != null)
            {
                _process.CancelOutputRead();
                _process.OutputDataReceived -= Process_OutputDataReceived;
            }

            _processExitedCallback = null;
            _standardErrorReceivedCallback = null;
            _standardOutputReceivedCallback = null;

            _process.Dispose();
        }
    }
}