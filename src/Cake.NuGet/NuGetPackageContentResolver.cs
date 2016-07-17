﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
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
            if (type == PackageType.NuScript)
            {
                var result = new List<IFile>();
                var toolDirectory = _fileSystem.GetDirectory(path);
                if (toolDirectory.Exists)
                {
                    var loadOrder = new List<string>();

                    // Check if there is a load.text file
                    var loadFile = toolDirectory.GetFiles("load.txt", SearchScope.Recursive).FirstOrDefault();
                    if (loadFile != null)
                    {
                        using (var loadStream = loadFile.Open(FileMode.Open, FileAccess.Read))
                        using (var reader = new StreamReader(loadStream))
                        {
                            string line;
                            while ((line = reader.ReadLine()) != null)
                            {
                                loadOrder.Add(line);
                            }
                        }
                    }
                    if (loadOrder.Any())
                    {
                        // We do a revers as the first item in the list represents the last item in the scripts result.
                        foreach (var name in loadOrder)
                        {
                            var file = toolDirectory.GetFiles(name, SearchScope.Recursive).First();
                            result.Add(file);
                        }
                    }
                    else
                    {
                        // add cake Files
                        var files = toolDirectory.GetFiles("*.cake", SearchScope.Recursive);
                        result.AddRange(files);
                    }
                }
                return result;
            }
            throw new InvalidOperationException("Unknown resource type.");
        }
    }
}
