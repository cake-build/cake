// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about a <see cref="CakeTask"/> following its invocation.
    /// </summary>
    public interface ITaskTeardownContext
    {
        /// <summary>
        /// Gets the <see cref="ICakeTaskInfo"/> describing the <see cref="CakeTask"/> that has just been invoked.
        /// </summary>
        /// <value>
        /// The task.
        /// </value>
        ICakeTaskInfo Task { get; }

        /// <summary>
        /// Gets the duration of the <see cref="CakeTask"/>'s execution.
        /// </summary>
        /// <value>
        /// The duration of the <see cref="CakeTask"/>'s execution.
        /// </value>
        TimeSpan Duration { get; }

        /// <summary>
        /// Gets a value indicating whether this <see cref="CakeTask"/> was skipped (not executed).
        /// </summary>
        /// <value>
        /// <c>true</c> if skipped; otherwise, <c>false</c>.
        /// </value>
        bool Skipped { get; }
    }
}
