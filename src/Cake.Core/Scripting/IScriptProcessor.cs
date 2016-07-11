// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
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
    }
}
