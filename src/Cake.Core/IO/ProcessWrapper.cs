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

        private event EventHandler<ProcessExitedEventArgs> ProcessExited;

        private event EventHandler<ProcessDataReceivedEventArgs> ProcessOutputDataReceived;

        private event EventHandler<ProcessDataReceivedEventArgs> ProcessErrorDataReceived;

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

        event EventHandler<ProcessExitedEventArgs> IProcess.Exited
        {
            add
            {
                if (ProcessExited == null)
                {
                    if (!_process.EnableRaisingEvents)
                    {
                        _process.EnableRaisingEvents = true;
                    }
                    _process.Exited += Process_Exited;
                }

                ProcessExited += value;

                if (HasExited)
                {
                    Process_Exited(_process, new EventArgs());
                }
            }

            remove
            {
                ProcessExited -= value;
            }
        }

        private void Process_Exited(object sender, EventArgs args)
        {
            OnProcessExited(new ProcessExitedEventArgs(GetExitCode()));
        }

        private void OnProcessExited(ProcessExitedEventArgs e)
        {
            var handler = ProcessExited;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Occurs when an application writes to its redirected StandardError stream.
        /// </summary>
        /// <remarks>
        /// The ErrorDataReceived event indicates that the associated process has written to its redirected StandardError stream.
        /// The event only occurs during asynchronous read operations on StandardError. To start asynchronous read operations, 
        /// you must redirect the StandardError stream of a Process, add your event handler to the ErrorDataReceived event, 
        /// and call BeginErrorReadLine. Thereafter, the ErrorDataReceived event signals each time the process writes a 
        /// line to the redirected StandardError stream, until the process exits or calls CancelErrorRead.
        /// <note>The application that is processing the asynchronous output should call the WaitForExit method to ensure that the output buffer has been flushed.</note>
        /// </remarks>
        event EventHandler<ProcessDataReceivedEventArgs> IProcess.ErrorDataReceived
        {
            add
            {
                if (ProcessErrorDataReceived == null)
                {
                    _process.ErrorDataReceived += Process_ErrorDataReceived;
                    _process.BeginErrorReadLine();
                }

                ProcessErrorDataReceived += value;
            }

            remove
            {
                _process.CancelErrorRead();
                ProcessErrorDataReceived -= value;
            }
        }

        private void Process_ErrorDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                OnProcessErrorDataReceived(new ProcessDataReceivedEventArgs(e.Data));
            }
        }

        private void OnProcessErrorDataReceived(ProcessDataReceivedEventArgs e)
        {
            var handler = ProcessErrorDataReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Occurs when an application writes to its redirected StandardOutput stream.
        /// </summary>
        /// <remarks>
        /// The OutputDataReceived event indicates that the associated process has written to its redirected StandardOutput stream.
        /// The event only occurs during asynchronous read operations on StandardOutput. To start asynchronous read operations, 
        /// you must redirect the StandardOutput stream of a Process, add your event handler to the OutputDataReceived event, 
        /// and call BeginOutputReadLine. Thereafter, the OutputDataReceived event signals each time the process writes a 
        /// line to the redirected StandardOutput stream, until the process exits or calls CancelOutputRead.
        /// <note>The application that is processing the asynchronous output should call the WaitForExit method to ensure that the output buffer has been flushed.</note>
        /// </remarks>
        event EventHandler<ProcessDataReceivedEventArgs> IProcess.OutputDataReceived
        {
            add
            {
                if (ProcessOutputDataReceived == null)
                {
                    _process.OutputDataReceived += Process_OutputDataReceived;
                    _process.BeginOutputReadLine();
                }

                ProcessOutputDataReceived += value;
            }

            remove
            {
                _process.CancelOutputRead();
                ProcessOutputDataReceived -= value;
            }
        }

        private void Process_OutputDataReceived(object sender, DataReceivedEventArgs e)
        {
            if (e.Data != null)
            {
                OnProcessOutputDataReceived(new ProcessDataReceivedEventArgs(e.Data));
            }
        }

        private void OnProcessOutputDataReceived(ProcessDataReceivedEventArgs e)
        {
            var handler = ProcessOutputDataReceived;
            if (handler != null)
            {
                handler(this, e);
            }
        }

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        /// <remarks>This also closes the underlying process handle, but does not stop the process if it is still running.</remarks>
        public void Dispose()
        {
            ProcessExited = null;
            ProcessErrorDataReceived = null;
            ProcessOutputDataReceived = null;

            _process.Exited -= Process_Exited;
            if (_process.StartInfo.RedirectStandardError)
            {
                _process.ErrorDataReceived -= Process_ErrorDataReceived;
            }
            if (_process.StartInfo.RedirectStandardOutput)
            {
                _process.OutputDataReceived -= Process_OutputDataReceived;
            }

            _process.Dispose();
        }
    }
}