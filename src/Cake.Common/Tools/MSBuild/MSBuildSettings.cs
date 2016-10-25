// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Contains settings used by <see cref="MSBuildRunner"/>.
    /// </summary>
    public sealed class MSBuildSettings : ToolSettings
    {
        private readonly HashSet<string> _targets;
        private readonly Dictionary<string, IList<string>> _properties;
        private readonly List<MSBuildLogger> _loggers;
        private readonly List<MSBuildFileLogger> _fileLoggers;

        /// <summary>
        /// Gets the targets.
        /// </summary>
        /// <value>The targets.</value>
        public ISet<string> Targets => _targets;

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public IDictionary<string, IList<string>> Properties => _properties;

        /// <summary>
        /// Gets or sets the platform target.
        /// </summary>
        /// <value>The platform target.</value>
        public PlatformTarget? PlatformTarget { get; set; }

        /// <summary>
        /// Gets or sets the MSBuild platform.
        /// </summary>
        /// <value>The MSBuild platform.</value>
        public MSBuildPlatform MSBuildPlatform { get; set; }

        /// <summary>
        /// Gets or sets the tool version.
        /// </summary>
        /// <value>The tool version.</value>
        public MSBuildToolVersion ToolVersion { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets the maximum CPU count.
        /// If this value is zero, MSBuild will use as many processes as
        /// there are available CPUs to build the project. If not set
        /// MSBuild compile projects in this solution one at a time.
        /// </summary>
        /// <value>The maximum CPU count.</value>
        public int? MaxCpuCount { get; set; }

        /// <summary>
        /// Gets or sets whether or not node reuse is used.
        /// When you’re doing multiple builds in a row, this helps reduce your total build time,
        /// by avoiding the start up costs of each MSBuild child node.
        /// </summary>
        public bool? NodeReuse { get; set; }

        /// <summary>
        /// Gets or sets whether or not detailed summary is created.
        /// Shows detailed information at the end of the build
        /// about the configurations built and how they were
        /// scheduled to nodes.
        /// </summary>
        public bool? DetailedSummary { get; set; }

        /// <summary>
        /// Gets or sets whether or not information is logged to the console.
        /// Disable the default console logger and do not log events
        /// to the console.
        /// </summary>
        public bool? NoConsoleLogger { get; set; }

        /// <summary>
        /// Gets or sets the amount of information to display in the build log.
        /// Each logger displays events based on the verbosity level that you set for that logger.
        /// </summary>
        /// <value>The build log verbosity.</value>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Gets the loggers.
        /// </summary>
        public ICollection<MSBuildLogger> Loggers => _loggers;

        /// <summary>
        /// Gets the file loggers
        /// </summary>
        public ICollection<MSBuildFileLogger> FileLoggers => _fileLoggers;

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildSettings"/> class.
        /// </summary>
        public MSBuildSettings()
        {
            _targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _properties = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);
            _loggers = new List<MSBuildLogger>();
            _fileLoggers = new List<MSBuildFileLogger>();

            ToolVersion = MSBuildToolVersion.Default;
            Configuration = string.Empty;
            Verbosity = Verbosity.Normal;
            MSBuildPlatform = MSBuildPlatform.Automatic;
        }
    }
}