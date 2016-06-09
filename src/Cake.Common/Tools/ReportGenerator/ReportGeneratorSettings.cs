// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.ReportGenerator
{
    /// <summary>
    /// Contains settings used by <see cref="ReportGeneratorRunner"/>.
    /// </summary>
    public sealed class ReportGeneratorSettings : ToolSettings
    {
        /// <summary>
        /// Gets or sets the list of coverage reports that should be parsed.
        /// </summary>
        public ICollection<ReportGeneratorReportType> ReportTypes { get; set; }

        /// <summary>
        /// Gets or sets the directories which contain the corresponding source code.
        /// The source files are used if coverage report contains classes without path information.
        /// </summary>
        public ICollection<DirectoryPath> SourceDirectories { get; set; }

        /// <summary>
        /// Gets or sets the directory for storing persistent coverage information.
        /// Can be used in future reports to show coverage evolution.
        /// </summary>
        public DirectoryPath HistoryDirectory { get; set; }

        /// <summary>
        /// Gets or sets the list of assemblies that should be included or excluded in the report.
        /// Exclusion filters take precedence over inclusion filters.
        /// Wildcards are allowed.
        /// </summary>
        public ICollection<string> AssemblyFilters { get; set; }

        /// <summary>
        /// Gets or sets the list of classes that should be included or excluded in the report.
        /// Exclusion filters take precedence over inclusion filters.
        /// Wildcards are allowed.
        /// </summary>
        public ICollection<string> ClassFilters { get; set; }

        /// <summary>
        /// Gets or sets the verbosity level of the log messages.
        /// </summary>
        public ReportGeneratorVerbosity? Verbosity { get; set; }
    }
}
