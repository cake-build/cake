// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core.IO;

namespace Cake.NuGet
{
    /// <summary>
    /// Bundles packageAssemblyFiles into package reference sets grouped by their target framework.
    /// </summary>
    public sealed class NuGetPackageReferenceBundler : INuGetPackageReferenceBundler
    {
        private readonly IAssemblyFrameworkNameParser _frameworkNameParser;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageReferenceBundler"/> class.
        /// </summary>
        /// <param name="frameworkNameParser">The framework name parser.</param>
        public NuGetPackageReferenceBundler(IAssemblyFrameworkNameParser frameworkNameParser)
        {
            _frameworkNameParser = frameworkNameParser;
        }

        /// <summary>
        /// Bundles the provided packageAssemblyFiles into package reference sets grouped by their target framework.
        /// </summary>
        /// <param name="packageAssemblyFiles">The package assembly files.</param>
        /// <returns>The newly created PackageReferenceSets</returns>
        public IReadOnlyCollection<NuGetPackageReferenceSet> BundleByTargetFramework(IEnumerable<FilePath> packageAssemblyFiles)
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
                .Select(grp => new NuGetPackageReferenceSet(grp.Key, grp.Select(d => d.FilePath))).ToList();

            return referenceSets.AsReadOnly();
        }
    }
}
