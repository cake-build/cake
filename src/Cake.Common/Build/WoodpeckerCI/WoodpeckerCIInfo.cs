// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.WoodpeckerCI
{
    /// <summary>
    /// Base class used to provide information about the WoodpeckerCI environment.
    /// </summary>
    public abstract class WoodpeckerCIInfo
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="WoodpeckerCIInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        protected WoodpeckerCIInfo(ICakeEnvironment environment)
        {
            _environment = environment;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.String"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected string GetEnvironmentString(string variable)
        {
            return _environment.GetEnvironmentVariable(variable) ?? string.Empty;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected int GetEnvironmentInteger(string variable)
        {
            var value = GetEnvironmentString(variable);
            return !string.IsNullOrWhiteSpace(value) && int.TryParse(value, out var result) ? result : 0;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Int64"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected long GetEnvironmentLong(string variable)
        {
            var value = GetEnvironmentString(variable);
            return !string.IsNullOrWhiteSpace(value) && long.TryParse(value, out var result) ? result : 0;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected bool GetEnvironmentBoolean(string variable)
        {
            var value = GetEnvironmentString(variable);
            return !string.IsNullOrWhiteSpace(value) && bool.TryParse(value, out var result) && result;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Uri"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable as a Uri, or null if the value is not a valid absolute URI.</returns>
        protected Uri GetEnvironmentUri(string variable)
        {
            var value = GetEnvironmentString(variable);
            return !string.IsNullOrWhiteSpace(value) && Uri.TryCreate(value, UriKind.Absolute, out var result) ? result : null;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.DateTimeOffset"/> from a Unix timestamp.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable as a DateTimeOffset, or DateTimeOffset.MinValue if the value is not a valid Unix timestamp.</returns>
        protected DateTimeOffset GetEnvironmentDateTimeOffset(string variable)
        {
            var timestamp = GetEnvironmentLong(variable);
            return timestamp > 0 ? DateTimeOffset.FromUnixTimeSeconds(timestamp) : DateTimeOffset.MinValue;
        }
    }
}
