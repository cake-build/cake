// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using NuGet.Client;
using NuGet.ContentModel;
using NuGet.Frameworks;
using NuGet.RuntimeModel;

namespace Cake.NuGet
{
    internal sealed class NuGetContentResolver : INuGetContentResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;

        private static readonly Lazy<RuntimeGraph> RuntimeGraph = new Lazy<RuntimeGraph>(() =>
        {
            var assembly = typeof(NuGetContentResolver).Assembly;
            using (var stream = assembly.GetManifestResourceStream($"{assembly.GetName().Name}.runtime.json"))
            {
                return JsonRuntimeFormat.ReadRuntimeGraph(stream);
            }
        });

        public NuGetContentResolver(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber,
            ICakeLog log)
        {
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _globber = globber ?? throw new ArgumentNullException(nameof(globber));
            _log = log ?? throw new ArgumentNullException(nameof(log));
        }

        public IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, PackageReference package, PackageType type)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (type == PackageType.Addin || type == PackageType.Module)
            {
                return GetAddinAssemblies(path, package);
            }
            if (type == PackageType.Tool)
            {
                return GetToolFiles(path, package);
            }

            throw new InvalidOperationException("Unknown resource type.");
        }

        private IReadOnlyCollection<IFile> GetAddinAssemblies(DirectoryPath path, PackageReference package)
        {
            if (!_fileSystem.Exist(path))
            {
                _log.Debug("Path not found at {0}.", path);
                return Array.Empty<IFile>();
            }

            // Get current framework.
            var tfm = NuGetFramework.Parse(_environment.Runtime.BuiltFramework.FullName, DefaultFrameworkNameProvider.Instance);

            // Get current runtime identifier.
            string rid = _environment.Runtime.IsCoreClr ? Microsoft.DotNet.PlatformAbstractions.RuntimeEnvironment.GetRuntimeIdentifier() : null;

            // Get all candidate files.
            var pathComparer = PathComparer.Default;
            var assemblies = GetFiles(path, package, new[] { path.FullPath + "/**/*.{dll,so,dylib}" })
                .Where(file => !"Cake.Core.dll".Equals(file.Path.GetFilename().FullPath, StringComparison.OrdinalIgnoreCase))
                .ToDictionary(x => path.GetRelativePath(x.Path).FullPath);
            if (assemblies.Count == 0)
            {
                _log.Debug("Assemblies not found at {0}.", path);
            }

            var conventions = new ManagedCodeConventions(RuntimeGraph.Value);
            var collection = new ContentItemCollection();
            collection.Load(assemblies.Keys);
            var criteria = conventions.Criteria.ForFrameworkAndRuntime(tfm, rid);

            var managedAssemblies = collection.FindBestItemGroup(criteria, conventions.Patterns.RuntimeAssemblies);
            var files = managedAssemblies?.Items.Select(x => assemblies[x.Path]).Where(x => x.IsClrAssembly()).ToArray() ?? Array.Empty<IFile>();
            if (_environment.Runtime.IsCoreClr)
            {
                var nativeAssemblies = collection.FindBestItemGroup(criteria, conventions.Patterns.NativeLibraries);
                files = (nativeAssemblies?.Items.Select(x => assemblies[x.Path]) ?? Array.Empty<IFile>()).Concat(files).ToArray();
            }
            if (files.Length == 0)
            {
                _log.Debug("Assemblies not found for tfm {0} and rid {1}.", tfm, rid);
            }

            return files;
        }

        private IReadOnlyCollection<IFile> GetToolFiles(DirectoryPath path, PackageReference package)
        {
            var result = new List<IFile>();
            var toolDirectory = _fileSystem.GetDirectory(path);
            if (toolDirectory.Exists)
            {
                result.AddRange(GetFiles(path, package));
            }
            return result;
        }

        private IEnumerable<IFile> GetFiles(DirectoryPath path, PackageReference package, string[] patterns = null)
        {
            var collection = new FilePathCollection(new PathComparer(_environment));

            // Get default files (exe and dll).
            patterns = patterns ?? new[] { path.FullPath + "/**/*.exe", path.FullPath + "/**/*.dll" };
            foreach (var pattern in patterns)
            {
                collection.Add(_globber.GetFiles(pattern));
            }

            // Include files.
            if (package.Parameters.ContainsKey("include"))
            {
                foreach (var include in package.Parameters["include"])
                {
                    var includePath = string.Concat(path.FullPath, "/", include.TrimStart('/'));
                    collection.Add(_globber.GetFiles(includePath));
                }
            }

            // Exclude files.
            if (package.Parameters.ContainsKey("exclude"))
            {
                foreach (var exclude in package.Parameters["exclude"])
                {
                    var excludePath = string.Concat(path.FullPath, "/", exclude.TrimStart('/'));
                    collection.Remove(_globber.GetFiles(excludePath));
                }
            }

            // Return the files.
            return collection.Select(p => _fileSystem.GetFile(p)).ToArray();
        }
    }
}
