// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;

namespace Cake.Common.Diagnostics
{
    /// <summary>
    /// Contains functionality related to logging.
    /// </summary>
    [CakeAliasCategory("Logging")]
    public static class LoggingAliases
    {
        /// <summary>
        /// Writes an error message to the log using the specified format information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <example>
        /// <code>
        /// Error("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Error")]
        public static void Error(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Error(format, args);
        }

        /// <summary>
        /// Writes an error message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        /// <example>
        /// <code>
        /// Error(logAction=>logAction("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Error")]
        public static void Error(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Error(logAction);
        }

        /// <summary>
        /// Writes an error message to the log using the specified value.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Error(new {FirstName = "John", LastName="Doe"});
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Error")]
        public static void Error(this ICakeContext context, object value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Error(value);
        }

        /// <summary>
        /// Writes an error message to the log using the specified string value.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Error("{string}");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Error")]
        public static void Error(this ICakeContext context, string value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Error(value);
        }

        /// <summary>
        /// Writes a warning message to the log using the specified format information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <example>
        /// <code>
        /// Warning("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Warning")]
        public static void Warning(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Warning(format, args);
        }

        /// <summary>
        /// Writes a warning message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        /// <example>
        /// <code>
        /// Warning(logAction=>logAction("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Warning")]
        public static void Warning(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Warning(logAction);
        }

        /// <summary>
        /// Writes an warning message to the log using the specified value.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Warning(new {FirstName = "John", LastName="Doe"});
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Warning")]
        public static void Warning(this ICakeContext context, object value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Warning(value);
        }

        /// <summary>
        /// Writes an warning message to the log using the specified string value.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Warning("{string}");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Warning")]
        public static void Warning(this ICakeContext context, string value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Warning(value);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified format information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <example>
        /// <code>
        /// Information("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Information")]
        public static void Information(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Information(format, args);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        /// <example>
        /// <code>
        /// Information(logAction=>logAction("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Information")]
        public static void Information(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Information(logAction);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Information(new {FirstName = "John", LastName="Doe"});
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Information")]
        public static void Information(this ICakeContext context, object value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Information(value);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified string value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Information("{string}");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Information")]
        public static void Information(this ICakeContext context, string value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Information(value);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified format information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <example>
        /// <code>
        /// Verbose("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Verbose")]
        public static void Verbose(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Verbose(format, args);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        /// <example>
        /// <code>
        /// Verbose(logAction=>logAction("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Verbose")]
        public static void Verbose(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Verbose(logAction);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Verbose(new {FirstName = "John", LastName="Doe"});
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Verbose")]
        public static void Verbose(this ICakeContext context, object value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Verbose(value);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified string value.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Verbose("{string}");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Verbose")]
        public static void Verbose(this ICakeContext context, string value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Verbose(value);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified format information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        /// <example>
        /// <code>
        /// Debug("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Debug")]
        public static void Debug(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Debug(format, args);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        /// <example>
        /// <code>
        /// Debug(logAction=>logAction("Hello {0}! Today is an {1:dddd}", "World", DateTime.Now));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Debug")]
        public static void Debug(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Debug(logAction);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified value.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Debug(new {FirstName = "John", LastName="Doe"});
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Debug")]
        public static void Debug(this ICakeContext context, object value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Debug(value);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified string value.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="value">The value.</param>
        /// <example>
        /// <code>
        /// Debug("{string}");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Debug")]
        public static void Debug(this ICakeContext context, string value)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            context.Log.Debug(value);
        }

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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return context.Log.DiagnosticVerbosity();
        }
    }
}