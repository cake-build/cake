// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.SpecFlow.TestExecutionReport
{
    /// <summary>
    /// Contains settings used by <see cref="SpecFlowTestExecutionReporter"/>.
    /// </summary>
    public sealed class SpecFlowTestExecutionReportSettings : SpecFlowSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether exceptions from the
        /// intercepted action should be rethrown after report generation.
        /// </summary>
        public bool ThrowOnTestFailure { get; set; }
    }
}