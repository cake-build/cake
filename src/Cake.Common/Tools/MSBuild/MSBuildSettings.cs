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

        /// <summary>
        /// Gets the targets.
        /// </summary>
        /// <value>The targets.</value>
        public ISet<string> Targets
        {
            get { return _targets; }
        }

        /// <summary>
        /// Gets the properties.
        /// </summary>
        /// <value>The properties.</value>
        public IDictionary<string, IList<string>> Properties
        {
            get { return _properties; }
        }

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
        /// </summary>
        /// <value>The maximum CPU count.</value>
        public int MaxCpuCount { get; set; }

        /// <summary>
        /// Gets or sets whether or not node reuse is used.
        /// When you’re doing multiple builds in a row, this helps reduce your total build time,
        /// by avoiding the start up costs of each MSBuild child node.
        /// </summary>
        public bool? NodeReuse { get; set; }

        /// <summary>
        /// Gets or sets the amount of information to display in the build log.
        /// Each logger displays events based on the verbosity level that you set for that logger.
        /// </summary>
        /// <value>The build log verbosity.</value>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="MSBuildSettings"/> class.
        /// </summary>
        public MSBuildSettings()
        {
            _targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _properties = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);

            ToolVersion = MSBuildToolVersion.Default;
            Configuration = string.Empty;
            Verbosity = Verbosity.Normal;
            MSBuildPlatform = MSBuildPlatform.Automatic;
        }
    }
}
