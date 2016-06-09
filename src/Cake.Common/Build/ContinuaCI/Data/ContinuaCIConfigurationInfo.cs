// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Globalization;
using Cake.Core;

namespace Cake.Common.Build.ContinuaCI.Data
{
    /// <summary>
    /// Provides Continua CI configuration information for a current build.
    /// </summary>
    public sealed class ContinuaCIConfigurationInfo : ContinuaCIInfo
    {
        private readonly string _prefix;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuaCIConfigurationInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        /// <param name="prefix">The environment variable key prefix.</param>
        public ContinuaCIConfigurationInfo(ICakeEnvironment environment, string prefix)
            : base(environment)
        {
            _prefix = prefix;
        }

        /// <summary>
        /// Gets the Continua CI Configuration Name
        /// </summary>
        /// <value>
        ///   The Continua CI Configuration Name.
        /// </value>
        public string Name
        {
            get
            {
                var key = string.Format(CultureInfo.InvariantCulture, "{0}.Name", _prefix);
                return GetEnvironmentString(key);
            }
        }
    }
}
