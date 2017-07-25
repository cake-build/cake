// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if NETCORE
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Packaging;
using NuGet.Frameworks;

namespace Cake.NuGet.V3
{
    internal sealed class NuGetV3ContentResolver : NuGetContentResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;

        public NuGetV3ContentResolver(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber) : base(fileSystem, environment, globber)
        {
            _fileSystem = fileSystem;
            _environment = environment;
        }

        protected override IReadOnlyCollection<IFile> GetAddinAssemblies(DirectoryPath path, PackageReference package)
        {
            if (!_fileSystem.Exist(path))
            {
                return new List<IFile>();
            }

            // Get current framework.
            var provider = DefaultFrameworkNameProvider.Instance;
            var current = NuGetFramework.Parse(_environment.Runtime.TargetFramework.FullName, provider);

            // Get all candidate files.
            var assemblies = GetFiles(path, package, new[] { path.FullPath + "/**/*.dll" })
                                .Where(file => !"Cake.Core.dll".Equals(file.Path.GetFilename().FullPath, StringComparison.OrdinalIgnoreCase)
                                                && IsCLRAssembly(file))
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

            // Return the result.
            return mapping[nearest].Select(p => _fileSystem.GetFile(p)).ToList();
        }

        private NuGetFramework ParseFromDirectoryPath(NuGetFramework current, DirectoryPath path)
        {
            var queue = new Queue<string>(path.Segments);
            while (queue.Count > 0)
            {
                var other = NuGetFramework.Parse(queue.Dequeue(), DefaultFrameworkNameProvider.Instance);
                var compatible = DefaultCompatibilityProvider.Instance.IsCompatible(other, current);
                if (compatible || queue.Count == 0)
                {
                    return other;
                }
            }
            throw new InvalidOperationException("Something went wrong when parsing framework.");
        }
    }
}
#endif