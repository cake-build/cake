using System;
using System.Collections.Generic;
using Cake.Common.Tools.DotCover;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.DotCover.Analyse
{
    /// <summary>
    /// Contains settings used by <see cref="DotCoverAnalyser" />.
    /// </summary>
    public sealed class DotCoverAnalyseSettings : DotCoverSettings
    {
        /// <summary>
        /// Gets or sets the type of the report.
        /// This represents the <c>/ReportType</c> option.
        /// The Default value is <see cref="DotCoverReportType.XML"/>.
        /// </summary>
        public DotCoverReportType ReportType { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotCoverAnalyseSettings"/> class.
        /// </summary>
        public DotCoverAnalyseSettings() : base()
        {
        }
    }
}