// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core.Diagnostics;

/// <summary>
/// Contains extension methods for <see cref="ICakeLog"/>.
/// </summary>
public static partial class LogExtensions
{
    /// <summary>
    /// Writes the text representation of the formattable string to the
    /// log using the specified verbosity, log level and format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="level">The log level.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Write(this ICakeLog log, Verbosity verbosity, LogLevel level, FormattableString formattable)
    {
        log?.Write(verbosity, level, formattable.Format, formattable.GetArguments());
    }

    /// <summary>
    /// Writes an formattable string error message to the log using the specified format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Error(this ICakeLog log, FormattableString formattable)
    {
        Error(log, Verbosity.Quiet, formattable);
    }

    /// <summary>
    /// Writes an formattable string error message to the log using the specified format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Error(this ICakeLog log, Verbosity verbosity, FormattableString formattable)
    {
        log?.Write(verbosity, LogLevel.Error, formattable);
    }

    /// <summary>
    /// Writes an formattable string error message to the log using the specified verbosity and log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattableLogAction">The formattable log action.</param>
    public static void Error(this ICakeLog log, Verbosity verbosity, FormattableLogAction formattableLogAction)
    {
        Write(log, verbosity, LogLevel.Error, formattableLogAction);
    }

    /// <summary>
    /// Writes an formattable string error message to the log using the specified log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattableLogAction">The log action.</param>
    public static void Error(this ICakeLog log, FormattableLogAction formattableLogAction)
    {
        Error(log, Verbosity.Quiet, formattableLogAction);
    }

    /// <summary>
    /// Writes a formattable string warning message to the log using the specified format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Warning(this ICakeLog log, FormattableString formattable)
    {
        Warning(log, Verbosity.Minimal, formattable);
    }

    /// <summary>
    /// Writes a formattable string warning message to the log using the specified verbosity and format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Warning(this ICakeLog log, Verbosity verbosity, FormattableString formattable)
    {
        log?.Write(verbosity, LogLevel.Warning, formattable);
    }

    /// <summary>
    /// Writes a formattable string warning message to the log using the specified log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattableLogAction">The formattable log action.</param>
    public static void Warning(this ICakeLog log, FormattableLogAction formattableLogAction)
    {
        Warning(log, Verbosity.Minimal, formattableLogAction);
    }

    /// <summary>
    /// Writes a formattable string warning message to the log using the specified verbosity and log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattableLogAction">The formattable log action.</param>
    public static void Warning(this ICakeLog log, Verbosity verbosity, FormattableLogAction formattableLogAction)
    {
        Write(log, verbosity, LogLevel.Warning, formattableLogAction);
    }

    /// <summary>
    /// Writes an formattable string informational message to the log using the specified format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Information(this ICakeLog log, FormattableString formattable)
    {
        Information(log, Verbosity.Normal, formattable);
    }

    /// <summary>
    /// Writes an formattable string informational message to the log using the specified verbosity and format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Information(this ICakeLog log, Verbosity verbosity, FormattableString formattable)
    {
        log?.Write(verbosity, LogLevel.Information, formattable);
    }

    /// <summary>
    /// Writes an formattable string informational message to the log using the specified verbosity and log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattableLogAction">The formattable log action.</param>
    public static void Information(this ICakeLog log, Verbosity verbosity, FormattableLogAction formattableLogAction)
    {
        Write(log, verbosity, LogLevel.Information, formattableLogAction);
    }

    /// <summary>
    /// Writes an formattable string informational message to the log using the specified log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattableLogAction">The formattable log action.</param>
    public static void Information(this ICakeLog log, FormattableLogAction formattableLogAction)
    {
        Information(log, Verbosity.Normal, formattableLogAction);
    }

    /// <summary>
    /// Writes a formattable string verbose message to the log using the specified format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Verbose(this ICakeLog log, FormattableString formattable)
    {
        Verbose(log, Verbosity.Verbose, formattable);
    }

    /// <summary>
    /// Writes a formattable string verbose message to the log using the specified verbosity and format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Verbose(this ICakeLog log, Verbosity verbosity, FormattableString formattable)
    {
        log?.Write(verbosity, LogLevel.Verbose, formattable);
    }

    /// <summary>
    /// Writes a formattable string verbose message to the log using the specified log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattableLogAction">The formattable log action.</param>
    public static void Verbose(this ICakeLog log, FormattableLogAction formattableLogAction)
    {
        Verbose(log, Verbosity.Verbose, formattableLogAction);
    }

    /// <summary>
    /// Writes a formattable string verbose message to the log using the specified verbosity and log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattableLogAction">The formattable log action.</param>
    public static void Verbose(this ICakeLog log, Verbosity verbosity, FormattableLogAction formattableLogAction)
    {
        Write(log, verbosity, LogLevel.Verbose, formattableLogAction);
    }

    /// <summary>
    /// Writes a formattable string debug message to the log using the specified format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Debug(this ICakeLog log, FormattableString formattable)
    {
        Debug(log, Verbosity.Diagnostic, formattable);
    }

    /// <summary>
    /// Writes a formattable string debug message to the log using the specified verbosity and format information.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattable">The string to be formatted.</param>
    public static void Debug(this ICakeLog log, Verbosity verbosity, FormattableString formattable)
    {
        log?.Write(verbosity, LogLevel.Debug, formattable);
    }

    /// <summary>
    /// Writes a formattable string debug message to the log using the specified log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="formattableLogAction">The formattable log action.</param>
    public static void Debug(this ICakeLog log, FormattableLogAction formattableLogAction)
    {
        Debug(log, Verbosity.Diagnostic, formattableLogAction);
    }

    /// <summary>
    /// Writes a formattable string debug message to the log using the specified verbosity and log message action.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="formattableLogAction">The formattable log action.</param>
    public static void Debug(this ICakeLog log, Verbosity verbosity, FormattableLogAction formattableLogAction)
    {
        Write(log, verbosity, LogLevel.Debug, formattableLogAction);
    }

    /// <summary>
    /// Writes a formattable string message to the log using the specified verbosity, log level and log action delegate.
    /// Evaluates log message only if the verbosity is equal to or more verbose than the log's verbosity.
    /// </summary>
    /// <param name="log">The log.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <param name="level">The log level.</param>
    /// <param name="formattableLogAction">The log action.</param>
    public static void Write(this ICakeLog log, Verbosity verbosity, LogLevel level, FormattableLogAction formattableLogAction)
    {
        if (log == null || formattableLogAction == null)
        {
            return;
        }

        if (verbosity > log.Verbosity)
        {
            return;
        }

        void actionEntry(FormattableString formattable)
            => log.Write(verbosity, level, formattable);

        formattableLogAction(actionEntry);
    }
}
