// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <inheritdoc/>
    public sealed class TaskTeardownContext : CakeContextAdapter, ITaskTeardownContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTeardownContext"/> class.
        /// </summary>
        /// <param name="context">The Cake Context.</param>
        /// <param name="task">The task.</param>
        /// <param name="duration">The duration of the task.</param>
        /// <param name="skipped">if set to <c>true</c>, the task was not executed.</param>
        /// <param name="throwException">The exception that was thrown by the task.</param>
        public TaskTeardownContext(ICakeContext context, ICakeTaskInfo task, TimeSpan duration, bool skipped, Exception throwException)
            : base(context)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            Task = task;
            Duration = duration;
            Skipped = skipped;
            ThrownException = throwException;
        }

        /// <inheritdoc/>
        public ICakeTaskInfo Task { get; }

        /// <inheritdoc/>
        public TimeSpan Duration { get; }

        /// <inheritdoc/>
        public bool Skipped { get; }

        /// <inheritdoc/>
        public bool Successful => ThrownException == null;

        /// <inheritdoc/>
        public Exception ThrownException { get; }
    }
}