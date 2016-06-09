// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.Bamboo.Data;

namespace Cake.Common.Build.Bamboo
{
    /// <summary>
    /// Represents a Bamboo provider.
    /// </summary>
    public interface IBambooProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on Bamboo.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Bamboo; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnBamboo { get; }

        /// <summary>
        /// Gets the Bamboo environment.
        /// </summary>
        /// <value>
        /// The Bamboo environment.
        /// </value>
        BambooEnvironmentInfo Environment { get; }
    }
}
