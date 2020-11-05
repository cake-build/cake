// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.Bitrise.Data;

namespace Cake.Common.Build.Bitrise
{
    /// <summary>
    /// Represents a Bitrise provider.
    /// </summary>
    public interface IBitriseProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on Bitrise.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Bitrise; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnBitrise { get; }

        /// <summary>
        /// Gets the Bitrise environment.
        /// </summary>
        /// <value>
        /// The Bitrise environment.
        /// </value>
        BitriseEnvironmentInfo Environment { get; }

        /// <summary>
        /// Sets and environment variable that can be used in next steps on Bitrise.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="value">The value.</param>
        void SetEnvironmentString(string variable, string value);
    }
}
