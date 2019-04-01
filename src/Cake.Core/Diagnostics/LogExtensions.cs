// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

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
        /// Writes an error message to the log using the specified verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Error(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            log?.Write(verbosity, LogLevel.Error, format, args);
        }

        /// <summary>
        /// Writes an error message to the log using the specified verbosity and log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="logAction">The log action.</param>
        public static void Error(this ICakeLog log, Verbosity verbosity, LogAction logAction)
        {
            Write(log, verbosity, LogLevel.Error, logAction);
        }

        /// <summary>
        /// Writes an error message to the log using the specified log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="logAction">The log action.</param>
        public static void Error(this ICakeLog log, LogAction logAction)
        {
            Error(log, Verbosity.Quiet, logAction);
        }

        /// <summary>
        /// Writes an error message to the log using the specified value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Error(this ICakeLog log, object value)
        {
            log.Error("{0}", value);
        }

        /// <summary>
        /// Writes an error message to the log using the specified string value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Error(this ICakeLog log, string value)
        {
            log.Error("{0}", value);
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
        /// Writes a warning message to the log using the specified verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Warning(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            log?.Write(verbosity, LogLevel.Warning, format, args);
        }

        /// <summary>
        /// Writes a warning message to the log using the specified log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="logAction">The log action.</param>
        public static void Warning(this ICakeLog log, LogAction logAction)
        {
            Warning(log, Verbosity.Minimal, logAction);
        }

        /// <summary>
        /// Writes a warning message to the log using the specified verbosity and log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="logAction">The log action.</param>
        public static void Warning(this ICakeLog log, Verbosity verbosity, LogAction logAction)
        {
            Write(log, verbosity, LogLevel.Warning, logAction);
        }

        /// <summary>
        /// Writes an warning message to the log using the specified value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Warning(this ICakeLog log, object value)
        {
            log.Warning("{0}", value);
        }

        /// <summary>
        /// Writes an warning message to the log using the specified string value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Warning(this ICakeLog log, string value)
        {
            log.Warning("{0}", value);
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
        /// Writes an informational message to the log using the specified verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Information(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            log?.Write(verbosity, LogLevel.Information, format, args);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified verbosity and log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="logAction">The log action.</param>
        public static void Information(this ICakeLog log, Verbosity verbosity, LogAction logAction)
        {
            Write(log, verbosity, LogLevel.Information, logAction);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="logAction">The log action.</param>
        public static void Information(this ICakeLog log, LogAction logAction)
        {
            Information(log, Verbosity.Normal, logAction);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Information(this ICakeLog log, object value)
        {
            log.Information("{0}", value);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified string value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Information(this ICakeLog log, string value)
        {
            log.Information("{0}", value);
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
        /// Writes a verbose message to the log using the specified verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Verbose(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            log?.Write(verbosity, LogLevel.Verbose, format, args);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="logAction">The log action.</param>
        public static void Verbose(this ICakeLog log, LogAction logAction)
        {
            Verbose(log, Verbosity.Verbose, logAction);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified verbosity and log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="logAction">The log action.</param>
        public static void Verbose(this ICakeLog log, Verbosity verbosity, LogAction logAction)
        {
            Write(log, verbosity, LogLevel.Verbose, logAction);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Verbose(this ICakeLog log, object value)
        {
            log.Verbose("{0}", value);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified string value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Verbose(this ICakeLog log, string value)
        {
            log.Verbose("{0}", value);
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
        /// Writes a debug message to the log using the specified verbosity and format information.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="format">A composite format string.</param>
        /// <param name="args">An array of objects to write using format.</param>
        public static void Debug(this ICakeLog log, Verbosity verbosity, string format, params object[] args)
        {
            log?.Write(verbosity, LogLevel.Debug, format, args);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="logAction">The log action.</param>
        public static void Debug(this ICakeLog log, LogAction logAction)
        {
            Debug(log, Verbosity.Diagnostic, logAction);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified verbosity and log message action.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="logAction">The log action.</param>
        public static void Debug(this ICakeLog log, Verbosity verbosity, LogAction logAction)
        {
            Write(log, verbosity, LogLevel.Debug, logAction);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Debug(this ICakeLog log, object value)
        {
            log.Debug("{0}", value);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified string value.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="value">The value.</param>
        public static void Debug(this ICakeLog log, string value)
        {
            log.Debug("{0}", value);
        }

        /// <summary>
        /// Writes a message to the log using the specified verbosity, log level and log action delegate.
        /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <param name="level">The log level.</param>
        /// <param name="logAction">The log action.</param>
        public static void Write(this ICakeLog log, Verbosity verbosity, LogLevel level, LogAction logAction)
        {
            if (log == null || logAction == null)
            {
                return;
            }

            if (verbosity > log.Verbosity)
            {
                return;
            }

            LogActionEntry actionEntry = (format, args) => log.Write(verbosity, level, format, args);
            logAction(actionEntry);
        }

        /// <summary>
        /// Sets the log verbosity to quiet and returns a disposable that restores the log verbosity on dispose.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>A disposable that restores the log verbosity.</returns>
        public static IDisposable QuietVerbosity(this ICakeLog log)
        {
            return log.WithVerbosity(Verbosity.Quiet);
        }

        /// <summary>
        /// Sets the log verbosity to minimal and returns a disposable that restores the log verbosity on dispose.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>A disposable that restores the log verbosity.</returns>
        public static IDisposable MinimalVerbosity(this ICakeLog log)
        {
            return log.WithVerbosity(Verbosity.Minimal);
        }

        /// <summary>
        /// Sets the log verbosity to normal and returns a disposable that restores the log verbosity on dispose.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>A disposable that restores the log verbosity.</returns>
        public static IDisposable NormalVerbosity(this ICakeLog log)
        {
            return log.WithVerbosity(Verbosity.Normal);
        }

        /// <summary>
        /// Sets the log verbosity to verbose and returns a disposable that restores the log verbosity on dispose.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>A disposable that restores the log verbosity.</returns>
        public static IDisposable VerboseVerbosity(this ICakeLog log)
        {
            return log.WithVerbosity(Verbosity.Verbose);
        }

        /// <summary>
        /// Sets the log verbosity to diagnostic and returns a disposable that restores the log verbosity on dispose.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <returns>A disposable that restores the log verbosity.</returns>
        public static IDisposable DiagnosticVerbosity(this ICakeLog log)
        {
            return log.WithVerbosity(Verbosity.Diagnostic);
        }

        /// <summary>
        /// Sets the log verbosity as specified and returns a disposable that restores the log verbosity on dispose.
        /// </summary>
        /// <param name="log">The log.</param>
        /// <param name="verbosity">The verbosity.</param>
        /// <returns>A disposable that restores the log verbosity.</returns>
        public static IDisposable WithVerbosity(this ICakeLog log, Verbosity verbosity)
        {
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            var lastVerbosity = log.Verbosity;
            log.Verbosity = verbosity;
            return Disposable.Create(() => log.Verbosity = lastVerbosity);
        }
    }
}