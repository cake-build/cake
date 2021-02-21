// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.ReportGenerator
{
    /// <summary>
    /// Represents ReportGenerator's output formats.
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
        XmlSummary = 8,

        /// <summary>
        /// Cobertura report
        /// </summary>
        Cobertura = 9,

        /// <summary>
        /// CSV summary report
        /// </summary>
        CsvSummary = 10,

        /// <summary>
        /// HTML Chart report
        /// </summary>
        HtmlChart = 11,

        /// <summary>
        /// Html report with inline CSS and JavaScript
        /// </summary>
        HtmlInline = 12,

        /// <summary>
        /// Same as HTMLInline but with modified CSS that matches the look and feel of Azure Pipelines.
        /// </summary>
        HtmlInline_AzurePipelines = 13,

        /// <summary>
        /// A single PNG file containing a chart with historic coverage information.
        /// </summary>
        PngChart = 14,

        /// <summary>
        /// Same as HTML but packaged into a single MHTML file.
        /// </summary>
        MHtml = 15,

        /// <summary>
        /// Creates xml report in SonarQube 'Generic Test Data' format.
        /// </summary>
        /// <remarks>
        /// Requires ReportGenerator 4.0.6+
        /// </remarks>
        SonarQube = 16,

        /// <summary>
        /// Same as HTMLInline but with modified CSS that matches the dark look and feel of Azure Pipelines.
        /// </summary>
        /// <remarks>
        /// Requires ReportGenerator 4.0.10+
        /// </remarks>
        HtmlInline_AzurePipelines_Dark = 17,

        /// <summary>
        /// Creates xml report in Clover format.
        /// </summary>
        /// <remarks>
        /// Requires ReportGenerator 4.0.6+
        /// </remarks>
        Clover = 18,

        /// <summary>
        /// Creates summary report in JSON format.
        /// </summary>
        /// <remarks>
        /// Requires ReportGenerator 4.5.2+
        /// </remarks>
        JsonSummary = 19,

        /// <summary>
        /// Creates summary report in lcov format.
        /// </summary>
        /// <remarks>
        /// Requires ReportGenerator 4.3.0+
        /// </remarks>
        lcov = 20,

        /// <summary>
        /// Outputs summary report as TeamCity statistics messages.
        /// </summary>
        /// <remarks>
        /// Requires ReportGenerator 4.1.3+
        /// </remarks>
        TeamCitySummary = 21
    }
}