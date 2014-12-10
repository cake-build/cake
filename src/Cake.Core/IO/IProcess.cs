﻿using System.Collections.Generic;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a process.
    /// </summary>
    public interface IProcess
    {
        /// <summary>
        /// Waits for the process to exit.
        /// </summary>
        void WaitForExit();

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
    }
}
