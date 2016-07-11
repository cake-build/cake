// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Diagnostics
{
    /// <summary>
    /// Represents a debugger
    /// </summary>
    public interface IDebugger
    {
        /// <summary>
        /// Gets the current process id
        /// </summary>
        /// <returns>The current process id</returns>
        int GetProcessId();

        /// <summary>
        /// Wait for the debugger to attach
        /// </summary>
        /// <param name="timeout">A TimeSpan that represents the number of milliseconds to wait,
        /// or a TimeSpan that represents -1 milliseconds to wait indefinitely.</param>
        /// <returns>true if the debugger is attached within the timeout; otherwise, false.</returns>
        bool WaitForAttach(TimeSpan timeout);
    }
}
