// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Cli.Infrastructure
{
    /// <summary>
    /// Constants used by the Cake CLI.
    /// </summary>
    internal static class Constants
    {
        /// <summary>
        /// Configuration key names for CLI settings.
        /// </summary>
        public static class Settings
        {
            /// <summary>
            /// Configuration key for disabling the build report.
            /// </summary>
            public const string NoReport = "Settings_NoReport";

            /// <summary>
            /// Configuration key for using a unified dependency graph when running multiple targets.
            /// </summary>
            public const string UnifiedDependencyGraphForMultipleTargets = "Settings_UnifiedDependencyGraphForMultipleTargets";
        }
    }
}
