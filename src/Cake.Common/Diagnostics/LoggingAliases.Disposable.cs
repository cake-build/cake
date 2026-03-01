// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;

namespace Cake.Common.Diagnostics;

public static partial class LoggingAliases
{
    /// <summary>
    /// Sets the log verbosity to quiet and returns a disposable that restores the log verbosity on dispose.
    /// </summary>
    /// <param name="context">the context.</param>
    /// <returns>A disposable that restores the log verbosity.</returns>
    /// <example>
    /// <code>
    /// using (QuietVerbosity())
    /// {
    ///     Error("Show me.");
    ///     Warning("Hide me.");
    ///     Information("Hide me.");
    ///     Verbose("Hide me.");
    ///     Debug("Hide me.");
    /// }
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Verbosity")]
    public static IDisposable QuietVerbosity(this ICakeContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Log.QuietVerbosity();
    }

    /// <summary>
    /// Sets the log verbosity to minimal and returns a disposable that restores the log verbosity on dispose.
    /// </summary>
    /// <param name="context">the context.</param>
    /// <returns>A disposable that restores the log verbosity.</returns>
    /// <example>
    /// <code>
    /// using (MinimalVerbosity())
    /// {
    ///     Error("Show me.");
    ///     Warning("Show me.");
    ///     Information("Hide me.");
    ///     Verbose("Hide me.");
    ///     Debug("Hide me.");
    /// }
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Verbosity")]
    public static IDisposable MinimalVerbosity(this ICakeContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Log.MinimalVerbosity();
    }

    /// <summary>
    /// Sets the log verbosity to normal and returns a disposable that restores the log verbosity on dispose.
    /// </summary>
    /// <param name="context">the context.</param>
    /// <returns>A disposable that restores the log verbosity.</returns>
    /// <example>
    /// <code>
    /// using (NormalVerbosity())
    /// {
    ///     Error("Show me.");
    ///     Warning("Show me.");
    ///     Information("Show me.");
    ///     Verbose("Hide me.");
    ///     Debug("Hide me.");
    /// }
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Verbosity")]
    public static IDisposable NormalVerbosity(this ICakeContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Log.NormalVerbosity();
    }

    /// <summary>
    /// Sets the log verbosity to verbose and returns a disposable that restores the log verbosity on dispose.
    /// </summary>
    /// <param name="context">the context.</param>
    /// <returns>A disposable that restores the log verbosity.</returns>
    /// <example>
    /// <code>
    /// using (VerboseVerbosity())
    /// {
    ///     Error("Show me.");
    ///     Warning("Show me.");
    ///     Information("Show me.");
    ///     Verbose("Show me.");
    ///     Debug("Hide me.");
    /// }
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Verbosity")]
    public static IDisposable VerboseVerbosity(this ICakeContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Log.VerboseVerbosity();
    }

    /// <summary>
    /// Sets the log verbosity to diagnostic and returns a disposable that restores the log verbosity on dispose.
    /// </summary>
    /// <param name="context">the context.</param>
    /// <returns>A disposable that restores the log verbosity.</returns>
    /// <example>
    /// <code>
    /// using (DiagnosticVerbosity())
    /// {
    ///     Error("Show me.");
    ///     Warning("Show me.");
    ///     Information("Show me.");
    ///     Verbose("Show me.");
    ///     Debug("Show me.");
    /// }
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Verbosity")]
    public static IDisposable DiagnosticVerbosity(this ICakeContext context)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Log.DiagnosticVerbosity();
    }

    /// <summary>
    /// Sets the log verbosity as specified and returns a disposable that restores the log verbosity on dispose.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="verbosity">The verbosity.</param>
    /// <returns>A disposable that restores the log verbosity.</returns>
    /// <example>
    /// <code>
    /// using (DiagnosticVerbosity())
    /// {
    ///     Error("Show me.");
    ///     Warning("Show me.");
    ///     Information("Show me.");
    ///     Verbose("Show me.");
    ///     Debug("Show me.");
    /// }
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Verbosity")]
    public static IDisposable WithVerbosity(this ICakeContext context, Verbosity verbosity)
    {
        ArgumentNullException.ThrowIfNull(context);
        return context.Log.WithVerbosity(verbosity);
    }
}
