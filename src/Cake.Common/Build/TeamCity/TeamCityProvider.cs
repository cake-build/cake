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
        private readonly IFileSystem _fileSystem;
        private readonly IBuildSystemServiceMessageWriter _writer;

        /// <inheritdoc/>
        public bool IsRunningOnTeamCity => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TEAMCITY_VERSION"));

        /// <inheritdoc/>
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
        /// <param name="fileSystem">The cake file system.</param>
        /// <param name="writer">The build system service message writer.</param>
        public TeamCityProvider(ICakeEnvironment environment, IFileSystem fileSystem, IBuildSystemServiceMessageWriter writer)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _fileSystem = fileSystem ?? throw new ArgumentNullException(nameof(fileSystem));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));

            Environment = new TeamCityEnvironmentInfo(environment, fileSystem);
        }

        /// <inheritdoc/>
        public void WriteProgressMessage(string message)
        {
            WriteServiceMessage("progressMessage", message);
        }

        /// <inheritdoc/>
        public void WriteStartProgress(string message)
        {
            WriteServiceMessage("progressStart", message);
        }

        /// <inheritdoc/>
        public void WriteEndProgress(string message)
        {
            WriteServiceMessage("progressFinish", message);
        }

        /// <inheritdoc/>
        public void WriteStartBlock(string blockName)
        {
            WriteServiceMessage("blockOpened", "name", blockName);
        }

        /// <inheritdoc/>
        public void WriteEndBlock(string blockName)
        {
            WriteServiceMessage("blockClosed", "name", blockName);
        }

        /// <inheritdoc/>
        public void WriteStartBuildBlock(string compilerName)
        {
            WriteServiceMessage("compilationStarted", "compiler", compilerName);
        }

        /// <inheritdoc/>
        public void WriteEndBuildBlock(string compilerName)
        {
            WriteServiceMessage("compilationFinished", "compiler", compilerName);
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void BuildProblem(string description, string identity = null)
        {
            var tokens = new Dictionary<string, string> { { "description", description } };
            if (!string.IsNullOrEmpty(identity))
            {
                tokens.Add("identity", identity);
            }

            WriteServiceMessage("buildProblem", tokens);
        }

        /// <inheritdoc/>
        public void PublishArtifacts(string path)
        {
            WriteServiceMessage("publishArtifacts", " ", path);
        }

        /// <inheritdoc/>
        public void SetBuildNumber(string buildNumber)
        {
            WriteServiceMessage("buildNumber", buildNumber);
        }

        /// <inheritdoc/>
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
