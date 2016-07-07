using System;

namespace Cake.Core.Diagnostics
{
    /// <summary>
    ///     Class that provides the functionality for a callback whenever log entries are written.
    /// </summary>
    public sealed class ActionLog : ICakeLog
    {
        private readonly FullLogActionEntry _action;

        private readonly object _lock;

        /// <summary>
        ///     Initializes a new instance of the <see cref="ActionLog" /> class.
        /// </summary>
        /// <param name="logAction">The <see cref="FullLogActionEntry" /> delegate that will be called when a log entry is written</param>
        /// <param name="defaultVerbosity">The <see cref="Verbosity" /> to use when writing the logs</param>
        /// <param name="synchronize">
        ///     <c>True</c> if all calls to the delegate should be synchronized, <c>false</c> if the callback
        ///     can be triggered from multiple threads at the same time.
        /// </param>
        public ActionLog(FullLogActionEntry logAction, Verbosity defaultVerbosity = Verbosity.Normal, bool synchronize = true)
        {
            if (logAction == null)
            {
                throw new ArgumentNullException("logAction");
            }

            _action = logAction;
            _lock = synchronize ? new object() : null;
        }

        /// <summary>
        ///     Gets or sets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        ///     Writes the text representation of the specified array of objects to the
        ///     log using the specified verbosity, log level and format information.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            if (verbosity > Verbosity)
            {
                return;
            }

            if (_lock != null)
            {
                lock (_lock)
                {
                    _action(verbosity, level, format, args);
                }
            }
            else
            {
                _action(verbosity, level, format, args);
            }
        }
    }
}