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
        /// Get the standard error output of process
        /// </summary>
        /// <returns>Returns process error output <see cref="ProcessSettings.RedirectStandardError">RedirectStandardError</see> is true</returns>
        IEnumerable<string> GetStandardError();

        /// <summary>
        /// Gets the unique identifier for the associated process.
        /// </summary>
        /// <value>
        /// The process identifier.
        /// </value>
        int ProcessId { get; }

        /// <summary>
        /// Immediately stops the associated process.
        /// </summary>
        void Kill();

        /// <summary>
        /// Gets a value indicating whether this instance has exited.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance has exited; otherwise, <c>false</c>.
        /// </value>
        bool HasExited { get; }

        /// <summary>
        /// Specify a callback to be invoked when the process exits.
        /// </summary>
        /// <param name="action">The action.</param>
        void HandleExited(Action<ProcessExitedEventArgs> action);

        /// <summary>
        /// Specify a callback to be invoked when the process writes to its redirected StandardError stream.
        /// </summary>
        /// <remarks>May not be used with <see cref="GetStandardError"/>.</remarks>
        /// <param name="action">The action.</param>
        void HandleErrorOutput(Action<ProcessOutputReceivedEventArgs> action);

        /// <summary>
        /// Specify a callback to be invoked when the process writes to its redirected StandardOutput stream.
        /// </summary>
        /// <remarks>May not be used with <see cref="GetStandardOutput"/>.</remarks>
        /// <param name="action">The action.</param>
        void HandleStandardOutput(Action<ProcessOutputReceivedEventArgs> action);
    }
}
