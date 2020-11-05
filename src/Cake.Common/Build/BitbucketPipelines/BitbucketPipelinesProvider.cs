// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Build.BitbucketPipelines.Data;
using Cake.Core;

namespace Cake.Common.Build.BitbucketPipelines
{
    /// <summary>
    /// Responsible for communicating with Pipelines.
    /// </summary>
    public sealed class BitbucketPipelinesProvider : IBitbucketPipelinesProvider
    {
        /// <inheritdoc/>
        public bool IsRunningOnBitbucketPipelines => !string.IsNullOrWhiteSpace(Environment.Repository.RepoOwner) &&
                                                     !string.IsNullOrWhiteSpace(Environment.Repository.RepoSlug) &&
                                                     !string.IsNullOrWhiteSpace(Environment.Repository.Commit);

        /// <inheritdoc/>
        public BitbucketPipelinesEnvironmentInfo Environment { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitbucketPipelinesProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitbucketPipelinesProvider(ICakeEnvironment environment)
        {
            Environment = new BitbucketPipelinesEnvironmentInfo(environment);
        }
    }
}