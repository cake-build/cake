// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core.IO;
using Cake.Core.Scripting.Analysis;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Abstract line processor.
    /// </summary>
    internal abstract class LineProcessor
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
        /// <param name="analyzer">The analyzer.</param>
        /// <param name="line">The line.</param>
        /// <param name="replacement">The replacement for line, null if no replacement</param>
        /// <returns><c>true</c> if the line was processed
        /// by this processor; otherwise <c>false</c>.</returns>
        public abstract bool Process(IScriptAnalyzerContext analyzer, string line, out string replacement);

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
                // Concatenate the starting working directory
                // with the script file path.
                scriptLocation = _environment.WorkingDirectory
                    .CombineWithFilePath(path).GetDirectory();
            }
            return scriptLocation;
        }
    }
}
