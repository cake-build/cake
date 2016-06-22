using System;
using System.Collections.Generic;

using Cake.Core.Diagnostics;

namespace Cake.Diagnostics
{
    internal sealed class CakeLogPipeline : ICakeLogPipeline
    {
        private readonly CakeLogPipelineImplementation _pipeline;

        /// <summary>
        ///     Initializes a new instance of the <see cref="CakeLogPipeline" /> class.
        /// </summary>
        /// <param name="defaultLog">The log instance for the pipeline</param>
        public CakeLogPipeline(ICakeLog defaultLog)
        {
            if (defaultLog == null)
            {
                throw new ArgumentNullException("defaultLog");
            }

            _pipeline = new CakeLogPipelineImplementation(defaultLog);
        }

        /// <summary>
        ///     Gets the listeners for the pipeline
        /// </summary>
        /// <remarks>If a listener is added or removed while enumerating this property an exception will be thrown.</remarks>
        public IEnumerable<ICakeLog> Listeners
        {
            get { return _pipeline.Listeners; }
        }

        /// <summary>
        ///     Gets the <see cref="ICakeLog" /> implementation that represents the pipeline.
        /// </summary>
        public ICakeLog CakeLog
        {
            get { return _pipeline; }
        }

        /// <summary>
        ///     Adds a <see cref="ICakeLog" /> instance to the pipeline.
        /// </summary>
        /// <param name="toAdd">The pipeline instance to add.</param>
        public void AddLog(ICakeLog toAdd)
        {
            if (toAdd == null)
            {
                throw new ArgumentNullException("toAdd");
            }

            _pipeline.AddLog(toAdd);
        }

        /// <summary>
        ///     Removes a <see cref="ICakeLog" /> instance from the pipeline.
        /// </summary>
        /// <param name="toRemove">The pipeline instance to remove.</param>
        public void RemoveLog(ICakeLog toRemove)
        {
            if (toRemove == null)
            {
                throw new ArgumentNullException("toRemove");
            }

            _pipeline.RemoveLog(toRemove);
        }

        /// <summary>
        ///     The <see cref="ICakeLog" /> implementation for the pipeline logger, so that IoC resolve will not pick up a public
        ///     implementation and cause a circular resolve.
        /// </summary>
        private class CakeLogPipelineImplementation : ICakeLog
        {
            private readonly object _lock;

            private readonly IList<ICakeLog> _cakeLogs;

            private readonly ICakeLog _defaultLog;

            /// <summary>
            ///     Initializes a new instance of the <see cref="CakeLogPipelineImplementation" /> class.
            /// </summary>
            /// <param name="verbosity">The verbosity of the implementation</param>
            public CakeLogPipelineImplementation(ICakeLog defaultLog)
            {
                _cakeLogs = new List<ICakeLog> { defaultLog };
                _lock = new object();
                _defaultLog = defaultLog;
            }

            /// <summary>
            ///     Gets the current listeners
            /// </summary>
            public IEnumerable<ICakeLog> Listeners
            {
                get
                {
                    foreach (ICakeLog log in _cakeLogs)
                    {
                        yield return log;
                    }
                }
            }

            /// <summary>
            ///     Gets or sets the verbosity.
            /// </summary>
            /// <value>The verbosity.</value>
            public Verbosity Verbosity
            {
                get { return _defaultLog.Verbosity; }
                set { _defaultLog.Verbosity = value; }
            }

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
                lock (_lock)
                {
                    // Don't short circuit on verbosity in case downstream loggers have higher verbosity
                    foreach (ICakeLog log in _cakeLogs)
                    {
                        log.Write(verbosity, level, format, args);
                    }
                }
            }

            /// <summary>
            ///     Adds a <see cref="ICakeLog" /> instance to the pipeline.
            /// </summary>
            /// <param name="toAdd">The pipeline instance to add.</param>
            public void AddLog(ICakeLog toAdd)
            {
                lock (_lock)
                {
                    _cakeLogs.Add(toAdd);
                }
            }

            /// <summary>
            ///     Removes a <see cref="ICakeLog" /> instance from the pipeline.
            /// </summary>
            /// <param name="toRemove">The pipeline instance to remove.</param>
            public void RemoveLog(ICakeLog toRemove)
            {
                lock (_lock)
                {
                    _cakeLogs.Remove(toRemove);
                }
            }
        }
    }
}