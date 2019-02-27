// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.TravisCI.Data
{
    /// <summary>
    /// Provides TravisCI pull request information for a current build.
    /// </summary>
    public sealed class TravisCIPullRequestInfo : TravisCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="TravisCIPullRequestInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TravisCIPullRequestInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets a value indicating whether the current build was started by a pull request.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current build was started by a pull request; otherwise, <c>false</c>.
        /// </value>
        public bool IsPullRequest => Id > 0;

        /// <summary>
        /// Gets the pull request id.
        /// </summary>
        /// <value>
        ///   The pull request id.
        /// </value>
        public int Id => GetEnvironmentInteger("TRAVIS_PULL_REQUEST");
    }
}