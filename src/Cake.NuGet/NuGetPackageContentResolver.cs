﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    /// <summary>
    /// Implementation of a file locator for NuGet packages that
    /// returns relevant files for the current framework given a resource type.
    /// </summary>
    public sealed class NuGetPackageContentResolver : INuGetPackageContentResolver
    {
        private readonly IFileSystem _fileSystem;
        private readonly INuGetPackageAssembliesLocator _locator;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPackageContentResolver"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="locator">The locator.</param>
        public NuGetPackageContentResolver(
            IFileSystem fileSystem,
            INuGetPackageAssembliesLocator locator)
        {
            _fileSystem = fileSystem;
            _locator = locator;
        }

        /// <summary>
        /// Gets the relevant files for a NuGet package
        /// given a path and a resource type.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="type">The resource type.</param>
        /// <returns>A collection of files.</returns>
        public IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, PackageType type)
        {
            if (type == PackageType.Addin)
            {
                return _locator.FindAssemblies(path).ToArray();
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

        /// <summary>
        /// Gets the relevant files for a NuGet package
        /// given a path and a resource type defined with <paramref name="fileExtension"/>.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="fileExtension">The file extension ex: .cake</param>
        /// <returns>A collection of files.</returns>
        public IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, string fileExtension)
        {
            if (fileExtension == null)
            {
                throw new ArgumentNullException("fileExtension");
            }
            if (!System.IO.Path.HasExtension(fileExtension))
            {
                const string format = "The string parameter 'fileExtension' value is '{0}' but what is expected is a file extension.";
                var message = string.Format(CultureInfo.InvariantCulture, format, fileExtension);
                throw new CakeException(message);
            }

            fileExtension = System.IO.Path.GetExtension(fileExtension);
            if (!fileExtension.StartsWith("*"))
            {
                fileExtension = "*" + fileExtension;
            }

            var result = new List<IFile>();
            var directory = _fileSystem.GetDirectory(path);
            if (directory.Exists)
            {
                var files = directory.GetFiles(fileExtension, SearchScope.Recursive);
                result.AddRange(files);
            }
            return result;
        }
    }
}
