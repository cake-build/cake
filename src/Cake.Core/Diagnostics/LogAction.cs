// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
namespace Cake.Core.Diagnostics
{
    /// <summary>
    /// Delegate representing lazy log action.
    /// </summary>
    /// <param name="actionEntry">Proxy to log.</param>
    public delegate void LogAction(LogActionEntry actionEntry);

    /// <summary>
    /// Delegate representing lazy log entry.
    /// </summary>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using format.</param>
    public delegate void LogActionEntry(string format, params object[] args);

    /// <summary>
    /// Delegate representing a function that can format log entries
    /// </summary>
    /// <param name="verbosity">The verbosity of the log entry</param>
    /// <param name="level">The log level</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using format.</param>
    public delegate string LogEntryFormatterAction(Verbosity verbosity, LogLevel level, string format, params object[] args);

    /// <summary>
    /// Delegate representing a function that will take action on a log entry.
    /// </summary>
    /// <param name="verbosity">The verbosity of the log entry</param>
    /// <param name="level">The log level</param>
    /// <param name="format">A composite format string.</param>
    /// <param name="args">An array of objects to write using format.</param>
    public delegate void FullLogActionEntry(Verbosity verbosity, LogLevel level, string format, params object[] args);
}
