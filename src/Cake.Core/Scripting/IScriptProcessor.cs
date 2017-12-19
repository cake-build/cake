// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Represents a script processor.
    /// </summary>
    public interface IScriptProcessor
    {
        /// <summary>
        /// Installs the addins.
        /// </summary>
        /// <param name="addins">The addins to install.</param>
        /// <param name="installPath">The install path.</param>
        /// <returns>A list containing file paths to installed addin assemblies.</returns>
        IReadOnlyList<FilePath> InstallAddins(IReadOnlyCollection<PackageReference> addins, DirectoryPath installPath);

        /// <summary>
        /// Installs the tools.
        /// </summary>
        /// <param name="tools">The tools to install.</param>
        /// <param name="installPath">The install path.</param>
        void InstallTools(IReadOnlyCollection<PackageReference> tools, DirectoryPath installPath);

        /// <summary>
        /// Installs the modules.
        /// </summary>
        /// <param name="modules">The modules to install.</param>
        /// <param name="installPath">The install path.</param>
        void InstallModules(IReadOnlyCollection<PackageReference> modules, DirectoryPath installPath);
    }
}