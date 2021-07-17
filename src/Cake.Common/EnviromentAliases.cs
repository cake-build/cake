// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.ComponentModel;
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
        /// Retrieves the value of the environment variable or <c>null</c> if the environment variable does not exist.
        /// </summary>
        /// <example>
        /// <code>
        /// Information(EnvironmentVariable("HOME") ?? "Unknown location");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="variable">The environment variable.</param>
        /// <returns>The environment variable or <c>null</c> if the environment variable does not exist.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Environment Variables")]
        public static string EnvironmentVariable(this ICakeContext context, string variable)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }
            return context.Environment.GetEnvironmentVariable(variable);
        }

        /// <summary>
        /// Retrieves the value of the environment variable or <paramref name="defaultValue"/> if the environment variable does not exist.
        /// </summary>
        /// <example>
        /// <code>
        /// Information(EnvironmentVariable&lt;int&gt;("BUILD_NUMBER", 42));
        /// </code>
        /// </example>
        /// <typeparam name="T">The environment variable type.</typeparam>
        /// <param name="context">The context.</param>
        /// <param name="variable">The environment variable.</param>
        /// <param name="defaultValue">The value to return if the environment variable does not exist.</param>
        /// <returns>The environment variable or <paramref name="defaultValue"/> if the environment variable does not exist.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Environment Variables")]
        public static T EnvironmentVariable<T>(this ICakeContext context, string variable, T defaultValue)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
            }
            var value = context.Environment.GetEnvironmentVariable(variable);
            return value == null ? defaultValue : Convert<T>(value);
        }

        /// <summary>
        /// Retrieves all environment variables.
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
        /// <returns>The environment variables.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Environment Variables")]
        public static IDictionary<string, string> EnvironmentVariables(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
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
                throw new ArgumentNullException(nameof(context));
            }
            if (variable == null)
            {
                throw new ArgumentNullException(nameof(variable));
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
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return context.Environment.Platform.IsWindows();
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
                throw new ArgumentNullException(nameof(context));
            }
            return context.Environment.Platform.IsUnix();
        }

        /// <summary>
        /// Determines whether the build script running on a macOS based system.
        /// </summary>
        /// <example>
        /// <code>
        /// if (IsRunningOnMacOs())
        /// {
        ///     Information("macOS!");
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the build script running on a macOS based system; otherwise <c>false</c>.
        /// </returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Platform")]
        public static bool IsRunningOnMacOs(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return context.Environment.Platform.IsOSX();
        }

        /// <summary>
        /// Determines whether the build script running on a Linux based system.
        /// </summary>
        /// <example>
        /// <code>
        /// if (IsRunningOnLinux())
        /// {
        ///     Information("Linux!");
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <returns>
        ///   <c>true</c> if the build script running on a Linux based system; otherwise <c>false</c>.
        /// </returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Platform")]
        public static bool IsRunningOnLinux(this ICakeContext context)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            return context.Environment.Platform.IsLinux();
        }

        private static T Convert<T>(string value)
        {
            var converter = TypeDescriptor.GetConverter(typeof(T));
            return (T)converter.ConvertFromInvariantString(value);
        }
    }
}
