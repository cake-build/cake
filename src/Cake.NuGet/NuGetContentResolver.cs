// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    internal abstract class NuGetContentResolver : INuGetContentResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;

        protected NuGetContentResolver(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IGlobber globber)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
        }

        public IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, PackageReference package, PackageType type)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (type == PackageType.Addin)
            {
                return GetAddinAssemblies(path);
            }
            if (type == PackageType.Tool)
            {
                return GetToolFiles(path, package);
            }

            throw new InvalidOperationException("Unknown resource type.");
        }

        protected abstract IReadOnlyCollection<IFile> GetAddinAssemblies(DirectoryPath path);

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

        private IEnumerable<IFile> GetFiles(DirectoryPath path, PackageReference package)
        {
            var collection = new FilePathCollection(new PathComparer(_environment));

            // Get default files (exe and dll).
            var patterns = new[] { path.FullPath + "/**/*.exe", path.FullPath + "/**/*.dll" };
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
