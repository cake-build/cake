// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Workload.List
{
    /// <summary>
    /// An item as returned by <see cref="DotNetWorkloadLister"/>.
    /// </summary>
    public sealed class DotNetWorkloadListItem
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetWorkloadListItem" /> class.
        /// </summary>
        /// <param name="id">The workload Id.</param>
        /// <param name="manifestVersion">The workload manifest version.</param>
        /// <param name="installationSource">The workload installation source.</param>
        public DotNetWorkloadListItem(string id, string manifestVersion, string installationSource)
        {
            Id = id;
            ManifestVersion = manifestVersion;
            InstallationSource = installationSource;
        }

        /// <summary>
        /// Gets the workload ID.
        /// </summary>
        public string Id { get; }

        /// <summary>
        /// Gets the manifest version of the workload as string.
        /// </summary>
        public string ManifestVersion { get; }

        /// <summary>
        /// Gets the installation source of the workload as string.
        /// </summary>
        public string InstallationSource { get; }
    }
}
