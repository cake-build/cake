// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a process.
    /// </summary>
    public interface IProcess : IDisposable
    {
        /// <summary>
        /// Waits for the process to exit.
        /// </summary>
        void WaitForExit();

        /// <summary>
        /// Waits for the process to exit with possible timeout for command.
        /// </summary>
        /// <param name="milliseconds">The amount of time, in milliseconds, to wait for the associated process to exit. The maximum is the largest possible value of a 32-bit integer, which represents infinity to the operating system.</param>
        /// <returns>true if the associated process has exited; otherwise, false.</returns>
        bool WaitForExit(int milliseconds);

        /// <summary>
        /// Gets the exit code of the process.
        /// </summary>
        /// <returns>The exit code of the process.</returns>
        int GetExitCode();

        /// <summary>
        /// Get the standard output of process
        /// </summary>
        /// <returns>Returns process output <see cref="ProcessSettings.RedirectStandardOutput">RedirectStandardOutput</see> is true</returns>
        IEnumerable<string> GetStandardOutput();

        /// <summary>
        /// Immediately stops the associated process.
        /// </summary>
        void Kill();
    }
}
