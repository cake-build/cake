// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
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
        [CakeAliasCategory("Environment Variables")]
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
        /// Retrieves all environment variables
        /// </summary>
        /// <example>
        /// <code>
        /// var envVars = EnvironmentVariables();
        ///
        /// string path;
        /// if (envVars.TryGetValue("PATH", out path))
        /// {
        ///     Information("Path: {0}", path);
        /// }
        ///
        /// foreach(var envVar in envVars)
        /// {
        ///     Information(
        ///         "Key: {0}\tValue: \"{1}\"",
        ///         envVar.Key,
        ///         envVar.Value
        ///         );
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>The environment variables</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Environment Variables")]
        public static IDictionary<string, string> EnvironmentVariables(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            return context.Environment.GetEnvironmentVariables();
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
        [CakeAliasCategory("Environment Variables")]
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
        [CakeAliasCategory("Platform")]
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
        [CakeAliasCategory("Platform")]
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
