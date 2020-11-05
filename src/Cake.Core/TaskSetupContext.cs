// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <inheritdoc/>
    public sealed class TaskSetupContext : CakeContextAdapter, ITaskSetupContext
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSetupContext"/> class.
        /// </summary>
        /// <param name="context">The Cake Context.</param>
        /// <param name="task">The task.</param>
        public TaskSetupContext(ICakeContext context, ICakeTaskInfo task)
            : base(context)
        {
            if (task == null)
            {
                throw new ArgumentNullException(nameof(task));
            }

            Task = task;
        }

        /// <inheritdoc/>
        public ICakeTaskInfo Task { get; }
    }
}