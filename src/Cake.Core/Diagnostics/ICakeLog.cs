namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// Represents a log.
    /// </summary>
    public interface ICakeLog
    {
        /// <summary>
        /// Gets the verbosity.
        /// </summary>
        /// <value>The verbosity.</value>
        Verbosity Verbosity { get; }

        /// <summary>
        /// Writes the text representation of the specified array of objects to the
        /// log using the specified verbosity, log level and format information.
        /// </summary>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        void Write(Verbosity verbosity, LogLevel level, string format, params object[] args);
    }
}