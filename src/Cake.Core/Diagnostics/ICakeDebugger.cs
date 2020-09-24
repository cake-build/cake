using System;

namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// Represents a mechanism to interact with the debugger.
    /// </summary>
    public interface ICakeDebugger
    {
        /// <summary>
        /// Waits for a debugger to attach to the process.
        /// </summary>
        /// <param name="timeout">The timeout.</param>
        void WaitForAttach(TimeSpan timeout);
    }
}
