using Cake.Core.Diagnostics;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Abstract processor extension.
    /// </summary>
    public abstract class ProcessorExtension<TValueType> : IProcessorExtension<TValueType>
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
        /// The <see cref="IScriptProcessor"/>.
        /// </summary>
        public IScriptProcessor ScriptProcessor { get; private set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ProcessorExtension{TValueType}" /> class.
        /// </summary>
        /// <param name="environment"></param>
        /// <param name="cakeLog">The <see cref="ICakeLog"/>.</param>
        /// <param name="scriptProcessor">The <see cref="IScriptProcessor"/>.</param>
        protected ProcessorExtension(ICakeEnvironment environment, ICakeLog cakeLog, IScriptProcessor scriptProcessor)
        {
            Environment = environment;
            Log = cakeLog;
            ScriptProcessor = scriptProcessor;
        }

        /// <summary>
        /// Determind if this <see cref="IProcessorExtension{TValueType}"/> can process the directive <paramref name="alias"/>.
        /// </summary>
        /// <param name="alias">directive processor alias</param>
        /// <param name="value">the alias value</param>
        /// <returns>True if <see cref="IProcessorExtension{TValueType}"/> can process this <paramref name="alias"/>, else False</returns>
        public abstract bool CanProcessDirective(string alias, string value);

        /// <summary>
        /// Defines the <see cref="IScriptRunnerExtension{TValueType}"/> containing installation instructions.
        /// </summary>
        public abstract IScriptRunnerExtension<TValueType> ScriptRunnerExtension { get; }

        /// <summary>
        /// Processes the specified line.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <param name="line">The line.</param>
        /// <param name="replacement">The replacement for line, null if no replacement</param>
        /// <returns><c>true</c> if the line was processed
        /// by this processor; otherwise <c>false</c>.</returns>
        public abstract bool Process(IScriptAnalyzerContext analyzer, string line, out string replacement);

        /// <summary>
        /// Add a processor value to the <see cref="IScriptAnalyzerContext"/>.
        /// </summary>
        /// <param name="analyzer">The <see cref="IScriptAnalyzerContext"/></param>
        /// <param name="value">The value</param>
        protected void AddValue(IScriptAnalyzerContext analyzer, TValueType value)
        {
            analyzer.Script.ProcessorValues.Add(this, value);
        }
    }
}
