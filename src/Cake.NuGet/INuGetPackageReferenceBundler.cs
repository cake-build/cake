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