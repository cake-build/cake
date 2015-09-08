using System;

namespace Cake.Core
{
    /// <summary>
    /// Acts as a context providing info about a <see cref="ICakeTaskInfo"/> following its invocation.
    /// </summary>
    public sealed class TaskTeardownContext : ITaskTeardownContext
    {
        private readonly ICakeTaskInfo _task;
        private readonly TimeSpan _duration;
        private readonly bool _skipped;

        /// <summary>
        /// Initializes a new instance of the <see cref="TaskTeardownContext"/> class.
        /// </summary>
        /// <param name="task">The task.</param>
        /// <param name="duration">The duration of the task.</param>
        /// <param name="skipped">if set to <c>true</c>, the task was not executed.</param>
        public TaskTeardownContext(ICakeTaskInfo task, TimeSpan duration, bool skipped)
        {
            if (task == null)
            {
                throw new ArgumentNullException("task");
            }

            _task = task;
            _duration = duration;
            _skipped = skipped;
        }
        
        /// <summary>
        /// Gets the <see cref="ICakeTaskInfo" /> describing the <see cref="CakeTask" /> that has just been invoked.
        /// </summary>
        /// <value>
        /// The task.
        /// </value>
        public ICakeTaskInfo Task
        {
            get { return _task; }
        }

        /// <summary>
        /// Gets the duration of the <see cref="CakeTask"/>'s execution.
        /// </summary>
        /// <value>
        /// The duration of the <see cref="CakeTask"/>'s execution.
        /// </value>
        public TimeSpan Duration
        {
            get { return _duration; }
        }

        /// <summary>
        /// Gets a value indicating whether the <see cref="CakeTask"/> was skipped (not executed).
        /// </summary>
        /// <value>
        /// <c>true</c> if skipped; otherwise, <c>false</c>.
        /// </value>
        public bool Skipped
        {
            get { return _skipped; }
        }
    }
}