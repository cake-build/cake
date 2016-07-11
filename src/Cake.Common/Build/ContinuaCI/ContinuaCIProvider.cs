// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Common.Build.ContinuaCI.Data;
using Cake.Core;

namespace Cake.Common.Build.ContinuaCI
{
    /// <summary>
    /// Responsible for communicating with Continua CI.
    /// </summary>
    public sealed class ContinuaCIProvider : IContinuaCIProvider
    {
        private const string MessagePrefix = "@@continua[";
        private const string MessagePostfix = "]";
        private static readonly Dictionary<string, string> _sanitizationTokens;
        private readonly ICakeEnvironment _environment;
        private readonly ContinuaCIEnvironmentInfo _environmentInfo;

        static ContinuaCIProvider()
        {
            _sanitizationTokens = new Dictionary<string, string>
            {
                { "\\", "\\\\" },
                { "'", "\\'" },
                { "\n", "\\n" },
                { "\r", "\\r" },
                { "[", "\\['" },
                { "]", "\\]" }
            };
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuaCIProvider"/> class.
        /// </summary>
        /// <param name="environment">The cake environment.</param>
        public ContinuaCIProvider(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }

            _environment = environment;
            _environmentInfo = new ContinuaCIEnvironmentInfo(environment);
        }

        /// <summary>
        /// Gets a value indicating whether the current build is running on Continua CI.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Continua CI; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnContinuaCI
        {
            get
            {
                var continuaCIVersionVariable = _environment.GetEnvironmentVariable("ContinuaCI.Version");
                return !string.IsNullOrWhiteSpace(continuaCIVersionVariable);
            }
        }

        /// <summary>
        /// Gets the Continua CI environment.
        /// </summary>
        /// <value>
        /// The Continua CI environment.
        /// </value>
        public ContinuaCIEnvironmentInfo Environment
        {
            get { return _environmentInfo; }
        }

        /// <summary>
        /// Write a status message to the Continua CI build log.
        /// </summary>
        /// <param name="message">Message contents.</param>
        /// <param name="status">Build status.</param>
        public void WriteMessage(string message, ContinuaCIMessageType status)
        {
            var name = Enum.GetName(typeof(ContinuaCIMessageType), status);
            if (name != null)
            {
                var statusToWrite = name.ToLower();
                var attrs = new Dictionary<string, string>
                {
                    { "text", message },
                    { "status", statusToWrite }
                };
                WriteServiceMessage("message", attrs);
            }
        }

        /// <summary>
        /// Write the start of a message group to the Continua CI build log.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        public void WriteStartGroup(string groupName)
        {
            WriteServiceMessage("startGroup ", "name", groupName);
        }

        /// <summary>
        /// Write the end of a message block to the Continua CI build log.
        /// </summary>
        /// <param name="groupName">Group name.</param>
        public void WriteEndBlock(string groupName)
        {
            WriteServiceMessage("endGroup", "name", groupName);
        }

        /// <summary>
        /// Set a Continua CI build variable.
        /// </summary>
        /// <param name="name">Name of the variable to set.</param>
        /// <param name="value">Value to assign to the variable.</param>
        /// <param name="skipIfNotDefined">Set to 'true' to prevent the build failing if the variable has not been defined for the configuration..</param>
        public void SetVariable(string name, string value, bool skipIfNotDefined = true)
        {
            WriteServiceMessage("setParameter", new Dictionary<string, string>
            {
                { "name", name },
                { "value", value },
                { "skipIfNotDefined", skipIfNotDefined.ToString() }
            });
        }

        /// <summary>
        /// Set a Continua CI build version.
        /// </summary>
        /// <param name="version">The new build version.</param>
        public void SetBuildVersion(string version)
        {
            WriteServiceMessage("setBuildVersion", "value", version);
        }

        /// <summary>
        /// Set a Continua CI build status message, which is shown on the build details page when a build is running.
        /// </summary>
        /// <param name="text">The new build status text.</param>
        public void SetBuildStatus(string text)
        {
            WriteServiceMessage("setBuildStatus", "value", text);
        }

        private static void WriteServiceMessage(string messageName, string attributeName, string attributeValue)
        {
            WriteServiceMessage(messageName, new Dictionary<string, string> { { attributeName, attributeValue } });
        }

        private static void WriteServiceMessage(string messageName, Dictionary<string, string> parameters)
        {
            var arrayOfParameters = parameters.Select(keypair =>
                                                {
                                                    if (string.IsNullOrWhiteSpace(keypair.Key))
                                                    {
                                                        return string.Format(CultureInfo.InvariantCulture, "'{0}'", Sanitize(keypair.Value));
                                                    }
                                                    return string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", keypair.Key, Sanitize(keypair.Value));
                                                }).ToArray();

            var allParameters = string.Join(" ", arrayOfParameters);

            Console.WriteLine("{0}{1} {2}{3}", MessagePrefix, messageName, allParameters, MessagePostfix);
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
