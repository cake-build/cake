using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
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
        private static readonly Dictionary<string, string> SanitizationTokens =
            new Dictionary<string, string>
            {
                { "|", "||" },
                { "\'", "|\'" },
                { "\n", "|n" },
                { "\r", "|r" },
                { "[", "|['" },
                { "]", "|]" }
            };

        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeamCityProvider"/> class.
        /// </summary>
        /// <param name="environment">The cake environment.</param>
        public TeamCityProvider(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            _environment = environment;
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on TeamCity.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on TeamCity; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnTeamCity
        {
            get { return !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TEAMCITY_VERSION")); }
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
            WriteServiceMessage("progressStart", "message", message);
        }

        /// <summary>
        /// Write a progressFinish message to the TeamCity build log.
        /// </summary>
        /// <param name="message">Build log message.</param>
        public void WriteEndProgress(string message)
        {
            WriteServiceMessage("progressFinish", "message", message);
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
                throw new ArgumentNullException("type");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            WriteServiceMessage("importData", new Dictionary<string, string>
            {
                { "type", type },
                { "path", path.FullPath }
            });
        }

        /// <summary>
        /// Report a build problem to TeamCity.
        /// </summary>
        /// <param name="description">Description of build problem.</param>
        /// <param name="identity">Build identity.</param>
        public void BuildProblem(string description, string identity)
        {
            WriteServiceMessage("buildProblem", new Dictionary<string, string>
            {
                { "description", description },
                { "identity", identity }
            });
        }

        /// <summary>
        /// Tells TeamCity to publish artifacts in the given directory.
        /// </summary>
        /// <param name="path">Path to artifacts.</param>
        public void PublishArtifacts(string path)
        {
            WriteServiceMessage("publishArtifacts", " ", path);
        }

        private static void WriteServiceMessage(string messageName, string attributeValue)
        {
            WriteServiceMessage(messageName, new Dictionary<string, string> { { " ", attributeValue } });
        }

        private static void WriteServiceMessage(string messageName, string attributeName, string attributeValue)
        {
            WriteServiceMessage(messageName, new Dictionary<string, string> { { attributeName, attributeValue } });
        }

        [SuppressMessage("StyleCop.CSharp.ReadabilityRules", "SA1118:ParameterMustNotSpanMultipleLines", Justification = "Reviewed.")]
        private static void WriteServiceMessage(string messageName, Dictionary<string, string> values)
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
            Console.WriteLine("{0}{1} {2}{3}", MessagePrefix, messageName, valueString, MessagePostfix);
        }

        private static string Sanitize(string source)
        {
            foreach (var charPair in SanitizationTokens)
            {
                source = source.Replace(charPair.Key, charPair.Value);
            }
            return source;
        }
    }
}
