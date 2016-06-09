// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.Bitrise.Data;
using Cake.Core;

namespace Cake.Common.Build.Bitrise
{
    /// <summary>
    /// Responsible for communicating with Bitrise.
    /// </summary>
    public sealed class BitriseProvider : IBitriseProvider
    {
        private readonly ICakeEnvironment _environment;
        private readonly BitriseEnvironmentInfo _bitriseEnvironment;

        /// <summary>
        /// Gets a value indicating whether the current build is running on Bamboo.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Bamboo; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnBitrise
        {
            get { return !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("BITRISE_BUILD_URL")); }
        }

        /// <summary>
        /// Gets the Bamboo environment.
        /// </summary>
        /// <value>
        /// The Bamboo environment.
        /// </value>
        public BitriseEnvironmentInfo Environment
        {
            get { return _bitriseEnvironment; }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseProvider(ICakeEnvironment environment)
        {
            _environment = environment;
            _bitriseEnvironment = new BitriseEnvironmentInfo(_environment);
        }
    }
}
