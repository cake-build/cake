// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Event data for the <see cref="ICakeEngine.AfterTaskSetup"/> event.
    /// </summary>
    public sealed class AfterTaskSetupEventArgs : EventArgs
    {
        /// <summary>
        /// Gets the task setup context.
        /// </summary>
        public ITaskSetupContext TaskSetupContext { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AfterTaskSetupEventArgs"/> class.
        /// </summary>
        /// <param name="taskSetupContext">The task setup context.</param>
        public AfterTaskSetupEventArgs(ITaskSetupContext taskSetupContext)
        {
            TaskSetupContext = taskSetupContext;
        }
    }
}