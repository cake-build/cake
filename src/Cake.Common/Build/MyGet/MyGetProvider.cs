// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Cake.Core;

namespace Cake.Common.Build.MyGet
{
    /// <summary>
    /// Responsible for communicating with MyGet.
    /// </summary>
    public sealed class MyGetProvider : IMyGetProvider
    {
        private const string MessagePrefix = "##myget[";
        private const string MessagePostfix = "]";
        private static readonly Dictionary<string, string> _sanitizationTokens;
        private readonly ICakeEnvironment _environment;

        static MyGetProvider()
        {
            _sanitizationTokens = new Dictionary<string, string>
            {
                { "|", "||" },
                { "\'", "|\'" },
                { "\n", "|n" },
                { "\r", "|r" },
                { "[", "|['" },
                { "]", "|]" }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyGetProvider"/> class.
        /// </summary>
        /// <param name="environment">The cake environment.</param>
        public MyGetProvider(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            _environment = environment;
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on MyGet.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on MyGet; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnMyGet
        {
            get
            {
                var buildRunner = _environment.GetEnvironmentVariable("BuildRunner");
                return !string.IsNullOrWhiteSpace(buildRunner) && string.Equals("MyGet", buildRunner, StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <summary>
        /// Report a build problem to MyGet.
        /// </summary>
        /// <param name="description">Description of build problem.</param>
        public void BuildProblem(string description)
        {
            WriteServiceMessage("buildProblem", "description", description);
        }

        /// <summary>
        /// Allows setting an environment variable that can be used by a future process.
        /// </summary>
        /// <param name="name">Name of the parameter to set.</param>
        /// <param name="value">Value to assign to the parameter.</param>
        public void SetParameter(string name, string value)
        {
            WriteServiceMessage("setParameter", new Dictionary<string, string>
            {
                { "name", name },
                { "value", value }
            });
        }

        /// <summary>
        /// Write a status message to the MyGet build log.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        /// <param name="errorDetails">Error details if status is error.</param>
        public void WriteStatus(string message, MyGetBuildStatus status, string errorDetails = null)
        {
            var statusToWrite = string.Empty;

            switch (status)
            {
                case MyGetBuildStatus.Failure:
                    statusToWrite = "FAILURE";
                    break;
                case MyGetBuildStatus.Error:
                    statusToWrite = "ERROR";
                    break;
                case MyGetBuildStatus.Warning:
                    statusToWrite = "WARNING";
                    break;
                case MyGetBuildStatus.Normal:
                    statusToWrite = "NORMAL";
                    break;
            }

            var attrs = new Dictionary<string, string>
            {
                { "text", message },
                { "status", statusToWrite }
            };

            if (errorDetails != null)
            {
                attrs.Add("errorDetails", errorDetails);
            }

            WriteServiceMessage("message", attrs);
        }

        /// <summary>
        /// Tells MyGet to change the current build number.
        /// </summary>
        /// <param name="buildNumber">The required build number.</param>
        public void SetBuildNumber(string buildNumber)
        {
            WriteServiceMessage("buildNumber", buildNumber);
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
            foreach (var charPair in _sanitizationTokens)
            {
                source = source.Replace(charPair.Key, charPair.Value);
            }

            return source;
        }
    }
}
