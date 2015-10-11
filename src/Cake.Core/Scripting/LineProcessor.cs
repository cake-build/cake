using System;
using Cake.Core.IO;

namespace Cake.Core.Scripting
{
    /// <summary>
    /// Abstract line processor.
    /// </summary>
    public abstract class LineProcessor
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="LineProcessor"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        protected LineProcessor(ICakeEnvironment environment)
        {
            _environment = environment;
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
        public abstract bool Process(IScriptProcessor processor, ScriptProcessorContext context, FilePath currentScriptPath, string line);

        /// <summary>
        /// Splits the specified line into tokens.
        /// </summary>
        /// <param name="line">The line to split.</param>
        /// <returns>The parts that make up the line.</returns>
        protected static string[] Split(string line)
        {
            if (line == null)
            {
                throw new ArgumentNullException("line");
            }
            return line.Split(new[] { ' ' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Gets the absolute directory for the path.
        /// </summary>
        /// <param name="path">The path.</param>
        /// <returns>The absolute directory path.</returns>
        protected DirectoryPath GetAbsoluteDirectory(FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // Get the script location.
            var scriptLocation = path.GetDirectory();
            if (scriptLocation.IsRelative)
            {
                // Concatinate the starting working directory
                // with the script file path.
                scriptLocation = _environment.WorkingDirectory
                    .CombineWithFilePath(path).GetDirectory();
            }
            return scriptLocation;
        }
    }
}