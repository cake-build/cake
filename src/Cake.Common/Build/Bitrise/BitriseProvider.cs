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

        /// <inheritdoc/>
        public bool IsRunningOnBitrise => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("BITRISE_BUILD_URL"));

        /// <inheritdoc/>
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

        /// <inheritdoc/>
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
