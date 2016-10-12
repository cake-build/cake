// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.GitLabCI.Data
{
    /// <summary>
    /// Provides GitLab CI runner information for a current build.
    /// </summary>
    public sealed class GitLabCIRunnerInfo : GitLabCIInfo
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabCIRunnerInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GitLabCIRunnerInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }

        /// <summary>
        /// Gets the unique id of runner being used.
        /// </summary>
        /// <value>
        /// The unique id of runner being used.
        /// </value>
        public int Id => GetEnvironmentInteger("CI_RUNNER_ID");

        /// <summary>
        /// Gets the description of the runner as saved in GitLab.
        /// </summary>
        /// <value>
        /// The description of the runner as saved in GitLab.
        /// </value>
        public string Description => GetEnvironmentString("CI_RUNNER_DESCRIPTION");

        /// <summary>
        /// Gets an array of the defined runner tags.
        /// </summary>
        /// <value>
        /// The defined runner tags.
        /// </value>
        public string[] Tags
        {
            get
            {
                var tags = GetEnvironmentString("CI_RUNNER_TAGS").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
                for (int i = 0; i < tags.Length; i++)
                {
                    tags[i] = tags[i].Trim();
                }
                return tags;
            }
        }
    }
}
