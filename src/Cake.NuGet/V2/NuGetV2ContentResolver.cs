// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCORE
using System;
using System.Collections.Generic;
using System.Linq;
using System.Runtime.Versioning;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.NuGet.V2
{
    /// <summary>
    /// Implementation of a file locator for NuGet packages that
    /// returns relevant files for the current framework given a resource type.
    /// </summary>
    internal sealed class NuGetV2ContentResolver : NuGetContentResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        public NuGetV2ContentResolver(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber,
            ICakeLog log) : base(fileSystem, environment, globber)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
        }

        protected override IReadOnlyCollection<IFile> GetAddinAssemblies(DirectoryPath packageDirectory, PackageReference package)
        {
            if (packageDirectory == null)
            {
                throw new ArgumentNullException("packageDirectory");
            }
            if (packageDirectory.IsRelative)
            {
                throw new CakeException("Package directory (" + packageDirectory.FullPath + ") must be an absolute path.");
            }
            if (!_fileSystem.Exist(packageDirectory))
            {
                return new List<IFile>();
            }

            var packageAssemblies = GetAllPackageAssemblies(packageDirectory, package);
            if (!packageAssemblies.Any())
            {
                _log.Warning("Unable to locate any assemblies under {0}", packageDirectory.FullPath);
            }

            var compatibleAssemblyPaths = FilterCompatibleAssemblies(packageAssemblies, packageDirectory);
            var resolvedAssemblyFiles = ResolveAssemblyFiles(compatibleAssemblyPaths);

            return resolvedAssemblyFiles;
        }

        private IReadOnlyCollection<IFile> ResolveAssemblyFiles(IEnumerable<FilePath> compatibleAssemblyPaths)
        {
            return compatibleAssemblyPaths.Select(_fileSystem.GetFile).ToList().AsReadOnly();
        }

        private FilePath[] GetAllPackageAssemblies(DirectoryPath packageDirectory, PackageReference package)
        {
            return GetFiles(packageDirectory, package, new[] { packageDirectory.FullPath + "/**/*.dll" })
                .Where(file => !"Cake.Core.dll".Equals(file.Path.GetFilename().FullPath, StringComparison.OrdinalIgnoreCase)
                            && IsCLRAssembly(file))
                .Select(a => a.Path)
                .ToArray();
        }

        private IEnumerable<FilePath> FilterCompatibleAssemblies(
            IEnumerable<FilePath> assemblies, DirectoryPath packageDirectory)
        {
            var targetFramework = _environment.Runtime.TargetFramework;

            // standardize assembly paths as relative to package directory
            var standardizedAssemblyPaths =
                assemblies.Select(a => a.IsRelative ? a : a.FullPath.Substring(packageDirectory.FullPath.Length + 1))
                    .ToArray();

            var compatibleAssemblyPaths = FilterCompatibleAssemblies(targetFramework, standardizedAssemblyPaths);

            // return as absolute paths
            return compatibleAssemblyPaths
                .Select(cp => cp.MakeAbsolute(packageDirectory));
        }

        private IEnumerable<FilePath> FilterCompatibleAssemblies(FrameworkName targetFramework, IEnumerable<FilePath> assemblyPaths)
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

            var referenceSets = BundleByTargetFramework(assemblyPaths);

            return GetCompatibleItems(targetFramework, referenceSets).SelectMany(r => r.References);
        }

        private IEnumerable<NuGetPackageReferenceSet> GetCompatibleItems(FrameworkName projectFramework, IEnumerable<NuGetPackageReferenceSet> items)
        {
            if (projectFramework == null)
            {
                throw new ArgumentNullException("projectFramework");
            }
            if (items == null)
            {
                throw new ArgumentNullException("items");
            }

            IEnumerable<NuGetPackageReferenceSet> compatibleNugetItems;
            if (global::NuGet.VersionUtility.TryGetCompatibleItems(projectFramework, items, out compatibleNugetItems))
            {
                return compatibleNugetItems;
            }

            return Enumerable.Empty<NuGetPackageReferenceSet>();
        }

        private IEnumerable<NuGetPackageReferenceSet> BundleByTargetFramework(IEnumerable<FilePath> packageAssemblyFiles)
        {
            if (packageAssemblyFiles == null)
            {
                throw new ArgumentNullException("packageAssemblyFiles");
            }

            var items = packageAssemblyFiles.Select(d => new
            {
                FilePath = d,
                FrameworkName = ParseFramework(d)
            });

            return items
                 .GroupBy(item => item.FrameworkName)
                 .Select(group => new NuGetPackageReferenceSet(group.Key, group.Select(item => item.FilePath)));
        }

        private FrameworkName ParseFramework(FilePath path)
        {
            return ParseFramework(path.GetDirectory());
        }

        private FrameworkName ParseFramework(DirectoryPath path)
        {
            var queue = new Queue<string>(path.Segments);
            while (queue.Count > 0)
            {
                var current = queue.Dequeue();
                var parsedFxName = global::NuGet.VersionUtility.ParseFrameworkName(current);
                if (parsedFxName != global::NuGet.VersionUtility.UnsupportedFrameworkName || queue.Count == 0)
                {
                    return parsedFxName;
                }
            }
            return null;
        }
    }
}
#endif