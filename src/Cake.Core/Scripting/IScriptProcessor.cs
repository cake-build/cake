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
        /// <param name="installationRoot">The installation root path.</param>
        /// <returns>A list containing file paths to installed addin assemblies.</returns>
        IReadOnlyList<FilePath> InstallAddins(ScriptAnalyzerResult analyzerResult, DirectoryPath installationRoot);

        /// <summary>
        /// Installs the tools specified in the build scripts.
        /// </summary>
        /// <param name="analyzerResult">The analyzer result.</param>
        /// <param name="installationRoot">The installation root path.</param>
        void InstallTools(ScriptAnalyzerResult analyzerResult, DirectoryPath installationRoot);
    }
}