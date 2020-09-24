// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Text;

namespace Cake.Core.Scripting.Processors
{
    /// <summary>
    /// Abstract line processor.
    /// </summary>
    internal abstract class LineProcessor
    {
        /// <summary>
        /// Processes the specified line.
        /// </summary>
        /// <param name="analyzer">The analyzer.</param>
        /// <param name="line">The line.</param>
        /// <param name="replacement">The replacement for line, null if no replacement.</param>
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
                throw new ArgumentNullException(nameof(line));
            }

            return QuoteAwareStringSplitter.Split(line).ToArray();
        }
    }
}