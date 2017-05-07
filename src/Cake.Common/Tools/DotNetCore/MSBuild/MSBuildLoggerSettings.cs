// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// Represents the common settings for a logger.
    /// </summary>
    public class MSBuildLoggerSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show the time that’s spent in tasks, targets, and projects.
        /// </summary>
        public bool PerformanceSummary { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the error and warning summary at the end.
        /// </summary>
        public bool NoSummary { get; set; }

        /// <summary>
        /// Gets or sets value that indicates the level of summary output at the end for the logger.
        /// </summary>
        /// <remarks>Default is to show errors and summary.</remarks>
        public MSBuildLoggerOutputLevel SummaryOutputLevel { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to hide the list of items and properties that would appear at the start of each project build if the verbosity level is set to diagnostic.
        /// </summary>
        public bool HideItemAndPropertyList { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show TaskCommandLineEvent messages.
        /// </summary>
        public bool ShowCommandLine { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the timestamp as a prefix to any message.
        /// </summary>
        public bool ShowTimestamp { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to show the event Id for each started event, finished event, and message.
        /// </summary>
        public bool ShowEventId { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to not align the text to the size of the console buffer.
        /// </summary>
        public bool ForceNoAlign { get; set; }

        /// <summary>
        /// Gets or sets value that indicates the type of console color to use for all logging messages.
        /// </summary>
        public MSBuildConsoleColorType ConsoleColorType { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the multiprocessor logging style of output when running in non-multiprocessor mode.
        /// </summary>
        public bool DisableMultiprocessorLogging { get; set; }

        /// <summary>
        /// Gets or sets a value that overrides the /verbosity setting for this logger.
        /// </summary>
        public DotNetCoreVerbosity? Verbosity { get; set; }
    }
}