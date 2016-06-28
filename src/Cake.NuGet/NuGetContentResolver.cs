// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    internal abstract class NuGetContentResolver : INuGetContentResolver
    {
        private readonly IFileSystem _fileSystem;

        protected NuGetContentResolver(IFileSystem fileSystem)
        {
            _fileSystem = fileSystem;
        }

        public IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, PackageType type)
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
                var result = new List<IFile>();
                var toolDirectory = _fileSystem.GetDirectory(path);
                if (toolDirectory.Exists)
                {
                    var files = toolDirectory.GetFiles("*.exe", SearchScope.Recursive);
                    result.AddRange(files);
                }
                return result;
            }

            throw new InvalidOperationException("Unknown resource type.");
        }

        protected abstract IReadOnlyCollection<IFile> GetAddinAssemblies(DirectoryPath path);
    }
}
