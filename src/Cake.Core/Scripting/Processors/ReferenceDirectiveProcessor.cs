using System;
using Cake.Core.IO;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Processor for #r directives.
    /// </summary>
    public sealed class ReferenceDirectiveProcessor : LineProcessor
    {
        private readonly IFileSystem _fileSystem;

        /// <summary>
        /// Initializes a new instance of the <see cref="ReferenceDirectiveProcessor"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        public ReferenceDirectiveProcessor(IFileSystem fileSystem, ICakeEnvironment environment) 
            : base(environment)
        {
            _fileSystem = fileSystem;
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
            var tokens = Split(line);
            if (tokens.Length <= 0)
            {
                return false;
            }

            if (!tokens[0].Equals("#r", StringComparison.Ordinal))
            {
                return false;
            }

            var referencePath = new FilePath(tokens[1].UnQuote());

            var directoryPath = GetAbsoluteDirectory(currentScriptPath);
            var absoluteReferencePath = referencePath.MakeAbsolute(directoryPath);

            context.AddReference(_fileSystem.Exist(absoluteReferencePath) 
                ? absoluteReferencePath.FullPath : referencePath.FullPath);

            return true;
        }
    }
}