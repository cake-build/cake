using System;
using Cake.Core.IO;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Processor for #l directives.
    /// </summary>
    public sealed class LoadDirectiveProcessor : LineProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="LoadDirectiveProcessor"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public LoadDirectiveProcessor(ICakeEnvironment environment) 
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
            if (processor == null)
            {
                throw new ArgumentNullException("processor");
            }

            var tokens = Split(line);
            if (tokens.Length <= 0)
            {
                return false;
            }

            if (!tokens[0].Equals("#l", StringComparison.Ordinal) &&
                !tokens[0].Equals("#load", StringComparison.Ordinal))
            {
                return false;
            }

            var directoryPath = GetAbsoluteDirectory(currentScriptPath);
            var scriptPath = new FilePath(tokens[1].UnQuote()).MakeAbsolute(directoryPath);

            processor.Process(scriptPath, context);

            return true;
        }
    }
}