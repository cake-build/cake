﻿using System;
using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about the overall build following its completion
    /// </summary>
    public sealed class SetupContext : CakeContextAdapter, ISetupContext
    {
        /// <summary>
        /// Gets target / initating task.
        /// </summary>
        public ICakeTaskInfo TargetTask { get;  }

        /// <summary>
        /// Gets all registered tasks that are going to be executed.
        /// </summary>
        public IReadOnlyCollection<ICakeTaskInfo> TasksToExecute { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupContext"/> class.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="targetTask">The target / initating task.</param>
        /// <param name="tasksToExecute">The tasks to execute.</param>
        public SetupContext(ICakeContext context,
            ICakeTaskInfo targetTask,
            IEnumerable<ICakeTaskInfo> tasksToExecute)
            : base(context)
        {
            TargetTask = targetTask;
            TasksToExecute = new List<ICakeTaskInfo>(tasksToExecute ?? Array.Empty<ICakeTaskInfo>());
        }
    }
}