// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.WoodpeckerCI.Data
{
    /// <summary>
    /// Provides WoodpeckerCI system information for the current build.
    /// </summary>
    public class WoodpeckerCISystemInfo : WoodpeckerCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCISystemInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public WoodpeckerCISystemInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the system name.
        /// </summary>
        /// <value>
        /// The system name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"System Name: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.System.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via WoodpeckerCI.</para>
        /// <example>
        /// <code>
        /// if (WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"System Name: {0}",
        ///         WoodpeckerCI.Environment.System.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public string Name => GetEnvironmentString("CI_SYSTEM_NAME");

        /// <summary>
        /// Gets the system URL.
        /// </summary>
        /// <value>
        /// The system URL.
        /// </value>
        public string Url => GetEnvironmentString("CI_SYSTEM_URL");

        /// <summary>
        /// Gets the system host.
        /// </summary>
        /// <value>
        /// The system host.
        /// </value>
        public string Host => GetEnvironmentString("CI_SYSTEM_HOST");

        /// <summary>
        /// Gets the system version.
        /// </summary>
        /// <value>
        /// The system version.
        /// </value>
        public string Version => GetEnvironmentString("CI_SYSTEM_VERSION");
    }
}
