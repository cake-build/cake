using System;
using System.Collections.Generic;

namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about the overall build following its completion.
    /// </summary>
    public sealed class SetupContext : CakeContextAdapter, ISetupContext
    {
        /// <inheritdoc/>
        public ICakeTaskInfo TargetTask { get;  }

        /// <inheritdoc/>
        public IReadOnlyCollection<ICakeTaskInfo> TasksToExecute { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="SetupContext"/> class.
        /// </summary>
        /// <param name="context">The Cake context.</param>
        /// <param name="targetTask">The target (initiating) task.</param>
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