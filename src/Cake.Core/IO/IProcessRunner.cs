using System.Diagnostics;

namespace Cake.Core.IO
{
    /// <summary>
    /// Represents a process runner.
    /// </summary>
    public interface IProcessRunner
    {
        /// <summary>
        /// Starts a process using the specified information.
        /// </summary>
        /// <param name="info">The information about the process to start.</param>
        /// <returns>A process handle.</returns>
        IProcess Start(ProcessStartInfo info);
    }
}
