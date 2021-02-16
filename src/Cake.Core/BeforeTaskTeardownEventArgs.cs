// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Event data for the <see cref="ICakeEngine.BeforeTaskTeardown"/> event.
    /// </summary>
    public sealed class BeforeTaskTeardownEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the task teardown context.
        /// </summary>
        public ITaskTeardownContext TaskTeardownContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BeforeTaskTeardownEventArgs"/> class.
        /// </summary>
        /// <param name="taskTeardownContext">The task teardown context.</param>
        public BeforeTaskTeardownEventArgs(ITaskTeardownContext taskTeardownContext)
        {
            TaskTeardownContext = taskTeardownContext;
        }
    }
}