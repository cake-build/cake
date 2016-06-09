// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.NuGet
{
    /// <summary>
    /// Represents a file locator for NuGet packages that returns relevant
    /// files for the current framework given the resource type.
    /// </summary>
    public interface INuGetPackageContentResolver
    {
        /// <summary>
        /// Gets the relevant files for a NuGet package
        /// given a path and a resource type.
        /// </summary>
        /// <param name="path">The path to search.</param>
        /// <param name="type">The resource type.</param>
        /// <returns>A collection of files.</returns>
        IReadOnlyCollection<IFile> GetFiles(DirectoryPath path, PackageType type);
    }
}
