// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.WoodpeckerCI.Data
{
    /// <summary>
    /// Provides WoodpeckerCI step information for the current build.
    /// </summary>
    public class WoodpeckerCIStepInfo : WoodpeckerCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCIStepInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public WoodpeckerCIStepInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the step name.
        /// </summary>
        /// <value>
        /// The step name.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.WoodpeckerCI.IsRunningOnWoodpeckerCI)
        /// {
        ///     Information(
        ///         @"Step Name: {0}",
        ///         BuildSystem.WoodpeckerCI.Environment.Step.Name
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
        ///         @"Step Name: {0}",
        ///         WoodpeckerCI.Environment.Step.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on WoodpeckerCI");
        /// }
        /// </code>
        /// </example>
        public string Name => GetEnvironmentString("CI_STEP_NAME");

        /// <summary>
        /// Gets the step number.
        /// </summary>
        /// <value>
        /// The step number.
        /// </value>
        public int Number => GetEnvironmentInteger("CI_STEP_NUMBER");

        /// <summary>
        /// Gets the step started timestamp.
        /// </summary>
        /// <value>
        /// The step started timestamp.
        /// </value>
        public DateTimeOffset Started => GetEnvironmentDateTimeOffset("CI_STEP_STARTED");

        /// <summary>
        /// Gets the step URL.
        /// </summary>
        /// <value>
        /// The step URL.
        /// </value>
        public Uri Url => GetEnvironmentUri("CI_STEP_URL");
    }
}
