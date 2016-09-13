// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core;

namespace Cake.Common.Build.GoCD.Data
{
    /// <summary>
    /// Provides Go.CD repository information for a current build.
    /// </summary>
    public sealed class GoCDRepositoryInfo : GoCDInfo
    {
        /// <summary>
        /// Gets the source control revision.
        /// </summary>
        /// <value>
        /// The the source control revision.
        /// </value>
        public string Revision => GetEnvironmentString("GO_REVISION");

        /// <summary>
        /// Gets the last revision the build was triggered by if there were multiple revisions.
        /// </summary>
        /// <value>
        /// The last revision the build was triggered by if there were multiple revisions.
        /// </value>
        public string ToRevision => GetEnvironmentString("GO_TO_REVISION");

        /// <summary>
        /// Gets the first revision the build was triggered by if there were multiple revisions.
        /// </summary>
        /// <value>
        /// The first revision the build was triggered by if there were multiple revisions.
        /// </value>
        public string FromRevision => GetEnvironmentString("GO_FROM_REVISION");

        /// <summary>
        /// Initializes a new instance of the <see cref="GoCDRepositoryInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GoCDRepositoryInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}