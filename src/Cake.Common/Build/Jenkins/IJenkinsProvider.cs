// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.Jenkins.Data;

namespace Cake.Common.Build.Jenkins
{
    /// <summary>
    /// Represnts a Jenkins Provider
    /// </summary>
    public interface IJenkinsProvider
    {
        /// <summary>
        /// Gets a value indicating whether this instance is running on jenkins.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running on jenkins; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnJenkins { get; }

        /// <summary>
        /// Gets the Bamboo environment.
        /// </summary>
        /// <value>
        /// The Bamboo environment.
        /// </value>
        JenkinsEnvironmentInfo Environment { get; }
    }
}
