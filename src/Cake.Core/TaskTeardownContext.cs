﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about a <see cref="ICakeTaskInfo"/> following its invocation.
    /// </summary>
    public sealed class TaskTeardownContext : CakeContextAdapter, ITaskTeardownContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTeardownContext"/> class.
        /// </summary>
        /// <param name="context">The Cake Context.</param>
        /// <param name="task">The task.</param>
        /// <param name="duration">The duration of the task.</param>
        /// <param name="skipped">if set to <c>true</c>, the task was not executed.</param>
        public TaskTeardownContext(ICakeContext context, ICakeTaskInfo task, TimeSpan duration, bool skipped)
            : base(context)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            Task = task;
            Duration = duration;
            Skipped = skipped;
        }

        /// <summary>
        /// Gets the <see cref="ICakeTaskInfo" /> describing the <see cref="CakeTask" /> that has just been invoked.
        /// </summary>
        /// <value>
        /// The task.
        /// </value>
        public ICakeTaskInfo Task { get; }

        /// <summary>
        /// Gets the duration of the <see cref="CakeTask"/>'s execution.
        /// </summary>
        /// <value>
        /// The duration of the <see cref="CakeTask"/>'s execution.
        /// </value>
        public TimeSpan Duration { get; }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CakeTask"/> was skipped (not executed).
        /// </summary>
        /// <value>
        /// <c>true</c> if skipped; otherwise, <c>false</c>.
        /// </value>
        public bool Skipped { get; }
    }
}