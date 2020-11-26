// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using Cake.Core;

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
        /// Initializes a new instance of the <see cref="TeamCityBuildInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public TeamCityBuildInfo(ICakeEnvironment environment)
            : base(environment)
        {
        }
    }
}