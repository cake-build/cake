// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.GoCD.Data;
using Cake.Core;

namespace Cake.Common.Build.GoCD
{
    /// <summary>
    /// Responsible for communicating with GoCD.
    /// </summary>
    public sealed class GoCDProvider : IGoCDProvider
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GoCDProvider"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public GoCDProvider(ICakeEnvironment environment)
        {
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            _environment = environment;
            Environment = new GoCDEnvironmentInfo(environment);
        }

         /// <summary>
        /// Gets a value indicating whether the current build is running on Go.CD.
        /// </summary>
        /// <value>
        /// <c>true</c> if the current build is running on Go.CD.; otherwise, <c>false</c>.
        /// </value>
        public bool IsRunningOnGoCD => !string.IsNullOrWhiteSpace(_environment.GetEnvironmentVariable("GO_SERVER_URL"));

        /// <summary>
        /// Gets the Go.CD environment.
        /// </summary>
        /// <value>
        /// The Go.CD environment.
        /// </value>
        public GoCDEnvironmentInfo Environment { get; }
    }
}