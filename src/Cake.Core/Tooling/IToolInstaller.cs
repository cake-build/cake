// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;

namespace Cake.Core.Tooling
{
    /// <summary>
    /// Represents a tool installer.
    /// </summary>
    public interface IToolInstaller
    {
        /// <summary>
        /// Installs a tool using the specified package reference.
        /// </summary>
        /// <param name="tool">The package reference for the tool to install.</param>
        /// <returns>An enumerable of file paths where the tool was installed.</returns>
        IEnumerable<FilePath> Install(PackageReference tool);
    }
}
