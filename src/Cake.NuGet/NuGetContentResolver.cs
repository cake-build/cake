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
                return GetAddinAssemblies(path, package);
            }
            if (type == PackageType.Tool)
            {
                return GetToolFiles(path, package);
            }

            throw new InvalidOperationException("Unknown resource type.");
        }

        protected abstract IReadOnlyCollection<IFile> GetAddinAssemblies(DirectoryPath path, PackageReference package);

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

        protected IEnumerable<IFile> GetFiles(DirectoryPath path, PackageReference package, string[] patterns = null)
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

        protected bool IsCLRAssembly(IFile file)
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
