// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;

namespace Cake.Common.Build.AppVeyor;

/// <summary>
/// AddMessage extension methods for the IAppVeyorProvider.
/// </summary>
public static class AppVeyorProviderAddMessageExtensions
{
    /// <summary>
    /// Adds an informational message to the AppVeyor build log.
    /// </summary>
    /// <param name="provider">The AppVeyor provider.</param>
    /// <param name="format">The message.</param>
    /// <param name="args">The args.</param>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    /// {
    ///     BuildSystem.AppVeyor.AddInformationalMessage("Version {0}", "1.0.0");
    /// }
    /// </code>
    /// </example>
    /// <para>Via AppVeyor.</para>
    /// <example>
    /// <code>
    /// if (AppVeyor.IsRunningOnAppVeyor)
    /// {
    ///     AppVeyor.AddInformationalMessage("Version {0}", "1.0.0");
    /// }
    /// </code>
    /// </example>
    public static void AddInformationalMessage(this IAppVeyorProvider provider, string format, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(provider);
        provider.AddMessage(string.Format(CultureInfo.InvariantCulture, format, args));
    }

    /// <summary>
    /// Adds a warning message to the AppVeyor build log.
    /// </summary>
    /// <param name="provider">The AppVeyor provider.</param>
    /// <param name="format">The message.</param>
    /// <param name="args">The args.</param>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    /// {
    ///     BuildSystem.AppVeyor.AddWarningMessage("Skipped {0}", "optional step");
    /// }
    /// </code>
    /// </example>
    /// <para>Via AppVeyor.</para>
    /// <example>
    /// <code>
    /// if (AppVeyor.IsRunningOnAppVeyor)
    /// {
    ///     AppVeyor.AddWarningMessage("Skipped {0}", "optional step");
    /// }
    /// </code>
    /// </example>
    public static void AddWarningMessage(this IAppVeyorProvider provider, string format, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(provider);
        provider.AddMessage(string.Format(CultureInfo.InvariantCulture, format, args), AppVeyorMessageCategoryType.Warning);
    }

    /// <summary>
    /// Adds a warning message to the AppVeyor build log.
    /// </summary>
    /// <param name="provider">The AppVeyor provider.</param>
    /// <param name="format">The message.</param>
    /// <param name="args">The args.</param>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    /// {
    ///     BuildSystem.AppVeyor.AddErrorMessage("Failed to publish {0}", packageId);
    /// }
    /// </code>
    /// </example>
    /// <para>Via AppVeyor.</para>
    /// <example>
    /// <code>
    /// if (AppVeyor.IsRunningOnAppVeyor)
    /// {
    ///     AppVeyor.AddErrorMessage("Failed to publish {0}", packageId);
    /// }
    /// </code>
    /// </example>
    public static void AddErrorMessage(this IAppVeyorProvider provider, string format, params object[] args)
    {
        ArgumentNullException.ThrowIfNull(provider);
        provider.AddMessage(string.Format(CultureInfo.InvariantCulture, format, args), AppVeyorMessageCategoryType.Error);
    }

    /// <summary>
    /// Adds a warning message to the AppVeyor build log.
    /// </summary>
    /// <param name="provider">The AppVeyor provider.</param>
    /// <param name="message">The message.</param>
    /// <param name="exception">The exception.</param>
    /// <para>Via BuildSystem.</para>
    /// <example>
    /// <code>
    /// if (BuildSystem.AppVeyor.IsRunningOnAppVeyor)
    /// {
    ///     try
    ///     {
    ///         // Do work...
    ///     }
    ///     catch (Exception ex)
    ///     {
    ///         BuildSystem.AppVeyor.AddErrorMessage("Publish failed", ex);
    ///         throw;
    ///     }
    /// }
    /// </code>
    /// </example>
    /// <para>Via AppVeyor.</para>
    /// <example>
    /// <code>
    /// if (AppVeyor.IsRunningOnAppVeyor)
    /// {
    ///     try
    ///     {
    ///         // Do work...
    ///     }
    ///     catch (Exception ex)
    ///     {
    ///         AppVeyor.AddErrorMessage("Publish failed", ex);
    ///         throw;
    ///     }
    /// }
    /// </code>
    /// </example>
    public static void AddErrorMessage(this IAppVeyorProvider provider, string message, Exception exception)
    {
        ArgumentNullException.ThrowIfNull(provider);
        provider.AddMessage(message, AppVeyorMessageCategoryType.Error,
            exception?.ToString());
    }
}
