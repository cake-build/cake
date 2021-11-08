// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Event data for the <see cref="ICakeEngine.AfterTaskTeardown"/> event.
    /// </summary>
    public sealed class AfterTaskTeardownEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the task teardown context.
        /// </summary>
        public ITaskTeardownContext TaskTeardownContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterTaskTeardownEventArgs"/> class.
        /// </summary>
        /// <param name="taskTeardownContext">The task teardown context.</param>
        public AfterTaskTeardownEventArgs(ITaskTeardownContext taskTeardownContext)
        {
            TaskTeardownContext = taskTeardownContext;
        }
    }
}