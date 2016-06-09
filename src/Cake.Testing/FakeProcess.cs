// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Testing
{
    /// <summary>
    /// Implementation of a fake process.
    /// </summary>
    public sealed class FakeProcess : IProcess
    {
        private int _exitCode;
        private IEnumerable<string> _standardOutput;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <summary>
        /// Waits for the process to exit
        /// </summary>
        public void WaitForExit()
        {
        }

        /// <summary>
        /// Waits for the process to exit with possible timeout for command.
        /// </summary>
        /// <param name="milliseconds">The amount of time, in milliseconds, to wait for the associated process to exit. The maximum is the largest possible value of a 32-bit integer, which represents infinity to the operating system.</param>
        /// <returns>true if the associated process has exited; otherwise, false.</returns>
        public bool WaitForExit(int milliseconds)
        {
            return true;
        }

        /// <summary>
        /// Gets the exit code.
        /// </summary>
        /// <returns>The process exit code.</returns>
        public int GetExitCode()
        {
            return _exitCode;
        }

        /// <summary>
        /// Get the standard output of process
        /// </summary>
        /// <returns>Returns process output <see cref="ProcessSettings.RedirectStandardOutput">RedirectStandardOutput</see> is true</returns>
        public IEnumerable<string> GetStandardOutput()
        {
            return _standardOutput;
        }

        /// <summary>
        /// Immediately stops the associated process.
        /// </summary>
        public void Kill()
        {
        }

        /// <summary>
        /// Sets the exit code.
        /// </summary>
        /// <param name="exitCode">The exit code.</param>
        public void SetExitCode(int exitCode)
        {
            _exitCode = exitCode;
        }

        /// <summary>
        /// Sets the standard output.
        /// </summary>
        /// <param name="standardOutput">The standard output.</param>
        public void SetStandardOutput(IEnumerable<string> standardOutput)
        {
            _standardOutput = standardOutput;
        }
    }
}
