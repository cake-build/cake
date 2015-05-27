using System;
using Cake.Core.IO;
using Cake.Core.IO.Arguments;

namespace Cake.Core
{
    /// <summary>
    /// Contains extension methods for <see cref="ProcessSettings" />.
    /// </summary>
    public static class ProcessSettingsExtensions
    {
        /// <summary>
        /// Sets the working directory for the process to be started.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="path">The working directory for the process to be started.</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings SetWorkingDirectory(this ProcessSettings settings, DirectoryPath path)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.WorkingDirectory = path;
            return settings;
        }

        /// <summary>
        /// Sets a value indicating whether the output of an application is written to the <see cref="P:System.Diagnostics.Process.StandardOutput"/> stream.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="redirect">true if output should be written to <see cref="P:System.Diagnostics.Process.StandardOutput"/>; otherwise, false. The default is false.</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings SetRedirectStandardOutput(this ProcessSettings settings, bool redirect)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.RedirectStandardOutput = redirect;
            return settings;
        }

        /// <summary>
        /// Sets the optional timeout for process execution
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="timeout">The timeout duration</param>
        /// <returns>The same <see cref="ProcessSettings"/> instance so that multiple calls can be chained.</returns>
        public static ProcessSettings SetTimeout(this ProcessSettings settings, int? timeout)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            settings.Timeout = timeout;
            return settings;
        }
    }
}
