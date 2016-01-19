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
        private readonly bool _skipped;

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
        /// Gets a value indicating whether the task was skipped.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the task was skipped; otherwise, <c>false</c>.
        /// </value>
        public bool Skipped
        {
            get { return _skipped; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportEntry"/> class.
        /// </summary>
        /// <param name="taskName">The name of the task.</param>
        /// <param name="duration">The duration.</param>
        public CakeReportEntry(string taskName, TimeSpan duration) 
            : this(taskName, duration, false)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportEntry"/> class.
        /// </summary>
        /// <param name="taskName">The name of the task.</param>
        /// <param name="duration">The duration.</param>
        /// <param name="skipped">Indicates if the task was skipped.</param>
        public CakeReportEntry(string taskName, TimeSpan duration, bool skipped)
        {
            _taskName = taskName;
            _duration = duration;
            _skipped = skipped;
        }
    }
}