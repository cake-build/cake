// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Cake.Common.Build.TeamCity.Data;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.TeamCity
{
    /// <summary>
    /// Responsible for communicating with TeamCity.
    /// </summary>
    public sealed class TeamCityProvider : ITeamCityProvider
    {
        private const string MessagePrefix = "##teamcity[";
        private const string MessagePostfix = "]";

        private static readonly Dictionary<string, string> _sanitizationTokens;

        private readonly ICakeEnvironment _environment;
        private readonly IBuildSystemServiceMessageWriter _writer;

        /// <summary>
        /// Gets a value indicating whether the current build is running on TeamCity.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on TeamCity; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnTeamCity => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TEAMCITY_VERSION"));

        /// <summary>
        /// Gets the TeamCity environment.
        /// </summary>
        /// <value>
        /// The TeamCity environment.
        /// </value>
        /// <para>Via BuildSystem.</para>
        /// <example>
        /// <code>
        /// if (BuildSystem.TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"Environment:
        ///         PullRequest: {0}
        ///         Build Configuration Name: {1}
        ///         TeamCity Project Name: {2}",
        ///         BuildSystem.TeamCity.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.TeamCity.Environment.Build.BuildConfName,
        ///         BuildSystem.TeamCity.Environment.Project.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        /// <para>Via TeamCity.</para>
        /// <example>
        /// <code>
        /// if (TeamCity.IsRunningOnTeamCity)
        /// {
        ///     Information(
        ///         @"Environment:
        ///         PullRequest: {0}
        ///         Build Configuration Name: {1}
        ///         TeamCity Project Name: {2}",
        ///         BuildSystem.TeamCity.Environment.PullRequest.IsPullRequest,
        ///         BuildSystem.TeamCity.Environment.Build.BuildConfName,
        ///         BuildSystem.TeamCity.Environment.Project.Name
        ///         );
        /// }
        /// else
        /// {
        ///     Information("Not running on TeamCity");
        /// }
        /// </code>
        /// </example>
        public TeamCityEnvironmentInfo Environment { get; }

        static TeamCityProvider()
        {
            _sanitizationTokens = new Dictionary<string, string>
            {
                { "|", "||" },
                { "\'", "|\'" },
                { "\n", "|n" },
                { "\r", "|r" },
                { "[", "|[" },
                { "]", "|]" }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCityProvider"/> class.
        /// </summary>
        /// <param name="environment">The cake environment.</param>
        /// <param name="writer">The build system service message writer.</param>
        public TeamCityProvider(ICakeEnvironment environment, IBuildSystemServiceMessageWriter writer)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));

            Environment = new TeamCityEnvironmentInfo(environment);
        }

        /// <summary>
        /// Write a progress message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        public void WriteProgressMessage(string message)
        {
            WriteServiceMessage("progressMessage", message);
        }

        /// <summary>
        /// Write a progressStart message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        public void WriteStartProgress(string message)
        {
            WriteServiceMessage("progressStart", message);
        }

        /// <summary>
        /// Write a progressFinish message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        public void WriteEndProgress(string message)
        {
            WriteServiceMessage("progressFinish", message);
        }

        /// <summary>
        /// Write the start of a message block to the TeamCity build log.
        /// </summary>
        /// <param name="blockName">Block name.</param>
        public void WriteStartBlock(string blockName)
        {
            WriteServiceMessage("blockOpened", "name", blockName);
        }

        /// <summary>
        /// Write the end of a message block to the TeamCity build log.
        /// </summary>
        /// <param name="blockName">Block name.</param>
        public void WriteEndBlock(string blockName)
        {
            WriteServiceMessage("blockClosed", "name", blockName);
        }

        /// <summary>
        /// Write the start of a build block to the TeamCity build log.
        /// </summary>
        /// <param name="compilerName">Build compiler name.</param>
        public void WriteStartBuildBlock(string compilerName)
        {
            WriteServiceMessage("compilationStarted", "compiler", compilerName);
        }

        /// <summary>
        /// Write the end of a build block to the TeamCity build log.
        /// </summary>
        /// <param name="compilerName">Build compiler name.</param>
        public void WriteEndBuildBlock(string compilerName)
        {
            WriteServiceMessage("compilationFinished", "compiler", compilerName);
        }

        /// <summary>
        /// Write a status message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        /// <param name="errorDetails">Error details if status is error.</param>
        public void WriteStatus(string message, string status = "NORMAL", string errorDetails = null)
        {
            var attrs = new Dictionary<string, string>
            {
                { "text", message },
                { "status", status }
            };

            if (errorDetails != null)
            {
                attrs.Add("errorDetails", errorDetails);
            }

            WriteServiceMessage("message", attrs);
        }

        /// <summary>
        /// Tell TeamCity to import data of a given type.
        /// </summary>
        /// <param name="type">Date type.</param>
        /// <param name="path">Data file path.</param>
        public void ImportData(string type, FilePath path)
        {
            if (type == null)
            {
                throw new ArgumentNullException(nameof(type));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            WriteServiceMessage("importData", new Dictionary<string, string>
            {
                { "type", type },
                { "path", path.FullPath }
            });
        }

        /// <summary>
        /// Tell TeamCity to import coverage from dotCover snapshot file.
        /// </summary>
        /// <param name="snapshotFile">Snapshot file path.</param>
        /// <param name="dotCoverHome">The full path to the dotCover home folder to override the bundled dotCover.</param>
        public void ImportDotCoverCoverage(FilePath snapshotFile, DirectoryPath dotCoverHome = null)
        {
            if (snapshotFile == null)
            {
                throw new ArgumentNullException(nameof(snapshotFile));
            }

            var args = dotCoverHome == null ?
                new Dictionary<string, string>() :
                new Dictionary<string, string>
                {
                    { "dotcover_home", dotCoverHome.FullPath }
                };

            WriteServiceMessage("dotNetCoverage", args);

            WriteServiceMessage("importData", new Dictionary<string, string>
            {
                { "type", "dotNetCoverage" },
                { "tool", "dotcover" },
                { "path", snapshotFile.FullPath }
            });
        }

        /// <summary>
        /// Report a build problem to TeamCity.
        /// </summary>
        /// <param name="description">A human-readable plain text describing the build problem. By default, the description appears in the build status text and in the list of build's problems. The text is limited to 4000 symbols, and will be truncated if the limit is exceeded.</param>
        /// <param name="identity">A unique problem ID (optional). Different problems must have different identity, same problems - same identity, which should not change throughout builds if the same problem, for example, the same compilation error occurs. It must be a valid Java ID up to 60 characters. If omitted, the identity is calculated based on the description text.</param>
        public void BuildProblem(string description, string identity = null)
        {
            var tokens = new Dictionary<string, string> { { "description", description } };
            if (!string.IsNullOrEmpty(identity))
            {
                tokens.Add("identity", identity);
            }

            WriteServiceMessage("buildProblem", tokens);
        }

        /// <summary>
        /// Tells TeamCity to publish artifacts in the given directory.
        /// </summary>
        /// <param name="path">Path to artifacts.</param>
        public void PublishArtifacts(string path)
        {
            WriteServiceMessage("publishArtifacts", " ", path);
        }

        /// <summary>
        /// Tells TeamCity to change the current build number.
        /// </summary>
        /// <param name="buildNumber">The required build number.</param>
        public void SetBuildNumber(string buildNumber)
        {
            WriteServiceMessage("buildNumber", buildNumber);
        }

        /// <summary>
        /// Tells TeamCity to set a named parameter with a given value.
        /// </summary>
        /// <param name="parameterName">The name of the parameter to set.</param>
        /// <param name="parameterValue">The value to set for the named parameter.</param>
        public void SetParameter(string parameterName, string parameterValue)
        {
            WriteServiceMessage("setParameter", new Dictionary<string, string>
            {
                { "name", parameterName },
                { "value", parameterValue }
            });
        }

        private void WriteServiceMessage(string messageName, string attributeValue)
        {
            WriteServiceMessage(messageName, new Dictionary<string, string> { { " ", attributeValue } });
        }

        private void WriteServiceMessage(string messageName, string attributeName, string attributeValue)
        {
            WriteServiceMessage(messageName, new Dictionary<string, string> { { attributeName, attributeValue } });
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private void WriteServiceMessage(string messageName, Dictionary<string, string> values)
        {
            var valueString =
                string.Join(" ",
                    values
                        .Select(keypair =>
                        {
                            if (string.IsNullOrWhiteSpace(keypair.Key))
                            {
                                return string.Format(CultureInfo.InvariantCulture, "'{0}'", Sanitize(keypair.Value));
                            }
                            return string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", keypair.Key, Sanitize(keypair.Value));
                        })
                        .ToArray());
            _writer.Write("{0}{1} {2}{3}", MessagePrefix, messageName, valueString, MessagePostfix);
        }

        private static string Sanitize(string source)
        {
            foreach (var charPair in _sanitizationTokens)
            {
                source = source.Replace(charPair.Key, charPair.Value);
            }
            return source;
        }
    }
}
