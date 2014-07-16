namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// Contains extension methods for <see cref="ICakeLog"/>.
    /// </summary>
    public static class LogExtensions
    {
        /// <summary>
        /// Writes an error message to the log using the specified format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Error(this ICakeLog log, string format, params object[] args)
        {
            Error(log, Verbosity.Quiet, format, args);
        }

        /// <summary>
        /// Writes an error message to the log using the specified 
        /// verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Error(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Error, format, args);
            }
        }

        /// <summary>
        /// Writes a warning message to the log using the specified format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Warning(this ICakeLog log, string format, params object[] args)
        {
            Warning(log, Verbosity.Minimal, format, args);
        }

        /// <summary>
        /// Writes a warning message to the log using the specified 
        /// verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Warning(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Warning, format, args);
            }
        }

        /// <summary>
        /// Writes an informational message to the log using the specified format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Information(this ICakeLog log, string format, params object[] args)
        {
            Information(log, Verbosity.Normal, format, args);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified 
        /// verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Information(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Information, format, args);
            }
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Verbose(this ICakeLog log, string format, params object[] args)
        {
            Verbose(log, Verbosity.Verbose, format, args);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified 
        /// verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Verbose(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Verbose, format, args);
            }
        }

        /// <summary>
        /// Writes a debug message to the log using the specified format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Debug(this ICakeLog log, string format, params object[] args)
        {
            Debug(log, Verbosity.Diagnostic, format, args);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified 
        /// verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Debug(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            if (log != null)
            {
                log.Write(verbosity, LogLevel.Debug, format, args);
            }
        }
    }
}
