// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.XBuild
{
    /// <summary>
    /// Contains settings used by <see cref="XBuildRunner"/>.
    /// </summary>
    public sealed class XBuildSettings : ToolSettings
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
        /// Gets or sets the tool version.
        /// </summary>
        /// <value>The tool version.</value>
        public XBuildToolVersion ToolVersion { get; set; }

        /// <summary>
        /// Gets or sets the configuration.
        /// </summary>
        /// <value>The configuration.</value>
        public string Configuration { get; set; }

        /// <summary>
        /// Gets or sets the amount of information to display in the build log.
        /// Each logger displays events based on the verbosity level that you set for that logger.
        /// </summary>
        /// <value>The build log verbosity.</value>
        public Verbosity Verbosity { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="XBuildSettings"/> class.
        /// </summary>
        public XBuildSettings()
        {
            _targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _properties = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);

            ToolVersion = XBuildToolVersion.Default;
            Configuration = string.Empty;
            Verbosity = Verbosity.Normal;
        }
    }
}
