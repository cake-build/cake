// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.NuGet
{
    /// <summary>
    /// Finds assemblies included in a nuget package.
    /// </summary>
    public interface INuGetPackageAssembliesLocator
    {
        /// <summary>
        /// Finds assemblies (DLLs) included in a nuget package.
        /// </summary>
        /// <param name="packageDirectory">The package directory.</param>
        /// <returns>
        /// the DLLs.
        /// </returns>
        IReadOnlyList<IFile> FindAssemblies(DirectoryPath packageDirectory);
    }
}
