// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.DotCover.Report
{
    /// <summary>
    /// Contains settings used by <see cref="DotCoverReporter" />.
    /// </summary>
    public sealed class DotCoverReportSettings : DotCoverSettings
    {
        /// <summary>
        /// Gets or sets the type of the report.
        /// This represents the <c>/ReportType</c> option.
        /// The Default value is <see cref="DotCoverReportType.XML"/>.
        /// </summary>
        public DotCoverReportType ReportType { get; set; }

        /// <summary>
        /// Gets or sets the path to save a formatted JSON report.
        /// This represents the <c>--json-report-output</c> option.
        /// </summary>
        public FilePath JsonReportOutput { get; set; }

        /// <summary>
        /// Gets or sets granularity for including covering tests in JSON reports: [none|assembly|type|method|statement]
        /// This represents the <c>--json-report-covering-tests-scope</c> option.
        /// </summary>
        public DotCoverReportScope? JsonReportCoveringTestsScope { get; set; }

        /// <summary>
        /// Gets or sets the path to save a formatted XML report.
        /// This represents the <c>--xml-report-output</c> option.
        /// </summary>
        public FilePath XmlReportOutput { get; set; }

        /// <summary>
        /// Gets or sets granularity for including covering tests in XML reports: [none|assembly|type|method|statement]
        /// This represents the <c>--xml-report-covering-tests-scope</c> option.
        /// </summary>
        public DotCoverReportScope? XmlReportCoveringTestsScope { get; set; }
    }
}