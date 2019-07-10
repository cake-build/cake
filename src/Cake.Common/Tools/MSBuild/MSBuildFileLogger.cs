// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Contains settings for specifying a MSBuild file logger.
    /// </summary>
    public class MSBuildFileLogger : MSBuildLoggerSettings
    {
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
            parameters.Add(LogFile != null ? $"LogFile={LogFile.MakeAbsolute(environment).FullPath.Quote()}" : null);
            parameters.Add(!string.IsNullOrWhiteSpace(Encoding) ? $"Encoding={Encoding}" : null);
            parameters.Add(AppendToLogFile ? "Append" : null);
            parameters.Add(PerformanceSummary ? "PerformanceSummary" : null);
            parameters.Add(NoSummary ? "NoSummary" : null);
            parameters.Add(SummaryOutputLevel == MSBuildLoggerOutputLevel.ErrorsOnly ? "ErrorsOnly" : null);
            parameters.Add(SummaryOutputLevel == MSBuildLoggerOutputLevel.WarningsOnly ? "WarningsOnly" : null);
            parameters.Add(HideItemAndPropertyList ? "NoItemAndPropertyList" : null);
            parameters.Add(ShowCommandLine ? "ShowCommandLine" : null);
            parameters.Add(ShowTimestamp ? "ShowTimestamp" : null);
            parameters.Add(ShowEventId ? "ShowEventId" : null);
            parameters.Add(Verbosity != null ? $"Verbosity={Verbosity.Value}" : null);
            parameters.Add(ForceNoAlign ? "ForceNoAlign" : null);
            parameters.Add(ConsoleColorType == MSBuildConsoleColorType.Disabled ? "DisableConsoleColor" : null);
            parameters.Add(ConsoleColorType == MSBuildConsoleColorType.ForceAnsi ? "ForceConsoleColor" : null);
            parameters.Add(DisableMultiprocessorLogging ? "DisableMPLogging" : null);

            return string.Join(";", parameters.Where(p => p != null));
        }
    }
}
