using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Defines a script runner extension for a <see cref="IProcessorExtension"/>
    /// </summary>
    public interface IScriptRunnerExtension
    {
        /// <summary>
        /// Defines installation instructions.
        /// </summary>
        /// <param name="values">The <see cref="IProcessorExtension"/> values.</param>
        /// <param name="result">The current executing <see cref="ScriptAnalyzerResult"/>.</param>
        /// <param name="scriptAnalyzerContext">The executing <see cref="IScriptAnalyzerContext"/>.</param>
        /// <param name="toolsPath">Installation path to tools.</param>
        void Install(IEnumerable<object> values, ref ScriptAnalyzerResult result, IScriptAnalyzerContext scriptAnalyzerContext, DirectoryPath toolsPath);
    }
}