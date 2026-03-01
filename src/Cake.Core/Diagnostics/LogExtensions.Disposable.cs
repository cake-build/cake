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
        ArgumentNullException.ThrowIfNull(log);
        var lastVerbosity = log.Verbosity;
        log.Verbosity = verbosity;
        return Disposable.Create(() => log.Verbosity = lastVerbosity);
    }
}