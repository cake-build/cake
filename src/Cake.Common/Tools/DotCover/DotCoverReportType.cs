using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// Represents DotCover ReportType
    /// </summary>
    public enum DotCoverReportType
    {
        /// <summary>
        /// ReportType: <c>XML</c>
        /// </summary>
        XML = 0,

        /// <summary>
        /// ReportType: <c>HTML</c>
        /// </summary>
        HTML,

        /// <summary>
        /// ReportType: <c>JSON</c>
        /// </summary>
        JSON,

        /// <summary>
        /// ReportType: <c>DetailedXML</c>
        /// </summary>
        DetailedXML,

        /// <summary>
        /// ReportType: <c>NDependXML</c>
        /// </summary>
        NDependXML,
    }
}
