using System;
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSBuild
{
    /// <summary>
    /// Contains settings used by <see cref="MSBuildRunner"/>.
    /// </summary>
    public sealed class MSBuildSettings
    {
        private readonly FilePath _solution;
        private readonly HashSet<string> _targets;
        private readonly Dictionary<string, IList<string>> _properties;

        /// <summary>
        /// Gets the solution path.
        /// </summary>
        /// <value>The solution.</value>
        public FilePath Solution
        {
            get { return _solution; }
        }

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
        public PlatformTarget PlatformTarget { get; set; }

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
        /// <param name="solution">The solution.</param>
        public MSBuildSettings(FilePath solution)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }
            
            _solution = solution;
            _targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _properties = new Dictionary<string, IList<string>>(StringComparer.OrdinalIgnoreCase);

            PlatformTarget = PlatformTarget.MSIL;
            ToolVersion = MSBuildToolVersion.Default;
            Configuration = string.Empty;
            Verbosity = Verbosity.Normal;
        }
    }
}
