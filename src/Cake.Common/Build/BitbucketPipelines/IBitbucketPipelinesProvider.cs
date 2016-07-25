// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.BitbucketPipelines.Data;

namespace Cake.Common.Build.BitbucketPipelines
{
    /// <summary>
    /// Represents a Bitrise provider.
    /// </summary>
    public interface IBitbucketPipelinesProvider
    {
        /// <summary>
        /// Gets a value indicating whether the current build is running on Bitbucket Pipelines.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Bitbucket Pipelines; otherwise, <c>false</c>.
        /// </value>
        bool IsRunningOnBitbucketPipelines { get; }

        /// <summary>
        /// Gets the Bitbucket Pipelines environment.
        /// </summary>
        /// <value>
        /// The Bitbucket Pipelines environment.
        /// </value>
        BitbucketPipelinesEnvironmentInfo Environment { get; }
    }
}