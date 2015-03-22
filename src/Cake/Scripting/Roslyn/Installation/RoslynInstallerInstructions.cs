using System;
using System.Collections.Generic;
using Cake.Core.IO;
using NuGet;

namespace Cake.Scripting.Roslyn.Installation
{
    /// <summary>
    /// Represents instructions for installing a NuGet package
    /// and copy specific files to the bin directory.
    /// </summary>
    public sealed class RoslynInstallerInstructions
    {
        private readonly List<FilePath> _paths;
        private readonly Dictionary<string, SemanticVersion> _packages;
        private readonly List<PackageSource> _packageSources;

        /// <summary>
        /// Gets the paths to the files that should be copied.
        /// </summary>
        /// <value>The paths.</value>
        public IReadOnlyList<FilePath> Paths
        {
            get { return _paths; }
        }

        /// <summary>
        /// Gets the packages to be installed.
        /// </summary>
        /// <value>The packages.</value>
        public IReadOnlyDictionary<string, SemanticVersion> Packages
        {
            get { return _packages; }
        }

        /// <summary>
        /// Gets the package sources.
        /// </summary>
        /// <value>The package sources.</value>
        public IReadOnlyList<PackageSource> PackageSources
        {
            get { return _packageSources; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="RoslynInstallerInstructions"/> class.
        /// </summary>
        /// <param name="paths">The paths.</param>
        /// <param name="packages">The packages.</param>
        /// <param name="packageSources">The package sources.</param>
        public RoslynInstallerInstructions(
            IEnumerable<FilePath> paths,
            IDictionary<string, SemanticVersion> packages,
            IEnumerable<PackageSource> packageSources)
        {
            _paths = new List<FilePath>(paths);
            _packages = new Dictionary<string, SemanticVersion>(packages);
            _packageSources = new List<PackageSource>(packageSources);
        }
    }
}
