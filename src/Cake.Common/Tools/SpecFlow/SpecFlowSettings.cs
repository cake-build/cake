// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.SpecFlow
{
    /// <summary>
    /// Contains settings used by <see cref="SpecFlowTool{TSettings}"/>.
    /// </summary>
    public abstract class SpecFlowSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the generated Output File. Optional.
        /// Default: TestResult.html
        /// </summary>
        public FilePath Out { get; set; }

        /// <summary>
        /// Gets or sets the custom XSLT file to use, defaults to built-in stylesheet if not provided. Optional.
        /// Default: not specified
        /// </summary>
        public FilePath XsltFile { get; set; }
    }
}
