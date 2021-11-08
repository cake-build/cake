// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// [deprecated] TaskSetupEventArgs is obsolete and will be removed in a future release. Use <see cref="BeforeTaskSetupEventArgs" /> instead.
    /// Event data for the <see cref="ICakeEngine.TaskSetup"/> event.
    /// </summary>
    [Obsolete("TaskSetupEventArgs is obsolete and will be removed in a future release. Use BeforeTaskSetupEventArgs instead.")]
    public sealed class TaskSetupEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the task setup context.
        /// </summary>
        public ITaskSetupContext TaskSetupContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSetupEventArgs"/> class.
        /// </summary>
        /// <param name="taskSetupContext">The task setup context.</param>
        public TaskSetupEventArgs(ITaskSetupContext taskSetupContext)
        {
            TaskSetupContext = taskSetupContext;
        }
    }
}
