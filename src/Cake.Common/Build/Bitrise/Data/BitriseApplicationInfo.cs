// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Core;

namespace Cake.Common.Build.Bitrise.Data
{
    /// <summary>
    /// Provides Bitrise application information for the current build.
    /// </summary>
    public class BitriseApplicationInfo : BitriseInfo
    {
        /// <summary>
        /// Gets the application title.
        /// </summary>
        /// <value>
        /// The application title.
        /// </value>
        public string ApplicationTitle
        {
            get { return GetEnvironmentString("BITRISE_APP_TITLE"); }
        }

        /// <summary>
        /// Gets the application URL.
        /// </summary>
        /// <value>
        /// The application URL.
        /// </value>
        public string ApplicationUrl
        {
            get { return GetEnvironmentString("BITRISE_APP_URL"); }
        }

        /// <summary>
        /// Gets the application slug.
        /// </summary>
        /// <value>
        /// The application slug.
        /// </value>
        public string AppSlug
        {
            get { return GetEnvironmentString("BITRISE_APP_SLUG"); }
        }

        /// <summary>
        /// Initializes a new instance of the <see cref="BitriseApplicationInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        public BitriseApplicationInfo(ICakeEnvironment environment) : base(environment)
        {
        }
    }
}
