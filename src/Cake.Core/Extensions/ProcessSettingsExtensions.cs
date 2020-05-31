// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

// ReSharper disable once CheckNamespace
namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="ProcessSettings" />.
    /// </summary>
    public static class ProcessSettingsExtensions
    {
        /// <summary>
        /// Sets the arguments for the process.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="arguments">The action used to set arguments.</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings WithArguments(this ProcessSettings settings, Action<ProcessArgumentBuilder> arguments)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (arguments == null)
            {
                throw new ArgumentNullException(nameof(arguments));
            }

            if (settings.Arguments == null)
            {
                settings.Arguments = new ProcessArgumentBuilder();
            }

            arguments(settings.Arguments);
            return settings;
        }

        /// <summary>
        /// Sets the working directory for the process to be started.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="path">The working directory for the process to be started.</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings UseWorkingDirectory(this ProcessSettings settings, DirectoryPath path)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            settings.WorkingDirectory = path;
            return settings;
        }

        /// <summary>
        /// Sets a function that intercepts the standard output before being redirected. Use in conjunction with <see cref="ProcessSettings.RedirectStandardOutput"/>.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="handler">The standard output handler.</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings SetRedirectedStandardOutputHandler(this ProcessSettings settings, Func<string, string> handler)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.RedirectedStandardOutputHandler = handler;
            return settings;
        }

        /// <summary>
        /// Sets a function that intercepts the standard error before being redirected. Use in conjunction with <see cref="ProcessSettings.RedirectStandardOutput"/>.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="handler">The standard error handler.</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings SetRedirectedStandardErrorHandler(this ProcessSettings settings, Func<string, string> handler)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.RedirectedStandardErrorHandler = handler;
            return settings;
        }

        /// <summary>
        /// Sets a value indicating whether the output of an application is written to the standard output stream.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="redirect">true if output should be redirected; false if output should be written to the standard output stream. The default is false.</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings SetRedirectStandardOutput(this ProcessSettings settings, bool redirect)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.RedirectStandardOutput = redirect;
            return settings;
        }

        /// <summary>
        /// Sets a value indicating whether the standard error of an application is written to the standard error stream.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="redirect">true if error output should be redirected; false if error output should be written to the standard error stream. The default is false.</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings SetRedirectStandardError(this ProcessSettings settings, bool redirect)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.RedirectStandardError = redirect;
            return settings;
        }

        /// <summary>
        /// Sets the optional timeout for process execution.
        /// </summary>
        /// <param name="settings">The process settings.</param>
        /// <param name="timeout">The timeout duration.</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings SetTimeout(this ProcessSettings settings, int timeout)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }

            settings.Timeout = timeout;
            return settings;
        }
    }
}