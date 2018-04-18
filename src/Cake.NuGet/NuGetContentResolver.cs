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
using Cake.Core.Packaging;
using NuGet.Frameworks;

namespace Cake.NuGet
{
    internal sealed class NuGetContentResolver : INuGetContentResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly ICakeLog _log;

        public NuGetContentResolver(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber,
            ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
            _log = log;
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
                return new List<IFile>();
            }

            // Get current framework.
            var provider = DefaultFrameworkNameProvider.Instance;
            var current = NuGetFramework.Parse(_environment.Runtime.BuiltFramework.FullName, provider);

            // Get all ref assemblies.
            var refAssemblies = _globber.GetFiles(path.FullPath + "/ref/**/*.dll");

            // Get all candidate files.
            var pathComparer = PathComparer.Default;
            var assemblies = GetFiles(path, package, new[] { path.FullPath + "/**/*.dll" })
                .Where(file => !"Cake.Core.dll".Equals(file.Path.GetFilename().FullPath, StringComparison.OrdinalIgnoreCase)
                               && IsCLRAssembly(file)
                               && !refAssemblies.Contains(file.Path, pathComparer))
                .ToList();

            // Iterate all found files.
            var comparer = new NuGetFrameworkFullComparer();
            var mapping = new Dictionary<NuGetFramework, List<FilePath>>(comparer);
            foreach (var assembly in assemblies)
            {
                // Get relative path.
                var relative = path.GetRelativePath(assembly.Path);
                var framework = ParseFromDirectoryPath(current, relative.GetDirectory());
                if (!mapping.ContainsKey(framework))
                {
                    mapping.Add(framework, new List<FilePath>());
                }
                mapping[framework].Add(assembly.Path);
            }

            // Reduce found frameworks to the closest one.
            var reducer = new FrameworkReducer();
            var nearest = reducer.GetNearest(current, mapping.Keys);
            if (nearest == null || !mapping.ContainsKey(nearest))
            {
                return new List<IFile>();
            }

            if (nearest == NuGetFramework.AnyFramework)
            {
                var framework = _environment.Runtime.BuiltFramework;
                _log.Warning("Could not find any assemblies compatible with {0} in NuGet package {1}. " +
                             "Falling back to using root folder of NuGet package.", framework.FullName, package.Package);
            }

            // Return the result.
            return mapping[nearest].Select(p => _fileSystem.GetFile(p)).ToList();
        }

        private NuGetFramework ParseFromDirectoryPath(NuGetFramework current, DirectoryPath path)
        {
            var segments = path.Segments;

            if (segments.Length == 1 &&
                segments[0].Equals("lib", StringComparison.OrdinalIgnoreCase))
            {
                // Treat as AnyFramework if lib folder
                return NuGetFramework.AnyFramework;
            }

            var queue = new Queue<string>(segments);
            while (queue.Count > 0)
            {
                var other = NuGetFramework.Parse(queue.Dequeue(), DefaultFrameworkNameProvider.Instance);
                var compatible = DefaultCompatibilityProvider.Instance.IsCompatible(other, current);
                if (compatible || queue.Count == 0)
                {
                    return other;
                }
            }

            // Treat as AnyFramework if root folder
            return NuGetFramework.AnyFramework;
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

        private bool IsCLRAssembly(IFile file)
        {
            if (!file.Exists || file.Length < 365)
            {
                return false;
            }

            using (var fs = file.OpenRead())
            {
                using (var reader = new System.IO.BinaryReader(fs))
                {
                    const uint MagicOffset = 0x18;
                    const uint Magic32Bit = 0x10b;
                    const int Offset32Bit = 0x5e;
                    const int Offset64Bit = 0x6e;
                    const int OffsetDictionary = 0x70;

                    // PE Header Start
                    fs.Position = 0x3C;

                    // Go to Magic header
                    fs.Position = reader.ReadUInt32() + MagicOffset;

                    // Check magic to get 32 / 64 bit offset
                    var is32Bit = reader.ReadUInt16() == Magic32Bit;
                    var offset = fs.Position + (is32Bit ? Offset32Bit : Offset64Bit) + OffsetDictionary;

                    if (offset + 4 > fs.Length)
                    {
                        return false;
                    }

                    // Go to dictionary start
                    fs.Position = offset;
                    return reader.ReadUInt32() > 0;
                }
            }
        }
    }
}
