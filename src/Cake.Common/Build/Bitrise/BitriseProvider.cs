// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.Bitrise.Data;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Build.Bitrise
{
    /// <summary>
    /// Responsible for communicating with Bitrise.
    /// </summary>
    public sealed class BitriseProvider : IBitriseProvider
    {
        private readonly ICakeEnvironment _environment;
        private readonly IProcessRunner _processRunner;

        /// <summary>
        /// Gets a value indicating whether the current build is running on Bitrise.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Bitrise; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnBitrise => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("BITRISE_BUILD_URL"));

        /// <summary>
        /// Gets the Bitrise environment.
        /// </summary>
        /// <value>
        /// The Bamboo environment.
        /// </value>
        public BitriseEnvironmentInfo Environment { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        public BitriseProvider(ICakeEnvironment environment, IProcessRunner processRunner)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            _processRunner = processRunner ?? throw new ArgumentNullException(nameof(processRunner));
            Environment = new BitriseEnvironmentInfo(_environment);
        }

        /// <summary>
        /// Sets and environment variable that can be used in next steps on Bitrise.
        /// </summary>
        /// <param name="variable">The variable.</param>
        /// <param name="value">The value.</param>
        public void SetEnvironmentString(string variable, string value)
        {
            var arguments = new ProcessArgumentBuilder()
                .Append("add")
                .Append("--key")
                .Append(variable)
                .Append("--value")
                .AppendQuoted(value);

            var process = _processRunner.Start("envman", new ProcessSettings
            {
                Arguments = arguments
            });

            process.WaitForExit();
            var exitCode = process.GetExitCode();
            if (exitCode != 0)
            {
                throw new CakeException($"BitriseProvider SetEnvironmentString failed ({exitCode}).");
            }
        }
    }
}
