// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Collections.ObjectModel;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.NuGet
{
    /// <summary>
    /// Finds assemblies (DLLs) included in a nuget package.
    /// </summary>
    public sealed class NuGetPackageAssembliesLocator : INuGetPackageAssembliesLocator
    {
        private readonly INuGetAssemblyCompatibilityFilter _assemblyCompatibilityFilter;
        private readonly ICakeEnvironment _environment;
        private readonly IFileSystem _fileSystem;
        private readonly ICakeLog _log;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageAssembliesLocator"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="log">The log.</param>
        /// <param name="assemblyCompatibilityFilter">The assembly compatibility filter.</param>
        /// <param name="environment">The environment.</param>
        public NuGetPackageAssembliesLocator(IFileSystem fileSystem, ICakeLog log,
            INuGetAssemblyCompatibilityFilter assemblyCompatibilityFilter, ICakeEnvironment environment)
        {
            _fileSystem = fileSystem;
            _log = log;
            _assemblyCompatibilityFilter = assemblyCompatibilityFilter;
            _environment = environment;
        }

        /// <summary>
        /// Finds assemblies (DLLs) included in a nuget package.
        /// </summary>
        /// <param name="packageDirectory">The package directory.</param>
        /// <returns>
        /// the DLLs.
        /// </returns>
        public IReadOnlyList<IFile> FindAssemblies(DirectoryPath packageDirectory)
        {
            if (packageDirectory == null)
            {
                throw new ArgumentNullException("packageDirectory");
            }
            if (packageDirectory.IsRelative)
            {
                throw new CakeException("Package directory (" + packageDirectory.FullPath +
                                        ") must be an absolute path.");
            }

            if (!_fileSystem.Exist(packageDirectory))
            {
                return new List<IFile>().AsReadOnly();
            }

            var packageAssemblies = GetAllPackageAssemblies(packageDirectory);

            if (!packageAssemblies.Any())
            {
                _log.Warning("Unable to locate any assemblies under {0}", packageDirectory.FullPath);
            }

            var compatibleAssemblyPaths = FilterCompatibleAssemblies(packageAssemblies, packageDirectory);

            var resolvedAssemblyFiles = ResolveAssemblyFiles(compatibleAssemblyPaths);

            return resolvedAssemblyFiles;
        }

        private ReadOnlyCollection<IFile> ResolveAssemblyFiles(IEnumerable<FilePath> compatibleAssemblyPaths)
        {
            return compatibleAssemblyPaths.Select(_fileSystem.GetFile).ToList().AsReadOnly();
        }

        private FilePath[] GetAllPackageAssemblies(DirectoryPath packageDirectory)
        {
            return _fileSystem.GetDirectory(packageDirectory).GetFiles("*.dll", SearchScope.Recursive)
                .Where(
                    file =>
                        !"Cake.Core.dll".Equals(file.Path.GetFilename().FullPath, StringComparison.OrdinalIgnoreCase))
                .Select(a => a.Path)
                .ToArray();
        }

        private IEnumerable<FilePath> FilterCompatibleAssemblies(
            IEnumerable<FilePath> assemblies, DirectoryPath packageDirectory)
        {
            var targetFramework = _environment.GetTargetFramework();

            // standardize assembly paths as relative to package directory
            var standardizedAssemblyPaths =
                assemblies.Select(a => a.IsRelative ? a : a.FullPath.Substring(packageDirectory.FullPath.Length + 1))
                    .ToArray();

            var compatibleAssemblyPaths =
                _assemblyCompatibilityFilter.FilterCompatibleAssemblies(targetFramework, standardizedAssemblyPaths);

            // return as absolute paths
            return compatibleAssemblyPaths
                .Select(cp => cp.MakeAbsolute(packageDirectory));
        }
    }
}
