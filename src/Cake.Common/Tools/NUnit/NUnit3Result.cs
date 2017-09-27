// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;

namespace Cake.Common.Tools.NUnit
{
    /// <summary>
    /// Contains information for the results that should be exported.
    /// </summary>
    public sealed class NUnit3Result
    {
        /// <summary>
        /// Gets or sets the name of the XML result file.
        /// </summary>
        /// <value>
        /// The name of the XML result file. Defaults to <c>TestResult.xml</c>.
        /// </value>
        public FilePath FileName { get; set; }

        /// <summary>
        /// Gets or sets the format that the results should be in. <see cref="FileName"/> must be set to
        /// have any effect. Specify nunit2 to output the results in NUnit 2 xml format.
        /// nunit3 may be specified for NUnit 3 format, however this is the default. Additional
        /// formats may be supported in the future, check the NUnit documentation.
        /// </summary>
        /// <value>
        /// The format of the result file. Defaults to <c>nunit3</c>.
        /// </value>
        public string Format { get; set; }

        /// <summary>
        /// Gets or sets the file name of an XSL transform that will be applied to the results.
        /// </summary>
        /// <value>
        /// The name of an XSLT file that will be applied to the results.
        /// </value>
        public FilePath Transform { get; set; }
    }
}
