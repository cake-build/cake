// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Core.Scripting.Analysis
{
    /// <summary>
    /// Represents the script analyzer mode.
    /// </summary>
    public enum ScriptAnalyzerMode
    {
        /// <summary>
        /// Analyzes everything.
        /// </summary>
        Everything = 0,

        /// <summary>
        /// Analyzes modules.
        /// </summary>
        Modules = 1,
    }
}