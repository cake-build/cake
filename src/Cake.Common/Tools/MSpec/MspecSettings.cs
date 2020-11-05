// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSpec
{
    /// <summary>
    ///     Contains settings used by <see cref="MSpecRunner" />.
    /// </summary>
    public sealed class MSpecSettings : ToolSettings
    {
        /// <summary>
        ///     Gets or sets the path to the filter file specifying contexts to execute(full type name, one per line). Takes precedence over
        ///     tags.
        /// </summary>
        public FilePath Filters { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether reporting for TeamCity CI integration(also auto - detected).
        /// </summary>
        /// <value>
        ///     <c>true</c> to turn on TeamCity service messages; otherwise, <c>false</c>.
        /// </value>
        public bool TeamCity { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to suppress colored console output.
        /// </summary>
        /// <value>
        ///     <c>true</c> disable color output; otherwise, <c>false</c>.
        /// </value>
        public bool NoColor { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether an XML report should be generated.
        /// </summary>
        /// <value>
        ///     <c>true</c> if an XML report should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool XmlReport { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether an HTML report should be generated.
        /// </summary>
        /// <value>
        ///     <c>true</c> if an HTML report should be generated; otherwise, <c>false</c>.
        /// </value>
        public bool HtmlReport { get; set; }

        /// <summary>
        ///     Gets or sets the name that should be used for the HTML and XML reports.
        /// </summary>
        /// <value>The custom report name.</value>
        public string ReportName { get; set; }

        /// <summary>
        ///      Gets or sets Executes all specifications in contexts with these comma delimited tags. Ex. - i "foo,bar,foo_bar".
        /// </summary>
        public string Include { get; set; }

        /// <summary>
        ///     Gets or sets Exclude specifications in contexts with these comma delimited tags. Ex. - x "foo,bar,foo_bar".
        /// </summary>
        public string Exclude { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to show time-related information in HTML output.
        /// </summary>
        public bool TimeInfo { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to suppress progress output(print fatal errors, failures and summary).
        /// </summary>
        public bool Silent { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to print dotted progress output.
        /// </summary>
        public bool Progress { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to wait 15 seconds for debugger to be attached.
        /// </summary>
        public bool Wait { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to disable TeamCity autodetection.
        /// </summary>
        public bool NoTeamCity { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to enable reporting for AppVeyor CI integration (also auto-detected).
        /// </summary>
        public bool AppVeyor { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to disable AppVeyor autodetection.
        /// </summary>
        public bool NoAppVeyor { get; set; }

        /// <summary>
        ///     Gets or sets output directory for reports.
        /// </summary>
        public DirectoryPath OutputDirectory { get; set; }

        /// <summary>
        ///     Gets or sets a value indicating whether to use X86.
        /// </summary>
        public bool UseX86 { get; set; }
    }
}