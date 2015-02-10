using System.Collections.Generic;
using Cake.Core.Diagnostics;

namespace Cake.Testing.Fakes
{
    /// <summary>
    /// Implementation of a <see cref="ICakeLog"/> that saves all messages written to it.
    /// </summary>
    public sealed class FakeLog : ICakeLog
    {
        private readonly List<string> _messages;

        /// <summary>
        /// Gets the messages.
        /// </summary>
        /// <value>The messages.</value>
        public List<string> Messages
        {
            get { return _messages; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="FakeLog"/> class.
        /// </summary>
        public FakeLog()
        {
            _messages = new List<string>();
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
            _messages.Add(string.Format(format, args));
        }
    }
}
