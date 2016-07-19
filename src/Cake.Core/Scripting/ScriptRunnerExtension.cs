using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Scripting.Processors;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Defines a script runner extension for a <see cref="IProcessorExtension{TValueType}"/>
    /// </summary>
    public abstract class ScriptRunnerExtension<TValueType> : IScriptRunnerExtension<TValueType>
    {
        /// <summary>
        /// Get the <see cref="ICakeEnvironment"/>.
        /// </summary>
        public ICakeEnvironment Environment { get; private set; }

        /// <summary>
        /// Get the <see cref="ICakeLog"/>
        /// </summary>
        public ICakeLog Log { get; private set; }

        /// <summary>
        /// The <see cref="IProcessorExtension{TValueType}"/> used by this <see cref="IScriptRunnerExtension{TValueType}"/>.
        /// </summary>
        public IProcessorExtension<TValueType> ProcessorExtension { get; private set; }

        /// <summary>
        /// The <see cref="IScriptProcessor"/>.
        /// </summary>
        public IScriptProcessor ScriptProcessor { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptRunnerExtension{TValueType}" /> class.
        /// </summary>
        /// <param name="processorExtension">The <see cref="IProcessorExtension{TValueType}"/> used by this <see cref="IScriptRunnerExtension{TValueType}"/>.</param>
        /// <param name="environment">The <see cref="ICakeEnvironment"/>.</param>
        /// <param name="cakeLog">The <see cref="ICakeLog"/>.</param>
        /// <param name="scriptProcessor">The <see cref="IScriptProcessor"/>.</param>
        protected ScriptRunnerExtension(IProcessorExtension<TValueType> processorExtension, ICakeEnvironment environment, ICakeLog cakeLog, IScriptProcessor scriptProcessor)
        {
            ProcessorExtension = processorExtension;
            Environment = environment;
            Log = cakeLog;
            ScriptProcessor = scriptProcessor;
        }

        /// <summary>
        /// Defines installation instructions.
        /// </summary>
        /// <param name="values">The <see cref="IProcessorExtension{TValueType}"/> values.</param>
        /// <param name="result">The current executing <see cref="ScriptAnalyzerResult"/>.</param>
        /// <param name="scriptAnalyzerContext">The executing <see cref="IScriptAnalyzerContext"/>.</param>
        /// <param name="toolsPath">Installation path to tools.</param>
        public void Install(IEnumerable<TValueType> values, ref ScriptAnalyzerResult result, IScriptAnalyzerContext scriptAnalyzerContext, DirectoryPath toolsPath)
        {
            if (scriptAnalyzerContext == null)
            {
                throw new ArgumentNullException("scriptAnalyzerContext");
            }

            if (toolsPath == null)
            {
                throw new ArgumentNullException("toolsPath");
            }

            // Make the installation root absolute.
            toolsPath = toolsPath.MakeAbsolute(Environment);
            
            DoInstall(values, ref result, scriptAnalyzerContext, toolsPath);
        }

        /// <summary>
        /// Defines installation instructions.
        /// </summary>
        /// <param name="values">The processor values. is null if no <see cref="IProcessorExtension{TValueType}"/> values is set.</param>
        /// <param name="result">The current executing <see cref="ScriptAnalyzerResult"/>.</param>
        /// <param name="scriptAnalyzerContext">The executing <see cref="IScriptAnalyzerContext"/>.</param>
        /// <param name="toolsPath">Installation path to tools.</param>
        public abstract void DoInstall(IEnumerable<TValueType> values, ref ScriptAnalyzerResult result, IScriptAnalyzerContext scriptAnalyzerContext, DirectoryPath toolsPath);
    }
}
