﻿using System;
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
        [CakeMethodAlias]
        public static void Error(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Error(format, args);
        }
        
        /// <summary>
        /// Writes an error message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        [CakeMethodAlias]
        public static void Error(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Error(logAction);
        }

        /// <summary>
        /// Writes a warning message to the log using the specified format information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        [CakeMethodAlias]
        public static void Warning(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Warning(format, args);
        }

        /// <summary>
        /// Writes a warning message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        [CakeMethodAlias]
        public static void Warning(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Warning(logAction);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified format information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        [CakeMethodAlias]
        public static void Information(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Information(format, args);
        }

        /// <summary>
        /// Writes an informational message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        [CakeMethodAlias]
        public static void Information(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Information(logAction);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified format information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        [CakeMethodAlias]
        public static void Verbose(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Verbose(format, args);
        }

        /// <summary>
        /// Writes a verbose message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        [CakeMethodAlias]
        public static void Verbose(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Verbose(logAction);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified format information.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="format">The format.</param>
        /// <param name="args">The arguments.</param>
        [CakeMethodAlias]
        public static void Debug(this ICakeContext context, string format, params object[] args)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Debug(format, args);
        }

        /// <summary>
        /// Writes a debug message to the log using the specified log message action.
        /// Evaluation message only if verbosity same or more verbose.
        /// </summary>
        /// <param name="context">the context.</param>
        /// <param name="logAction">The function called for message when logging.</param>
        [CakeMethodAlias]
        public static void Debug(this ICakeContext context, LogAction logAction)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            context.Log.Debug(logAction);
        }
    }
}
