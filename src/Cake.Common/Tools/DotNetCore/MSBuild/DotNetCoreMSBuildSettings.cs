// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Common.Tools.MSBuild;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.MSBuild
{
    /// <summary>
    /// Contains settings used by <see cref="DotNetCoreMSBuildBuilder" />.
    /// </summary>
    public sealed class DotNetCoreMSBuildSettings : DotNetCoreSettings
    {
        /// <summary>
        /// Gets or sets a value indicating whether to show detailed information at the end of the build log about the configurations that were built and how they were scheduled to nodes.
        /// </summary>
        public bool DetailedSummary { get; set; }

        /// <summary>
        /// Gets or sets extensions to ignore when determining which project file to build.
        /// </summary>
        public ICollection<string> IgnoreProjectExtensions { get; set; }

        /// <summary>
        /// Gets or sets the maximum number of concurrent processes to use when building.
        /// </summary>
        /// <remarks>
        /// If you don't include this switch, the default value is 1. If you specifying a value that is zero or less, MSBuild will use up to the number of processors in the computer.
        /// </remarks>
        public int? MaxCpuCount { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to exclude any MSBuild.rsp files automatically.
        /// </summary>
        public bool ExcludeAutoResponseFiles { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to display the startup banner and the copyright message.
        /// </summary>
        public bool NoLogo { get; set; }

        /// <summary>
        /// Gets the project-level properties to set or override.
        /// </summary>
        public IDictionary<string, ICollection<string>> Properties { get; }

        /// <summary>
        /// Gets the targets to build in the project.
        /// </summary>
        /// <remarks>
        /// If you specify any targets, they are run instead of any targets in the DefaultTargets attribute in the project file.
        /// </remarks>
        public ICollection<string> Targets { get; }

        /// <summary>
        /// Gets or sets the version of the Toolset to use to build the project.
        /// </summary>
        public MSBuildVersion? ToolVersion { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to validate the project file and, if validation succeeds, build the project.
        /// </summary>
        public bool ValidateProjectFile { get; set; }

        /// <summary>
        /// Gets the response files to use.
        /// </summary>
        /// <remarks>
        /// A response file is a text file that is used to insert command-line switches. For more information see https://docs.microsoft.com/en-gb/visualstudio/msbuild/msbuild-response-files
        /// </remarks>
        public ICollection<FilePath> ResponseFiles { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to log the build output of each MSBuild node to its own file.
        /// </summary>
        /// <remarks>
        /// The initial location for these files is the current directory. By default, the files are named "MSBuildNodeId.log". You can use the /fileLoggerParameters switch to specify the location of the files and other parameters for the fileLogger.
        /// If you name a log file by using the /fileLoggerParameters switch, the distributed logger will use that name as a template and append the node ID to that name when creating a log file for each node.
        /// </remarks>
        public bool DistributedFileLogger { get; set; }

        /// <summary>
        /// Gets the distributed loggers to use.
        /// </summary>
        /// <remarks>
        /// A distributed logger consists of a central and forwarding logger. MSBuild will attach an instance of the forwarding logger to each secondary node.
        /// For more information see https://msdn.microsoft.com/en-us/library/bb383987.aspx
        /// </remarks>
        public ICollection<MSBuildDistributedLogger> DistributedLoggers { get; }

        /// <summary>
        /// Gets or sets the parameters for the console logger.
        /// </summary>
        public MSBuildLoggerSettings ConsoleLoggerSettings { get; set; }

        /// <summary>
        /// Gets the file loggers to use.
        /// </summary>
        public ICollection<MSBuildFileLoggerSettings> FileLoggers { get; }

        /// <summary>
        /// Gets the loggers to use to log events from MSBuild.
        /// </summary>
        public ICollection<MSBuildLogger> Loggers { get; }

        /// <summary>
        /// Gets or sets a value indicating whether to disable the default console logger, and not log events to the console.
        /// </summary>
        public bool DisableConsoleLogger { get; set; }

        /// <summary>
        /// Gets the warning codes to treats as errors.
        /// </summary>
        /// <remarks>
        /// When a warning is treated as an error the target will continue to execute as if it was a warning but the overall build will fail.
        /// </remarks>
        public IList<string> WarningCodesAsError { get; }

        /// <summary>
        /// Gets or sets a value indicating how all warnings should be treated.
        /// </summary>
        public MSBuildTreatAllWarningsAs TreatAllWarningsAs { get; set; }

        /// <summary>
        /// Gets the warning codes to treats as low importance messages.
        /// </summary>
        public IList<string> WarningCodesAsMessage { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreMSBuildSettings"/> class.
        /// </summary>
        public DotNetCoreMSBuildSettings()
        {
            Properties = new Dictionary<string, ICollection<string>>(StringComparer.OrdinalIgnoreCase);
            Targets = new List<string>();
            ResponseFiles = new List<FilePath>();
            DistributedLoggers = new List<MSBuildDistributedLogger>();
            FileLoggers = new List<MSBuildFileLoggerSettings>();
            Loggers = new List<MSBuildLogger>();
            WarningCodesAsError = new List<string>();
            WarningCodesAsMessage = new List<string>();
            IgnoreProjectExtensions = new List<string>();
        }
    }
}
