using System;
using Cake.Core.IO;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Processor for using statements.
    /// </summary>
    public sealed class UsingStatementProcessor : LineProcessor
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UsingStatementProcessor"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public UsingStatementProcessor(ICakeEnvironment environment) 
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
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            var tokens = Split(line);
            if (tokens.Length <= 0)
            {
                return false;
            }

            if (!tokens[0].Equals("using", StringComparison.Ordinal))
            {
                return false;
            }

            var @namespace = tokens[1].TrimEnd(new[] { ';' });
            context.AddNamespace(@namespace);

            return true;
        }
    }
}