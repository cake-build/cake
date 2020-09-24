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
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Configuration:
        ///         Name: {0}",
        ///         BuildSystem.ContinuaCI.Environment.Configuration.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via ContinuaCI.</para>
        /// <example>
        /// <code>
        /// if (ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Configuration:
        ///         Name: {0}",
        ///         ContinuaCI.Environment.Configuration.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        public ContinuaCIConfigurationInfo Configuration { get; }

        /// <summary>
        /// Gets Continua CI project information.
        /// </summary>
        /// <value>
        ///   The Continua CI project information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Project:
        ///         Name: {0}",
        ///         BuildSystem.ContinuaCI.Environment.Project.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via ContinuaCI.</para>
        /// <example>
        /// <code>
        /// if (ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Project:
        ///         Name: {0}",
        ///         ContinuaCI.Environment.Project.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        public ContinuaCIProjectInfo Project { get; }

        /// <summary>
        /// Gets Continua CI build information.
        /// </summary>
        /// <value>
        ///   The Continua CI build information.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Id: {0}
        ///         Version: {1}
        ///         Started By: {2}
        ///         Is Feature Branch Build: {3}
        ///         Build Number: {4}
        ///         Started: {5}",
        ///         BuildSystem.ContinuaCI.Environment.Build.Id,
        ///         BuildSystem.ContinuaCI.Environment.Build.Version,
        ///         BuildSystem.ContinuaCI.Environment.Build.StartedBy,
        ///         BuildSystem.ContinuaCI.Environment.Build.IsFeatureBranchBuild,
        ///         BuildSystem.ContinuaCI.Environment.Build.BuildNumber,
        ///         BuildSystem.ContinuaCI.Environment.Build.Started
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via ContinuaCI.</para>
        /// <example>
        /// <code>
        /// if (ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Build:
        ///         Id: {0}
        ///         Version: {1}
        ///         Started By: {2}
        ///         Is Feature Branch Build: {3}
        ///         Build Number: {4}
        ///         Started: {5}",
        ///         ContinuaCI.Environment.Build.Id,
        ///         ContinuaCI.Environment.Build.Version,
        ///         ContinuaCI.Environment.Build.StartedBy,
        ///         ContinuaCI.Environment.Build.IsFeatureBranchBuild,
        ///         ContinuaCI.Environment.Build.BuildNumber,
        ///         ContinuaCI.Environment.Build.Started
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        public ContinuaCIBuildInfo Build { get; }

        /// <summary>
        /// Gets Continua CI build variables.
        /// </summary>
        /// <value>
        ///   The Continua CI build variables.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Variables:
        ///         {0}",
        ///         BuildSystem.ContinuaCI.Environment.Variable.Aggregate(
        ///         new StringBuilder(),(builder, pair) => builder.AppendLine(
        ///         string.Format(":", pair.Key, pair.Value)),
        ///         builder => builder.ToString())
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via ContinuaCI.</para>
        /// <example>
        /// <code>
        /// if (ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Variables:
        ///         {0}",
        ///         ContinuaCI.Environment.Variable.Aggregate(
        ///         new StringBuilder(),(builder, pair) => builder.AppendLine(
        ///         string.Format(":", pair.Key, pair.Value)),
        ///         builder => builder.ToString())
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        public IDictionary<string, string> Variable
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Variable", Prefix);
                return GetEnvironmentStringDictionary(key);
            }
        }

        /// <summary>
        /// Gets Continua CI build agent properties.
        /// </summary>
        /// <value>
        ///   The Continua CI build agent properties.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Agent Property:
        ///         {0}",
        ///         BuildSystem.ContinuaCI.Environment.AgentProperty.Aggregate(
        ///         new StringBuilder(),(builder, pair) => builder.AppendLine(
        ///         string.Format(":", pair.Key, pair.Value)),
        ///         builder => builder.ToString())
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via ContinuaCI.</para>
        /// <example>
        /// <code>
        /// if (ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Variables:
        ///         {0}",
        ///         ContinuaCI.Environment.AgentProperty.Aggregate(
        ///         new StringBuilder(),(builder, pair) => builder.AppendLine(
        ///         string.Format(":", pair.Key, pair.Value)),
        ///         builder => builder.ToString())
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
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
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Version: {0}",
        ///         BuildSystem.ContinuaCI.Environment.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
        /// <para>Via ContinuaCI.</para>
        /// <example>
        /// <code>
        /// if (ContinuaCI.IsRunningOnContinuaCI)
        /// {
        ///     Information(
        ///         @"Version: {0}",
        ///         ContinuaCI.Environment.Version
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on ContinuaCI");
        /// }
        /// </code>
        /// </example>
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