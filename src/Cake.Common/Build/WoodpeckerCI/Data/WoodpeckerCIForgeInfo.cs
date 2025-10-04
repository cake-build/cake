// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.WoodpeckerCI.Data
{
    /// <summary>
    /// Provides WoodpeckerCI forge information for the current build.
    /// </summary>
    public class WoodpeckerCIForgeInfo : WoodpeckerCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCIForgeInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public WoodpeckerCIForgeInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the forge type.
        /// </summary>
        /// <value>
        /// The forge type.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Forge Type: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.Forge.Type
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
        ///         @"Forge Type: {0}",
        ///         WoodpeckerCI.Environment.Forge.Type
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public WoodpeckerCIForgeType Type => WoodpeckerCIForgeTypeExtensions.ParseForgeType(GetEnvironmentString("CI_FORGE_TYPE"));

        /// <summary>
        /// Gets the forge URL.
        /// </summary>
        /// <value>
        /// The forge URL.
        /// </value>
        public Uri Url => GetEnvironmentUri("CI_FORGE_URL");
    }
}
