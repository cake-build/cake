// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.Core.Packaging
{
    /// <summary>
    /// Represents an installer for a specific package.
    /// </summary>
    public interface IPackageInstaller
    {
        /// <summary>
        /// Determines whether this instance can install the specified resource.
        /// </summary>
        /// <param name="package">The package resource.</param>
        /// <param name="type">The package type.</param>
        /// <returns>
        ///   <c>true</c> if this installer can install the
        ///   specified resource; otherwise <c>false</c>.
        /// </returns>
        bool CanInstall(PackageReference package, PackageType type);

        /// <summary>
        /// Installs the specified resource at the given location.
        /// </summary>
        /// <param name="package">The package resource.</param>
        /// <param name="type">The package type.</param>
        /// <param name="path">The location where to install the resource.</param>
        /// <returns>The installed files.</returns>
        IReadOnlyCollection<IFile> Install(PackageReference package, PackageType type, DirectoryPath path);
    }
}
