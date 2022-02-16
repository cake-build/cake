// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.Scripting.Analysis
{
    /// <summary>
    /// Represents settings for the script analyzer.
    /// </summary>
    public sealed class ScriptAnalyzerSettings
    {
        /// <summary>
        /// Gets or sets the analyzer mode.
        /// </summary>
        public ScriptAnalyzerMode Mode { get; set; } = ScriptAnalyzerMode.Everything;
    }
}