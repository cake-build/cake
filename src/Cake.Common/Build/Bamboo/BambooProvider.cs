// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Common.Build.Bamboo.Data;
using Cake.Core;

namespace Cake.Common.Build.Bamboo
{
    /// <summary>
    /// Responsible for communicating with Bamboo.
    /// </summary>
    public sealed class BambooProvider : IBambooProvider
    {
        private readonly ICakeEnvironment _environment;
        private readonly BambooEnvironmentInfo _environmentInfo;

        /// <summary>
        /// Initializes a new instance of the <see cref="BambooProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BambooProvider(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            _environment = environment;
            _environmentInfo = new BambooEnvironmentInfo(environment);
        }

         /// <summary>
        /// Gets a value indicating whether the current build is running on Bamboo.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Bamboo.; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnBamboo
        {
            get { return !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("bamboo_buildNumber")); }
        }

        /// <summary>
        /// Gets the Bamboo environment.
        /// </summary>
        /// <value>
        /// The Bamboo environment.
        /// </value>
        public BambooEnvironmentInfo Environment
        {
            get { return _environmentInfo; }
        }
    }
}
