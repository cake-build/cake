using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Defines a script runner extension for a <see cref="IProcessorExtension{TValueType}"/>
    /// </summary>
    public interface IScriptRunnerExtension<in TValueType>
    {
        /// <summary>
        /// Defines installation instructions.
        /// </summary>
        /// <param name="values">The <see cref="IProcessorExtension{TValueType}"/> values.</param>
        /// <param name="result">The current executing <see cref="ScriptAnalyzerResult"/>.</param>
        /// <param name="scriptAnalyzerContext">The executing <see cref="IScriptAnalyzerContext"/>.</param>
        /// <param name="toolsPath">Installation path to tools.</param>
        void Install(IEnumerable<TValueType> values, ref ScriptAnalyzerResult result, IScriptAnalyzerContext scriptAnalyzerContext, DirectoryPath toolsPath);
    }
}