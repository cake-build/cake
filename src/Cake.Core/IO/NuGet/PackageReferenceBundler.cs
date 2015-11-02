using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO.NuGet.Parsing;

namespace Cake.Core.IO.NuGet
{
    /// <summary>
    /// Bundles packageAssemblyFiles into package reference sets grouped by their target framework.
    /// </summary>
    public sealed class PackageReferenceBundler : IPackageReferenceBundler
    {
        private readonly IAssemblyFilePathFrameworkNameParser _frameworkNameParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="PackageReferenceBundler"/> class.
        /// </summary>
        /// <param name="frameworkNameParser">The framework name parser.</param>
        public PackageReferenceBundler(IAssemblyFilePathFrameworkNameParser frameworkNameParser)
        {
            _frameworkNameParser = frameworkNameParser;
        }

        /// <summary>
        /// Bundles the provided packageAssemblyFiles into package reference sets grouped by their target framework.
        /// </summary>
        /// <param name="packageAssemblyFiles">The package assembly files.</param>
        /// <returns>The newly created PackageReferenceSets</returns>
        public IReadOnlyCollection<PackageReferenceSet> BundleByTargetFramework(IEnumerable<FilePath> packageAssemblyFiles)
        {
            if (packageAssemblyFiles == null)
            {
                throw new ArgumentNullException("packageAssemblyFiles");
            }
            
            // create PackageReferenceSets from the given assemblies
            var pathFrameworks = packageAssemblyFiles
                .Select(
                    d => new { FilePath = d, FrameworkName = _frameworkNameParser.Parse(d) }).ToArray();
           var referenceSets = pathFrameworks
                .GroupBy(d => d.FrameworkName)
                .Select(grp => new PackageReferenceSet(grp.Key, grp.Select(d => d.FilePath))).ToList();

            return referenceSets.AsReadOnly();
        }
    }
}