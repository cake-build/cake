// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;

namespace Cake.Core.Scripting.Analysis
{
    /// <summary>
    /// Represents the script analyzer responsible for
    /// parsing and analyzing scripts.
    /// </summary>
    public interface IScriptAnalyzer
    {
        /// <summary>
        /// Analyzes the specified script path.
        /// </summary>
        /// <param name="path">The path to the script to analyze.</param>
        /// <returns>The script analysis result.</returns>
        ScriptAnalyzerResult Analyze(FilePath path);
    }
}
