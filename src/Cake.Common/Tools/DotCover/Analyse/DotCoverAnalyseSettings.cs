// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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
    }
}
