// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using Cake.Common.Build.TravisCI.Data;
using Cake.Core;
using Cake.Core.Diagnostics;

namespace Cake.Common.Build.TravisCI
{
    /// <summary>
    /// Responsible for communicating with Travis CI.
    /// </summary>
    public sealed class TravisCIProvider : ITravisCIProvider
    {
        private const string MessagePrefix = "travis_";
        private const string MessagePostfix = "\r";
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly TravisCIEnvironmentInfo _environmentInfo;
        private static readonly Dictionary<string, string> _sanitizationTokens;

        static TravisCIProvider()
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
        /// Initializes a new instance of the <see cref="TravisCIProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
        public TravisCIProvider(ICakeEnvironment environment, ICakeLog log)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            _environment = environment;
            _log = log;
            _environmentInfo = new TravisCIEnvironmentInfo(environment);
        }

        /// <summary>
        /// Gets the Travis CI environment.
        /// </summary>
        /// <value>
        /// The environment.
        /// </value>
        public TravisCIEnvironmentInfo Environment
        {
            get { return _environmentInfo; }
        }

        /// <summary>
        /// Gets a value indicating whether this instance is running on Travis CI.
        /// </summary>
        /// <value>
        /// <c>true</c> if this instance is running on Travis CI; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnTravisCI
        {
            get { return !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("TRAVIS")); }
        }

        /// <summary>
        /// Write the start of a message fold to the Travis CI build log.
        /// </summary>
        /// <param name="name">Name of the group.</param>
        public void WriteStartFold(string name)
        {
            WriteServiceMessage("fold", "start", name);
        }

        /// <summary>
        /// Write the start of a message fold to the Travis CI build log.
        /// </summary>
        /// <param name="name">Name of the group.</param>
        public void WriteEndFold(string name)
        {
            WriteServiceMessage("fold", "end", name);
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
                            return string.Format(CultureInfo.InvariantCulture, ":{0}:{1}", keypair.Key, Sanitize(keypair.Value));
                        })
                        .ToArray());
            _log.Write(Verbosity.Quiet, LogLevel.Information, "{0}{1}{2}{3}", MessagePrefix, messageName, valueString, MessagePostfix);
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
