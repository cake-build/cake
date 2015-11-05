using System.Collections.Generic;
using Cake.Core.Diagnostics;

namespace Cake.Testing
{
    /// <summary>
    /// Implementation of a <see cref="ICakeLog"/> that saves all messages written to it.
    /// </summary>
    public sealed class FakeLog : ICakeLog
    {
        private readonly List<FakeLogMessage> _entries;

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public IReadOnlyList<FakeLogMessage> Entries
        {
            get { return _entries; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeLog"/> class.
        /// </summary>
        public FakeLog()
        {
            _entries = new List<FakeLogMessage>();
        }

        /// <summary>
        /// Gets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        public Verbosity Verbosity
        {
            get { return Verbosity.Quiet; }
        }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// log using the specified verbosity, log level and format information.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public void Write(Verbosity verbosity, LogLevel level, string format, params object[] args)
        {
            _entries.Add(new FakeLogMessage(verbosity, level, string.Format(format, args)));
        }
    }
}