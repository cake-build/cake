using System;
using Cake.Core.IO;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Processor for Unix shebang lines (#!).
    /// </summary>
    public sealed class ShebangProcessor : LineProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ShebangProcessor"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public ShebangProcessor(ICakeEnvironment environment) 
            : base(environment)
        {
        }

        /// <summary>
        /// Processes the specified line.
        /// </summary>
        /// <param name="processor">The script processor.</param>
        /// <param name="context">The script processor context.</param>
        /// <param name="currentScriptPath">The current script path.</param>
        /// <param name="line">The line to process.</param>
        /// <returns>
        ///   <c>true</c> if the processor handled the line; otherwise <c>false</c>.
        /// </returns>
        public override bool Process(IScriptProcessor processor, ScriptProcessorContext context, FilePath currentScriptPath, string line)
        {
            // Remove all shebang lines that we encounter.
            return line.StartsWith("#!", StringComparison.OrdinalIgnoreCase);
        }
    }
}
