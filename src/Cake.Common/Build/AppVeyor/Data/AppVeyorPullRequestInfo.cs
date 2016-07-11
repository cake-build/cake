// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.AppVeyor.Data
{
    /// <summary>
    /// Provides AppVeyor pull request information for a current build.
    /// </summary>
    public sealed class AppVeyorPullRequestInfo : AppVeyorInfo
    {
        /// <summary>
        /// Gets a value indicating whether the current build was started by a pull request.
        /// </summary>
        /// <value>
        ///   <c>true</c> if the current build was started by a pull request; otherwise, <c>false</c>.
        /// </value>
        public bool IsPullRequest
        {
            get { return Number > 0; }
        }

        /// <summary>
        /// Gets the GitHub pull request number.
        /// </summary>
        /// <value>
        ///   The GitHub pull request number.
        /// </value>
        public int Number
        {
            get { return GetEnvironmentInteger("APPVEYOR_PULL_REQUEST_NUMBER"); }
        }

        /// <summary>
        /// Gets the GitHub pull request title.
        /// </summary>
        /// <value>
        ///   The GitHub pull request title.
        /// </value>
        public string Title
        {
            get { return GetEnvironmentString("APPVEYOR_PULL_REQUEST_TITLE"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppVeyorPullRequestInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public AppVeyorPullRequestInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}
