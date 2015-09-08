using System;

namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about a <see cref="CakeTask"/> before its invocation.
    /// </summary>
    public sealed class TaskSetupContext : ITaskSetupContext
    {
        private readonly ICakeTaskInfo _task;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskSetupContext"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        public TaskSetupContext(ICakeTaskInfo task)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            _task = task;
        }

        /// <summary>
        /// Gets the <see cref="ICakeTaskInfo"/> describing the <see cref="CakeTask"/> that has just been invoked.
        /// </summary>
        /// <value>
        /// The task.
        /// </value>
        public ICakeTaskInfo Task
        {
            get { return _task; }
        }
    }
}