// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Xml.Linq;
using System.Xml.XPath;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.TeamCity.Data
{
    /// <summary>
    /// Provides TeamCity build information for a current build.
    /// </summary>
    public class TeamCityBuildInfo : TeamCityInfo
    {
        private DateTimeOffset? _startDateTime;

        /// <summary>
        /// Gets the build configuration name.
        /// </summary>
        /// <value>
        /// The build configuration name.
        /// </value>
        public string BuildConfName => GetEnvironmentString("TEAMCITY_BUILDCONF_NAME");

        /// <summary>
        /// Gets the build number.
        /// </summary>
        /// <value>
        /// The build number.
        /// </value>
        public string Number => GetEnvironmentString("BUILD_NUMBER");

        /// <summary>
        /// Gets the build start date and time.
        /// </summary>
        /// <value>
        /// The build start date and time if available, or <see langword="null"/>.
        /// </value>
        /// <remarks>
        /// The build start date and time are obtained from reading two environment variables
        /// BUILD_START_DATE (yyyyMMdd) and BUILD_START_TIME (HHmmss) are automatically set by JetBrain's
        /// <see href="https://confluence.jetbrains.com/display/TW/Groovy+plug">Groovy plug</see> plugin.
        /// </remarks>
        public DateTimeOffset? StartDateTime
        {
            get
            {
                if (_startDateTime.HasValue)
                {
                    return _startDateTime;
                }

                var startDate = GetEnvironmentString("BUILD_START_DATE"); // yyyyMMdd
                var startTime = GetEnvironmentString("BUILD_START_TIME"); // HHmmss

                if (DateTimeOffset.TryParseExact($"{startDate}{startTime}", "yyyyMMddHHmmss", CultureInfo.InvariantCulture,
                    DateTimeStyles.AssumeLocal, out var startDateTime))
                {
                    _startDateTime = startDateTime;
                }

                return _startDateTime;
            }
        }

        /// <summary>
        /// Gets the branch display name.
        /// </summary>
        /// <value>
        /// The branch display name.
        /// </value>
        public string BranchName => ConfigProperties.ContainsKey("teamcity.build.branch") ? ConfigProperties["teamcity.build.branch"] : string.Empty;

        /// <summary>
        /// Gets the vcs branch name.
        /// </summary>
        /// <value>
        /// The vcs branch name.
        /// </value>
        public string VcsBranchName
        {
            get
            {
                var rootBranchKey = ConfigProperties.Keys.FirstOrDefault(k => k.StartsWith("teamcity.build.vcs.branch."));
                return !string.IsNullOrWhiteSpace(rootBranchKey) ? ConfigProperties[rootBranchKey] : string.Empty;
            }
        }

        /// <summary>
        /// Gets the TeamCity build properties.
        /// </summary>
        /// <value>
        /// The TeamCity  build properties as a key/value dictionary.
        /// </value>
        public Dictionary<string, string> BuildProperties => _buildProperties.Value;

        /// <summary>
        /// Gets the TeamCity config properties.
        /// </summary>
        /// <value>
        /// The TeamCity config properties as a key/value dictionary.
        /// </value>
        public Dictionary<string, string> ConfigProperties => _configProperties.Value;

        /// <summary>
        /// Gets the TeamCity runner properties.
        /// </summary>
        /// <value>
        /// The TeamCity runner properties as a key/value dictionary.
        /// </value>
        public Dictionary<string, string> RunnerProperties => _runnerProperties.Value;

        private readonly Lazy<Dictionary<string, string>> _buildProperties;
        private readonly Lazy<Dictionary<string, string>> _configProperties;
        private readonly Lazy<Dictionary<string, string>> _runnerProperties;

        private Dictionary<string, string> ReadAndParseFile(IFileSystem fileSystem, string fileName)
        {
            if (string.IsNullOrWhiteSpace(fileName))
            {
                return new Dictionary<string, string>();
            }

            var configurationPropertiesXmlFile = fileSystem.GetFile($"{fileName}.xml");
            if (!configurationPropertiesXmlFile.Exists)
            {
                return new Dictionary<string, string>();
            }

            var configurationPropertiesXml = XDocument.Load(configurationPropertiesXmlFile.OpenRead());

            return configurationPropertiesXml.XPathSelectElements("//entry")
                .Where(entry => entry.Attribute("key") != null)
                .ToDictionary(entry => entry.Attribute("key").Value, entry => entry.Value);
        }

        private string BuildValueIfExists(string key)
        {
            return BuildProperties.ContainsKey(key) ? BuildProperties[key] : string.Empty;
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCityBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="fileSystem">The file system.</param>
        public TeamCityBuildInfo(ICakeEnvironment environment, IFileSystem fileSystem)
            : base(environment)
        {
            _buildProperties = new Lazy<Dictionary<string, string>>(() => ReadAndParseFile(fileSystem, GetEnvironmentString("TEAMCITY_BUILD_PROPERTIES_FILE")));
            _configProperties = new Lazy<Dictionary<string, string>>(() => ReadAndParseFile(fileSystem, BuildValueIfExists("teamcity.configuration.properties.file")));
            _runnerProperties = new Lazy<Dictionary<string, string>>(() => ReadAndParseFile(fileSystem, BuildValueIfExists("teamcity.runner.properties.file")));
        }
    }
}
