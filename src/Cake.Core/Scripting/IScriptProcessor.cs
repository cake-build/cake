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
        /// <param name="analyzerResult">The analyzer result.</param>
        /// <param name="installPath">The install path.</param>
        /// <returns>A list containing file paths to installed addin assemblies.</returns>
        IReadOnlyList<FilePath> InstallAddins(ScriptAnalyzerResult analyzerResult, DirectoryPath installPath);

        /// <summary>
        /// Installs the tools specified in the build scripts.
        /// </summary>
        /// <param name="analyzerResult">The analyzer result.</param>
        /// <param name="installPath">The install path.</param>
        void InstallTools(ScriptAnalyzerResult analyzerResult, DirectoryPath installPath);

        /// <summary>
        /// Installs the tools specified in the build scripts.
        /// </summary>
        /// <param name="scripts">Nuget script package references to install</param>
        /// <param name="installPath">The install path.</param>
        /// <returns>a list of <see cref="FilePath"/> *.cake files</returns>
        IEnumerable<KeyValuePair<PackageReference, FilePath>> InstallNugetScripts(IEnumerable<PackageReference> scripts, DirectoryPath installPath);

        /// <summary>
        /// Install the <paramref name="package"/> for <paramref name="type"/> in <paramref name="installPath"/>.
        /// </summary>
        /// <param name="package">The <see cref="PackageReference"/> to install</param>
        /// <param name="type">The <see cref="PackageType"/></param>
        /// <param name="installPath">The location to install this package</param>
        /// <returns>Returns <see cref="IEnumerable{IFile}"/> containing the installed files.</returns>
        IEnumerable<IFile> InstallPackage(PackageReference package, PackageType type, DirectoryPath installPath);
    }
}
