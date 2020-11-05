// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.Jenkins.Data;
using Cake.Core;

namespace Cake.Common.Build.Jenkins
{
    /// <summary>
    /// Responsible for communicating with Jenkins.
    /// </summary>
    public sealed class JenkinsProvider : IJenkinsProvider
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="JenkinsProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public JenkinsProvider(ICakeEnvironment environment)
        {
            _environment = environment ?? throw new ArgumentNullException(nameof(environment));
            Environment = new JenkinsEnvironmentInfo(_environment);
        }

        /// <inheritdoc/>
        public bool IsRunningOnJenkins => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("JENKINS_URL"));

        /// <inheritdoc/>
        public JenkinsEnvironmentInfo Environment { get; }
    }
}