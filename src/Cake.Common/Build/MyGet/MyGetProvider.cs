// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Diagnostics.CodeAnalysis;
using System.Globalization;
using System.Linq;
using System.Text;
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

        private static readonly Dictionary<char, string> _sanitizationTokens;
        private static readonly char[] _specialCharacters;

        private readonly ICakeEnvironment _environment;
        private readonly IBuildSystemServiceMessageWriter _writer;

        static MyGetProvider()
        {
            _sanitizationTokens = new Dictionary<char, string>
            {
                { '|', "||" },
                { '\'', "|\'" },
                { '\n', "|n" },
                { '\r', "|r" },
                { '[', "|[" },
                { ']', "|]" }
            };

            _specialCharacters = _sanitizationTokens.Keys.ToArray();
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="MyGetProvider"/> class.
        /// </summary>
        /// <param name="environment">The cake environment.</param>
        /// <param name="writer">The build system service message writer.</param>
        public MyGetProvider(ICakeEnvironment environment, IBuildSystemServiceMessageWriter writer)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _writer = writer ?? throw new ArgumentNullException(nameof(writer));
        }

        /// <inheritdoc/>
        public bool IsRunningOnMyGet
        {
            get
            {
                var buildRunner = _environment.GetEnvironmentVariable("BuildRunner");
                return !string.IsNullOrWhiteSpace(buildRunner) && string.Equals("MyGet", buildRunner, StringComparison.OrdinalIgnoreCase);
            }
        }

        /// <inheritdoc/>
        public void BuildProblem(string description)
        {
            WriteServiceMessage("buildProblem", "description", description);
        }

        /// <inheritdoc/>
        public void SetParameter(string name, string value)
        {
            WriteServiceMessage("setParameter", new Dictionary<string, string>
            {
                { "name", name },
                { "value", value }
            });
        }

        /// <inheritdoc/>
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

        /// <inheritdoc/>
        public void SetBuildNumber(string buildNumber)
        {
            WriteServiceMessage("buildNumber", buildNumber);
        }

        private void WriteServiceMessage(string messageName, string attributeValue)
        {
            WriteServiceMessage(messageName, new Dictionary<string, string> { { string.Empty, attributeValue } });
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
                            var sanitizedValue = Sanitize(keypair.Value);
                            if (string.IsNullOrEmpty(keypair.Key))
                            {
                                return string.Format(CultureInfo.InvariantCulture, "'{0}'", sanitizedValue);
                            }
                            return string.Format(CultureInfo.InvariantCulture, "{0}='{1}'", keypair.Key, sanitizedValue);
                        })
                        .ToArray());

            _writer.Write("{0}{1} {2}{3}", MessagePrefix, messageName, valueString, MessagePostfix);
        }

        private static string Sanitize(string source)
        {
            if (string.IsNullOrEmpty(source))
            {
                return string.Empty;
            }

            // When source does not contain special characters, then it is possible to skip building new string.
            // This approach can possibly iterate through string 2 times, but special characters are exceptional.
            if (source.IndexOfAny(_specialCharacters) < 0)
            {
                return source;
            }

            var stringBuilder = new StringBuilder(source.Length * 2);
            for (int index = 0; index < source.Length; index++)
            {
                char sourceChar = source[index];
                if (_sanitizationTokens.TryGetValue(sourceChar, out var replacement))
                {
                    stringBuilder.Append(replacement);
                }
                else
                {
                    stringBuilder.Append(sourceChar);
                }
            }

            return stringBuilder.ToString();
        }
    }
}