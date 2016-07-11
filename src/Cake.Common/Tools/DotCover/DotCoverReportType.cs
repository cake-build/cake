// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Diagnostics.CodeAnalysis;

namespace Cake.Common.Tools.DotCover
{
    /// <summary>
    /// Represents DotCover ReportType
    /// </summary>
    [SuppressMessage("ReSharper", "InconsistentNaming")]
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
