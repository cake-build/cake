// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.BitbucketPipelines.Data
{
    /// <summary>
    /// Provides Bitbucket Pipelines environment information for the current build.
    /// </summary>
    public class BitbucketPipelinesEnvironmentInfo : BitbucketPipelinesInfo
    {
         private readonly BitbucketPipelinesRepositoryInfo _repositoryProvider;

        /// <summary>
        /// Gets Bitbucket Pipelines repository information.
        /// </summary>
        /// <value>
        /// The repository.
        /// </value>
        public BitbucketPipelinesRepositoryInfo Repository
        {
            get { return _repositoryProvider; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitbucketPipelinesEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitbucketPipelinesEnvironmentInfo(ICakeEnvironment environment) : base(environment)
        {
            _repositoryProvider = new BitbucketPipelinesRepositoryInfo(environment);
        }
    }
}