using System.Collections.Generic;

namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Bundles packageAssemblyFiles into package reference sets grouped by their target framework.
    /// </summary>
    public interface IPackageReferenceBundler
    {
        /// <summary>
        /// Bundles the provided packageAssemblyFiles into package reference sets grouped by their target framework.
        /// </summary>
        /// <param name="packageAssemblyFiles">The package assembly files.</param>
        /// <returns>The newly created PackageReferenceSets</returns>
        IReadOnlyCollection<PackageReferenceSet> BundleByTargetFramework(IEnumerable<FilePath> packageAssemblyFiles);
    }
}