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
        private IEnumerable<string> _standardError;
        private IEnumerable<string> _standardOutput;

        /// <summary>
        /// Performs application-defined tasks associated with freeing, releasing, or resetting unmanaged resources.
        /// </summary>
        public void Dispose()
        {
        }

        /// <inheritdoc/>
        public void WaitForExit()
        {
        }

        /// <inheritdoc/>
        public bool WaitForExit(int milliseconds)
        {
            return true;
        }

        /// <inheritdoc/>
        public int GetExitCode()
        {
            return _exitCode;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetStandardError()
        {
            return _standardError;
        }

        /// <inheritdoc/>
        public IEnumerable<string> GetStandardOutput()
        {
            return _standardOutput;
        }

        /// <inheritdoc/>
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
        /// Sets the standard error.
        /// </summary>
        /// <param name="standardError">The standard error.</param>
        public void SetStandardError(IEnumerable<string> standardError)
        {
            _standardError = standardError;
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