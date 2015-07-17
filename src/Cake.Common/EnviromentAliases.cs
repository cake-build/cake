﻿using System;
using Cake.Core;
using Cake.Core.Annotations;

namespace Cake.Common
{
    /// <summary>
    /// Contains functionality related to the environment.
    /// </summary>
    [CakeAliasCategory("Environment")]
    public static class EnvironmentAliases
    {
        /// <summary>
        /// Retrieves the value of the environment variable or <c>null</c> if the environment variable do not exist.
        /// </summary>
        /// <example>
        /// <code>
        /// Information(EnvironmentVariable("HOME") ?? "Unknown location");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="variable">The environment variable.</param>
        /// <returns>The environment variable or <c>null</c> if the environment variable do not exist.</returns>
        [CakeMethodAlias]
        public static string EnvironmentVariable(this ICakeContext context, string variable)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (variable == null)
            {
                throw new ArgumentNullException("variable");
            }
            return context.Environment.GetEnvironmentVariable(variable);
        }

        /// <summary>
        /// Checks for the existence of a value for a given environment variable.
        /// </summary>
        /// <example>
        /// <code>
        /// if (HasEnvironmentVariable("SOME_ENVIRONMENT_VARIABLE"))
        /// {
        ///     Information("The environment variable was present.");
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="variable">The environment variable.</param>
        /// <returns>
        ///   <c>true</c> if the environment variable exist; otherwise <c>false</c>.
        /// </returns>
        [CakeMethodAlias]
        public static bool HasEnvironmentVariable(this ICakeContext context, string variable)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (variable == null)
            {
                throw new ArgumentNullException("variable");
            }
            return context.Environment.GetEnvironmentVariable(variable) != null;
        }

        /// <summary>
        /// Determines whether the build script is running on Windows.
        /// </summary>
        /// <example>
        /// <code>
        /// if (IsRunningOnWindows())
        /// {
        ///     Information("Windows!");
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the build script is running on Windows; otherwise <c>false</c>.
        /// </returns>
        [CakeMethodAlias]
        public static bool IsRunningOnWindows(this ICakeContext context)
        {
            return !IsRunningOnUnix(context);
        }

        /// <summary>
        /// Determines whether the build script running on a Unix or Linux based system.
        /// </summary>
        /// <example>
        /// <code>
        /// if (IsRunningOnUnix())
        /// {
        ///     Information("Not Windows!");
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the build script running on a Unix or Linux based system; otherwise <c>false</c>.
        /// </returns>
        [CakeMethodAlias]
        public static bool IsRunningOnUnix(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.Environment.IsUnix();
        }
    }
}
