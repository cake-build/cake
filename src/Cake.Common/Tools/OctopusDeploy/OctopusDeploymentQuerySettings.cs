// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.OctopusDeploy
{
    /// <summary>
    /// Contains settings used by <see cref="OctopusDeployDeploymentQuerier.QueryOctopusDeployments"/>.
    /// </summary>
    public sealed class OctopusDeploymentQuerySettings : OctopusDeployCommonToolSettings
    {
        /// <summary>
        /// Gets or Sets a value that is an Octopus Environment Name to filter for.
        /// </summary>
        public string EnvironmentName { get; set; }

        /// <summary>
        /// Gets or Sets a value that is an Octopus Project Name to filter for.
        /// </summary>
        public string ProjectName { get; set; }

        /// <summary>
        /// Gets or Sets a value that is an Octopus Tenant Name to filter for.
        /// </summary>
        public string TenantName { get; set; }

        /// <summary>
        /// Gets or Sets a value that indicates how many deployments to retrieve
        /// in Date Descending order (most recent first)
        /// Default: 1.
        /// </summary>
        public int Count { get; set; } = 1;
    }
}
