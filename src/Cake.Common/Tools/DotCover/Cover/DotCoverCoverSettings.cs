// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.DotCover.Cover
{
    /// <summary>
    /// Contains settings used by <see cref="DotCoverCoverer" />.
    /// </summary>
    public sealed class DotCoverCoverSettings : DotCoverCoverageSettings
    {
        /// <summary>
        /// Gets or sets the path to save formatted JSON report.
        /// This represents the <c>--json-report-output</c> option.
        /// </summary>
        public FilePath JsonReportOutput { get; set; }

        /// <summary>
        /// Gets or sets the granularity for including covering tests in JSON reports.
        /// This represents the <c>--json-report-covering-tests-scope</c> option.
        /// </summary>
        public DotCoverReportScope? JsonReportCoveringTestsScope { get; set; }

        /// <summary>
        /// Gets or sets the path to save formatted XML report.
        /// This represents the <c>--xml-report-output</c> option.
        /// </summary>
        public FilePath XmlReportOutput { get; set; }

        /// <summary>
        /// Gets or sets the granularity for including covering tests in XML reports.
        /// This represents the <c>--xml-report-covering-tests-scope</c> option.
        /// </summary>
        public DotCoverReportScope? XmlReportCoveringTestsScope { get; set; }

        /// <summary>
        /// Gets or sets the directory for temporary files.
        /// This represents the <c>--temporary-directory</c> option.
        /// </summary>
        public DirectoryPath TemporaryDirectory { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to control the coverage session using the profiler API.
        /// This represents the <c>--use-api</c> option.
        /// </summary>
        public bool UseApi { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable loading of NGen images during coverage.
        /// This represents the <c>--no-ngen</c> option.
        /// </summary>
        public bool NoNGen { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to use the legacy command syntax.
        /// When true, uses old format like '/TargetExecutable="/path"'.
        /// When false, uses new format like '--target-executable "/path"'.
        /// Default is false (new format).
        /// </summary>
        public bool UseLegacySyntax { get; set; }
    }
}