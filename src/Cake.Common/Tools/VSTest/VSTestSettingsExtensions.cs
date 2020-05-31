// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Common.Tools.VSTest
{
    /// <summary>
    /// Contains functionality related to VSTest settings.
    /// </summary>
    public static class VSTestSettingsExtensions
    {
        /// <summary>
        /// Do not Log.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="VSTestSettings"/> instance so that multiple calls can be chained.</returns>
        public static VSTestSettings WithoutAnyLogger(this VSTestSettings settings)
        {
            return settings.WithLogger(string.Empty);
        }

        /// <summary>
        /// Log to a trx file.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="VSTestSettings"/> instance so that multiple calls can be chained.</returns>
        public static VSTestSettings WithVisualStudioLogger(this VSTestSettings settings)
        {
            return settings.WithLogger("trx");
        }

        /// <summary>
        /// Log to the AppVeyor logger (which is only available when building your solution on the AppVeyor platform).
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The same <see cref="VSTestSettings"/> instance so that multiple calls can be chained.</returns>
        public static VSTestSettings WithAppVeyorLogger(this VSTestSettings settings)
        {
            return settings.WithLogger("AppVeyor");
        }

        /// <summary>
        /// Log to a custom logger.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <param name="loggerName">The name of the logger.</param>
        /// <returns>The same <see cref="VSTestSettings"/> instance so that multiple calls can be chained.</returns>
        public static VSTestSettings WithLogger(this VSTestSettings settings, string loggerName)
        {
            if (settings == null)
            {
                throw new ArgumentNullException(nameof(settings));
            }
            settings.Logger = loggerName;
            return settings;
        }
    }
}