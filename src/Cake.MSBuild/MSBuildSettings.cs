using System;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.MSBuild
{
    public sealed class MSBuildSettings
    {
        private readonly FilePath _solution;
        private readonly HashSet<string> _targets;
        private readonly Dictionary<string, string> _properties;        

        public FilePath Solution
        {
            get { return _solution; }
        }

        public ISet<string> Targets
        {
            get { return _targets; }
        }

        public IDictionary<string, string> Properties
        {
            get { return _properties; }
        }

        public PlatformTarget PlatformTarget { get; set; }
        public MSBuildToolVersion ToolVersion { get; set; }
        public string Configuration { get; set; }

        public MSBuildSettings(FilePath solution)
        {
            if (solution == null)
            {
                throw new ArgumentNullException("solution");
            }
            
            _solution = solution;
            _targets = new HashSet<string>(StringComparer.OrdinalIgnoreCase);
            _properties = new Dictionary<string, string>(StringComparer.OrdinalIgnoreCase);

            PlatformTarget = PlatformTarget.MSIL;
            ToolVersion = MSBuildToolVersion.VS2013;
            Configuration = string.Empty;
        }
    }
}
