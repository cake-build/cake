using System.Collections.Generic;
using System.Globalization;
using Cake.Core;

namespace Cake.Common.Build.ContinuaCI.Data
{
    /// <summary>
    /// Provides Continua CI environment information for a current build.
    /// </summary>
    public sealed class ContinuaCIEnvironmentInfo : ContinuaCIInfo
    {
        private const string _prefix = "ContinuaCI";
        private readonly ContinuaCIProjectInfo _projectInfo;
        private readonly ContinuaCIConfigurationInfo _configurationInfo;
        private readonly ContinuaCIBuildInfo _buildInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuaCIEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public ContinuaCIEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            _projectInfo = new ContinuaCIProjectInfo(environment, string.Format(CultureInfo.InvariantCulture, "{0}.Project", _prefix));
            _buildInfo = new ContinuaCIBuildInfo(environment, string.Format(CultureInfo.InvariantCulture, "{0}.Build", _prefix));
            _configurationInfo = new ContinuaCIConfigurationInfo(environment, string.Format(CultureInfo.InvariantCulture, "{0}.Configuration", _prefix));
        }

        /// <summary>
        /// Gets Continua CI configuration information.
        /// </summary>
        /// <value>
        ///   The Continua CI configuration information.
        /// </value>
        public ContinuaCIConfigurationInfo Configuration
        {
            get { return _configurationInfo; }
        }

        /// <summary>
        /// Gets Continua CI project information.
        /// </summary>
        /// <value>
        ///   The Continua CI project information.
        /// </value>
        public ContinuaCIProjectInfo Project
        {
            get { return _projectInfo; }
        }

        /// <summary>
        /// Gets Continua CI build information.
        /// </summary>
        /// <value>
        ///   The Continua CI build information.
        /// </value>
        public ContinuaCIBuildInfo Build
        {
            get { return _buildInfo; }
        }

        /// <summary>
        /// Gets Continua CI build variables.
        /// </summary>
        /// <value>
        ///   The Continua CI build variables.
        /// </value>
        public IDictionary<string, string> Variable
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Variable", _prefix);
                return GetEnvironmentStringDictionary(key);
            }
        }

        /// <summary>
        /// Gets Continua CI build agent properties
        /// </summary>
        /// <value>
        ///   The Continua CI build agent properties.
        /// </value>
        public IDictionary<string, string> AgentProperty
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.AgentProperty", _prefix);
                return GetEnvironmentStringDictionary(key);
            }
        }

        /// <summary>
        /// Gets Continua CI product version.
        /// </summary>
        /// <value>
        ///   The Continua CI product version.
        /// </value>
        public string Version
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Version", _prefix);
                return GetEnvironmentString(key);
            }
        }
    }
}