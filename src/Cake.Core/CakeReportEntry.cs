using System;

namespace Cake.Core
{
    /// <summary>
    /// Represents an entry in a <see cref="CakeReport"/>.
    /// </summary>
    public sealed class CakeReportEntry
    {
        private readonly string _taskName;
        private readonly TimeSpan _duration;

        /// <summary>
        /// Gets the task name.
        /// </summary>
        /// <value>The name.</value>
        public string TaskName
        {
            get { return _taskName; }
        }

        /// <summary>
        /// Gets the duration the task ran for.
        /// </summary>
        /// <value>The duration the task ran for.</value>
        public TimeSpan Duration
        {
            get { return _duration; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportEntry"/> class.
        /// </summary>
        /// <param name="taskName">The name of the task.</param>
        /// <param name="duration">The duration.</param>
        public CakeReportEntry(string taskName, TimeSpan duration)
        {
            _taskName = taskName;
            _duration = duration;
        }
    }
}