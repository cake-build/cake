using Cake.Core.Tooling;

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
    }
}
