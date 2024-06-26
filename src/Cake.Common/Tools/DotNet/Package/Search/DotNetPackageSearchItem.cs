// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNet.Package.Search
{
    /// <summary>
    /// An item as returned by <see cref="DotNetAliases.DotNetSearchPackage(Core.ICakeContext, string, DotNetPackageSearchSettings)"/>.
    /// </summary>
    public class DotNetPackageSearchItem
    {
        /// <summary>
        /// Gets or sets the name of the NuGetListItem.
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the version of the NuGetListItem as string.
        /// </summary>
        public string Version { get; set; }
    }
}
