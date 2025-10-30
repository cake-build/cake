// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Core
{
    /// <summary>
    /// Represent errors that occur during script execution.
    /// </summary>
    public sealed class CakeReportException : Exception
    {
        /// <summary>
        /// Gets or sets the Cake Report.
        /// </summary>
        public CakeReport Report { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportException"/> class.
        /// </summary>
        public CakeReportException()
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportException"/> class.
        /// </summary>
        /// <param name="message">The message that describes the error.</param>
        public CakeReportException(string message)
            : base(message)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportException"/> class.
        /// </summary>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CakeReportException(string message, Exception innerException)
            : base(message, innerException)
        {
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportException"/> class.
        /// </summary>
        /// <param name="report">The Cake Report.</param>
        public CakeReportException(CakeReport report)
        {
            Report = report;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportException"/> class.
        /// </summary>
        /// <param name="report">The Cake Report.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        public CakeReportException(CakeReport report, string message)
            : base(message)
        {
            Report = report;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="CakeReportException"/> class.
        /// </summary>
        /// <param name="report">The Cake Report.</param>
        /// <param name="message">The error message that explains the reason for the exception.</param>
        /// <param name="innerException">The exception that is the cause of the current exception, or a null reference (Nothing in Visual Basic) if no inner exception is specified.</param>
        public CakeReportException(CakeReport report, string message, Exception innerException)
            : base(message, innerException)
        {
            Report = report;
        }
    }
}