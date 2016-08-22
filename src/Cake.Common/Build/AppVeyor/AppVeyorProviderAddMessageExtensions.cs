// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Cake.Common.Build.AppVeyor
{
    /// <summary>
    /// AddMessage extension methods for the IAppVeyorProvider
    /// </summary>
    public static class AppVeyorProviderAddMessageExtensions
    {
        /// <summary>
        /// Adds an informational message to the AppVeyor build log
        /// </summary>
        /// <param name="provider">The AppVeyor provider</param>
        /// <param name="format">The message</param>
        /// <param name="args">The args</param>
        public static void AddInformationalMessage(this IAppVeyorProvider provider, string format, params object[] args)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            provider.AddMessage(string.Format(CultureInfo.InvariantCulture, format, args));
        }

        /// <summary>
        /// Adds a warning message to the AppVeyor build log
        /// </summary>
        /// <param name="provider">The AppVeyor provider</param>
        /// <param name="format">The message</param>
        /// <param name="args">The args</param>
        public static void AddWarningMessage(this IAppVeyorProvider provider, string format, params object[] args)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            provider.AddMessage(string.Format(CultureInfo.InvariantCulture, format, args), AppVeyorMessageCategoryType.Warning);
        }

        /// <summary>
        /// Adds a warning message to the AppVeyor build log
        /// </summary>
        /// <param name="provider">The AppVeyor provider</param>
        /// <param name="format">The message</param>
        /// <param name="args">The args</param>
        public static void AddErrorMessage(this IAppVeyorProvider provider, string format, params object[] args)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            provider.AddMessage(string.Format(CultureInfo.InvariantCulture, format, args), AppVeyorMessageCategoryType.Error);
        }

        /// <summary>
        /// Adds a warning message to the AppVeyor build log
        /// </summary>
        /// <param name="provider">The AppVeyor provider</param>
        /// <param name="message">The message</param>
        /// <param name="exception">The exception</param>
        public static void AddErrorMessage(this IAppVeyorProvider provider, string message, Exception exception)
        {
            if (provider == null)
            {
                throw new ArgumentNullException(nameof(provider));
            }
            provider.AddMessage(message, AppVeyorMessageCategoryType.Error,
                exception?.ToString());
        }
    }
}