// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.NuGet
{
    /// <summary>
    /// Bundles packageAssemblyFiles into package reference sets grouped by their target framework.
    /// </summary>
    public interface INuGetPackageReferenceBundler
    {
        /// <summary>
        /// Bundles the provided packageAssemblyFiles into package reference sets grouped by their target framework.
        /// </summary>
        /// <param name="packageAssemblyFiles">The package assembly files.</param>
        /// <returns>The newly created PackageReferenceSets</returns>
        IReadOnlyCollection<NuGetPackageReferenceSet> BundleByTargetFramework(IEnumerable<FilePath> packageAssemblyFiles);
    }
}
