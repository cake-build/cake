// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core;

/// <summary>
/// Provides extension methods for <see cref="CakeTaskExecutionStatus"/>.
/// </summary>
public static class CakeTaskExecutionStatusExtensions
{
    /// <summary>
    /// Converts a <see cref="CakeTaskExecutionStatus"/> value to a report status string.
    /// </summary>
    /// <param name="status">The task execution status.</param>
    /// <returns>
    /// "Succeeded" for <see cref="CakeTaskExecutionStatus.Executed"/> and <see cref="CakeTaskExecutionStatus.Delegated"/>,
    /// "Skipped" for <see cref="CakeTaskExecutionStatus.Skipped"/>,
    /// "Failed" for <see cref="CakeTaskExecutionStatus.Failed"/>,
    /// or the string representation of the status as fallback.
    /// </returns>
    public static string ToReportStatus(this CakeTaskExecutionStatus status)
        => status switch {
                CakeTaskExecutionStatus.Executed => "Succeeded",
                CakeTaskExecutionStatus.Delegated => "Succeeded",
                CakeTaskExecutionStatus.Skipped => "Skipped",
                CakeTaskExecutionStatus.Failed => "Failed",
                _ => status.ToString("F")
            };
}
