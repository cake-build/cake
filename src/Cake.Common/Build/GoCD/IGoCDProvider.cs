// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.GoCD.Data;

namespace Cake.Common.Build.GoCD
{
    /// <summary>
    /// Represents a Go.CD provider.
    /// </summary>
    public interface IGoCDProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on Go.CD.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Go.CD; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnGoCD { get; }

        /// <summary>
        /// Gets the Go.CD environment.
        /// </summary>
        /// <value>
        /// The Go.CD environment.
        /// </value>
        GoCDEnvironmentInfo Environment { get; }

        /// <summary>
        /// Gets the Go.CD build history, including the repository modifications that caused the pipeline to start.
        /// </summary>
        /// <param name="username">The Go.CD username.</param>
        /// <param name="password">The Go.CD password.</param>
        /// <returns>The Go.CD build history.</returns>
        GoCDHistoryInfo GetHistory(string username, string password);

        /// <summary>
        /// Gets the Go.CD build history, including the repository modifications that caused the pipeline to start.
        /// </summary>
        /// <param name="username">The Go.CD username.</param>
        /// <param name="password">The Go.CD password.</param>
        /// <param name="serverUrl">The Go.CD server URL.</param>
        /// <returns>The Go.CD build history.</returns>
        GoCDHistoryInfo GetHistory(string username, string password, string serverUrl);
    }
}
