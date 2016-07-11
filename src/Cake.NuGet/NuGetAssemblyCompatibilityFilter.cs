// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.NuGet
{
    /// <summary>
    /// Filters assemblies for .Net target framework compatibility
    /// </summary>
    public sealed class NuGetAssemblyCompatibilityFilter : INuGetAssemblyCompatibilityFilter
    {
        private readonly INuGetFrameworkCompatibilityFilter _frameworkCompatibilityFilter;
        private readonly INuGetPackageReferenceBundler _referenceBundler;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetAssemblyCompatibilityFilter" /> class.
        /// </summary>
        /// <param name="frameworkCompatibilityFilter">The framework compatibility filter.</param>
        /// <param name="referenceBundler">The reference set factory.</param>
        public NuGetAssemblyCompatibilityFilter(INuGetFrameworkCompatibilityFilter frameworkCompatibilityFilter,
            INuGetPackageReferenceBundler referenceBundler)
        {
            _frameworkCompatibilityFilter = frameworkCompatibilityFilter;
            _referenceBundler = referenceBundler;
        }

        /// <summary>
        /// Filters the assemblies for .Net target framework compatibility .
        /// </summary>
        /// <param name="targetFramework">The target framework.</param>
        /// <param name="assemblyPaths">The assembly files.</param>
        /// <returns>a subset of the provided assemblyFiles that match the provided targetFramework.</returns>
        public IEnumerable<FilePath> FilterCompatibleAssemblies(FrameworkName targetFramework,
            IEnumerable<FilePath> assemblyPaths)
        {
            if (targetFramework == null)
            {
                throw new ArgumentNullException("targetFramework");
            }
            if (assemblyPaths == null)
            {
                throw new ArgumentNullException("assemblyPaths");
            }

            assemblyPaths = assemblyPaths.ToArray();

            if (assemblyPaths.Any(a => !a.IsRelative))
            {
                throw new CakeException("All assemblyPaths must be relative to the package directory.");
            }

            var referenceSets = _referenceBundler.BundleByTargetFramework(assemblyPaths);

            return _frameworkCompatibilityFilter.GetCompatibleItems(targetFramework, referenceSets)
                    .SelectMany(r => r.References);
        }
    }
}
