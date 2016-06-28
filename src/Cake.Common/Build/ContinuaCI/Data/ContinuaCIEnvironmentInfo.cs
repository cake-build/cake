// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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
        private const string Prefix = "ContinuaCI";

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuaCIEnvironmentInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public ContinuaCIEnvironmentInfo(ICakeEnvironment environment)
            : base(environment)
        {
            Project = new ContinuaCIProjectInfo(environment, string.Format(CultureInfo.InvariantCulture, "{0}.Project", Prefix));
            Build = new ContinuaCIBuildInfo(environment, string.Format(CultureInfo.InvariantCulture, "{0}.Build", Prefix));
            Configuration = new ContinuaCIConfigurationInfo(environment, string.Format(CultureInfo.InvariantCulture, "{0}.Configuration", Prefix));
        }

        /// <summary>
        /// Gets Continua CI configuration information.
        /// </summary>
        /// <value>
        ///   The Continua CI configuration information.
        /// </value>
        public ContinuaCIConfigurationInfo Configuration { get; }

        /// <summary>
        /// Gets Continua CI project information.
        /// </summary>
        /// <value>
        ///   The Continua CI project information.
        /// </value>
        public ContinuaCIProjectInfo Project { get; }

        /// <summary>
        /// Gets Continua CI build information.
        /// </summary>
        /// <value>
        ///   The Continua CI build information.
        /// </value>
        public ContinuaCIBuildInfo Build { get; }

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
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Variable", Prefix);
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
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.AgentProperty", Prefix);
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
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Version", Prefix);
                return GetEnvironmentString(key);
            }
        }
    }
}