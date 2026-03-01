using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;

namespace Cake.Common.Diagnostics;

public static partial class LoggingAliases
{
    /// <summary>
    /// Writes an error message to the log using the specified format information.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattable">The string to be formatted.</param>
    /// <example>
    /// <code>
    /// Error($"Hello {"World"}! Today is an {DateTime.Now:dddd}");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Error")]
    public static void Error(this ICakeContext context, FormattableString formattable)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Error(formattable);
    }

    /// <summary>
    /// Writes an warning message to the log using the specified format information.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattable">The string to be formatted.</param>
    /// <example>
    /// <code>
    /// Warning($"Hello {"World"}! Today is an {DateTime.Now:dddd}");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Warning")]
    public static void Warning(this ICakeContext context, FormattableString formattable)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Warning(formattable);
    }

    /// <summary>
    /// Writes an informational message to the log using the specified formattable string.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattable">The string to be formatted.</param>
    /// <example>
    /// <code>
    /// Information($"Hello {"World"}! Today is an {DateTime.Now:dddd}");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Information")]
    public static void Information(this ICakeContext context, FormattableString formattable)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Information(formattable);
    }

    /// <summary>
    /// Writes an verbose message to the log using the specified formattable string.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattable">The string to be formatted.</param>
    /// <example>
    /// <code>
    /// Verbose($"Hello {"World"}! Today is an {DateTime.Now:dddd}");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Verbose")]
    public static void Verbose(this ICakeContext context, FormattableString formattable)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Verbose(formattable);
    }

    /// <summary>
    /// Writes an debug message to the log using the specified formattable string.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattable">The string to be formatted.</param>
    /// <example>
    /// <code>
    /// Debug($"Hello {"World"}! Today is an {DateTime.Now:dddd}");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Debug")]
    public static void Debug(this ICakeContext context, FormattableString formattable)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Debug(formattable);
    }

    /// <summary>
    /// Writes an error message to the log using the specified format information.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattableLogAction">The log action.</param>
    /// <example>
    /// <code>
    /// Error(logAction => logAction($"Hello {"World"}! Today is an {DateTime.Now:dddd}"));
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Error")]
    public static void Error(this ICakeContext context, FormattableLogAction formattableLogAction)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Error(formattableLogAction);
    }

    /// <summary>
    /// Writes an warning message to the log using the specified format information.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattableLogAction">The log action.</param>
    /// <example>
    /// <code>
    /// Warning(logAction => logAction($"Hello {"World"}! Today is an {DateTime.Now:dddd}"));
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Warning")]
    public static void Warning(this ICakeContext context, FormattableLogAction formattableLogAction)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Warning(formattableLogAction);
    }

    /// <summary>
    /// Writes an informational message to the log using the specified formattable string.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattableLogAction">The log action.</param>
    /// <example>
    /// <code>
    /// Information(logAction => logAction($"Hello {"World"}! Today is an {DateTime.Now:dddd}"));
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Information")]
    public static void Information(this ICakeContext context, FormattableLogAction formattableLogAction)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Information(formattableLogAction);
    }

    /// <summary>
    /// Writes an verbose message to the log using the specified formattable string.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattableLogAction">The log action.</param>
    /// <example>
    /// <code>
    /// Verbose(logAction => logAction($"Hello {"World"}! Today is an {DateTime.Now:dddd}"));
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Verbose")]
    public static void Verbose(this ICakeContext context, FormattableLogAction formattableLogAction)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Verbose(formattableLogAction);
    }

    /// <summary>
    /// Writes an debug message to the log using the specified formattable string.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="formattableLogAction">The log action.</param>
    /// <example>
    /// <code>
    /// Debug(logAction => logAction($"Hello {"World"}! Today is an {DateTime.Now:dddd}"));
    /// </code>
    /// </example>
    [CakeMethodAlias]
    [CakeAliasCategory("Debug")]
    public static void Debug(this ICakeContext context, FormattableLogAction formattableLogAction)
    {
        ArgumentNullException.ThrowIfNull(context);
        context.Log.Debug(formattableLogAction);
    }
}
