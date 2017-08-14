// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;

namespace Cake.Core.Scripting.Analysis
{
    /// <summary>
    /// Represents a script analysis error.
    /// </summary>
    public sealed class ScriptAnalyzerError
    {
        /// <summary>
        /// Gets the file containing the error.
        /// </summary>
        public FilePath File { get; }

        /// <summary>
        /// Gets the line number for the error.
        /// </summary>
        public int Line { get; }

        /// <summary>
        /// Gets the error message.
        /// </summary>
        public string Message { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="ScriptAnalyzerError"/> class.
        /// </summary>
        /// <param name="file">The file containing the error.</param>
        /// <param name="line">The line number for the error.</param>
        /// <param name="message">The error message.</param>
        public ScriptAnalyzerError(FilePath file, int line, string message)
        {
            if (file == null)
            {
                throw new ArgumentNullException(nameof(file));
            }
            if (message == null)
            {
                throw new ArgumentNullException(nameof(message));
            }

            File = file;
            Line = line;
            Message = message;
        }
    }
}