namespace Cake.Common.Tools.ReportGenerator
{
    /// <summary>
    /// Represents ReportGenerator's output formats
    /// </summary>
    public enum ReportGeneratorReportType
    {
        /// <summary>
        /// Badge report.
        /// </summary>
        Badges = 1,

        /// <summary>
        /// Html report.
        /// </summary>
        Html = 2,

        /// <summary>
        /// Html summary report.
        /// </summary>
        HtmlSummary = 3,

        /// <summary>
        /// Latex report.
        /// </summary>
        Latex = 4,

        /// <summary>
        /// Latex summary report.
        /// </summary>
        LatexSummary = 5,

        /// <summary>
        /// Text summary report.
        /// </summary>
        TextSummary = 6,

        /// <summary>
        /// Xml report.
        /// </summary>
        Xml = 7,

        /// <summary>
        /// Xml summary report.
        /// </summary>
        XmlSummary = 8
    }
}