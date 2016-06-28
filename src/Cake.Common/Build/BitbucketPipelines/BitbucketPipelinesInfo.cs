// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.BitbucketPipelines
{
    /// <summary>
    /// Base class used to provide information about the Bitbucket Pipelines environment.
    /// </summary>
    public abstract class BitbucketPipelinesInfo
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="BitbucketPipelinesInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        protected BitbucketPipelinesInfo(ICakeEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.String"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected string GetEnvironmentString(string variable)
        {
            return _environment.GetEnvironmentVariable(variable) ?? string.Empty;
        }
    }
}