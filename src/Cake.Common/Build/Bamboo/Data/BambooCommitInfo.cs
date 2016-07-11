// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bamboo.Data
{
    /// <summary>
    /// Provides Bamboo commit information for a current build.
    /// </summary>
    public sealed class BambooCommitInfo : BambooInfo
    {
        /// <summary>
        /// Gets the revision use to build this release. Format depends on the VCS used.
        /// </summary>
        /// <value>
        ///   The commit ID (SHA).
        /// </value>
        public string RepositoryRevision
        {
            get { return GetEnvironmentString("bamboo_planRepository_revision"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BambooCommitInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BambooCommitInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}
