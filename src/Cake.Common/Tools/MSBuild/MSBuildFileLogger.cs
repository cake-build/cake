// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Contains settings for specifying a MSBuild file logger.
    /// </summary>
    public class MSBuildFileLogger
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildFileLogger"/> class.
        /// </summary>
        public MSBuildFileLogger()
        {
        }

        /// <summary>
        /// Gets or sets a value indicating whether PerformanceSummary will Show the time that’s spent in tasks, targets, and projects.
        /// </summary>
        public bool PerformanceSummaryEnabled { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether Summary will Show the error and warning summary at the end.
        /// </summary>
        public bool SummaryDisabled { get; set; }

        /// <summary>
        /// Gets or sets show ErrorsOnly, WarningsOnly, or All.
        /// </summary>
        public MSBuildFileLoggerOutput MSBuildFileLoggerOutput { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether NoItemAndPropertyList will be set to Don't show the list of items and properties that would appear at the start of each project build if the verbosity level is set to diagnostic.
        /// </summary>
        public bool HideVerboseItemAndPropertyList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowCommandLine. Show TaskCommandLineEvent messages.
        /// </summary>
        public bool ShowCommandLine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowTimestamp. Show the timestamp as a prefix to any message.
        /// </summary>
        public bool ShowTimestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether ShowEventId. Show the event ID for each started event, finished event, and message.
        /// </summary>
        public bool ShowEventId { get; set; }

        /// <summary>
        /// Gets or sets Verbosity. Override the /verbosity setting for this logger.
        /// Specify the following verbosity levels: q[uiet], m[inimal], n[ormal], v[erbose] (detailed), and diag[nostic].
        /// </summary>
        public Verbosity? Verbosity { get; set; }

        /// <summary>
        /// Gets or sets LogFile. The path to the log file into which the build log is written.
        /// An empty string will use msbuild.log.
        /// </summary>
        public FilePath LogFile { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the build log is appended to the log file or overwrites it. When true, the build log is appended to the log file.
        /// </summary>
        public bool AppendToLogFile { get; set; }

        /// <summary>
        /// Gets or sets Specifies the encoding for the file (for example, UTF-8, Unicode, or ASCII).
        /// </summary>
        public string Encoding { get; set; }

        /// <summary>
        /// Process the file logger config and return parameters as a string.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <returns>The parameters separated by semi-colons.</returns>
        public string GetParameters(ICakeEnvironment environment)
        {
            var parameters = new List<string>();
            parameters.Add(LogFile != null ? $"logfile={LogFile.MakeAbsolute(environment).FullPath.Quote()}" : null);
            parameters.Add(!string.IsNullOrWhiteSpace(Encoding) ? $"Encoding={Encoding}" : null);
            parameters.Add(AppendToLogFile ? "Append" : null);
            parameters.Add(PerformanceSummaryEnabled ? "PerformanceSummary" : null);
            parameters.Add(SummaryDisabled ? "NoSummary" : null);
            parameters.Add(MSBuildFileLoggerOutput == MSBuildFileLoggerOutput.ErrorsOnly ? "ErrorsOnly" : null);
            parameters.Add(MSBuildFileLoggerOutput == MSBuildFileLoggerOutput.WarningsOnly ? "WarningsOnly" : null);
            parameters.Add(HideVerboseItemAndPropertyList ? "NoItemAndPropertyList" : null);
            parameters.Add(ShowCommandLine ? "ShowCommandLine" : null);
            parameters.Add(ShowTimestamp ? "ShowTimestamp" : null);
            parameters.Add(ShowEventId ? "ShowEventId" : null);
            parameters.Add(Verbosity != null ? $"Verbosity={Verbosity.Value.GetMSBuildVerbosityName()}" : null);

            return string.Join(";", parameters.Where(p => p != null));
        }
    }
}
