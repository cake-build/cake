// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core.IO;

namespace Cake.Common.Tools.SpecFlow.StepDefinitionReport
{
    /// <summary>
    /// Contains settings used by <see cref="SpecFlowStepDefinitionReporter"/>.
    /// </summary>
    public sealed class SpecFlowStepDefinitionReportSettings : SpecFlowSettings
    {
        /// <summary>
        /// Gets or sets the path for the compiled SpecFlow project. Optional.
        /// Default: bin\Debug
        /// </summary>
        public DirectoryPath BinFolder { get; set; }
    }
}
