// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;

namespace Cake.Common.Build.GitLabCI
{
    /// <summary>
    /// Base class used to provide information about the GitLab CI environment.
    /// </summary>
    public abstract class GitLabCIInfo
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GitLabCIInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        protected GitLabCIInfo(ICakeEnvironment environment)
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
        /// Gets an environment variable as a <see cref="System.String"/>.
        /// </summary>
        /// <param name="primaryVariable">The primary environment variable name.</param>
        /// <param name="secondaryVariable">The secondary environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected string GetEnvironmentString(string primaryVariable, string secondaryVariable)
        {
            return !string.IsNullOrEmpty(GetEnvironmentString(primaryVariable)) ? GetEnvironmentString(primaryVariable) : GetEnvironmentString(secondaryVariable);
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected int GetEnvironmentInteger(string variable)
        {
            var value = GetEnvironmentString(variable);
            if (!string.IsNullOrWhiteSpace(value))
            {
                int result;
                if (int.TryParse(value, out result))
                {
                    return result;
                }
            }
            return 0;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Int32"/>.
        /// </summary>
        /// <param name="primaryVariable">The primary environment variable name.</param>
        /// <param name="secondaryVariable">The secondary environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected int GetEnvironmentInteger(string primaryVariable, string secondaryVariable)
        {
            return GetEnvironmentInteger(primaryVariable) != 0 ? GetEnvironmentInteger(primaryVariable) : GetEnvironmentInteger(secondaryVariable);
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected bool GetEnvironmentBoolean(string variable)
        {
            var value = GetEnvironmentString(variable);
            if (!string.IsNullOrWhiteSpace(value))
            {
                return value.Equals("true", StringComparison.OrdinalIgnoreCase);
            }
            return false;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Boolean"/>.
        /// </summary>
        /// <param name="primaryVariable">The primary environment variable name.</param>
        /// <param name="secondaryVariable">The secondary environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected bool GetEnvironmentBoolean(string primaryVariable, string secondaryVariable)
        {
            return GetEnvironmentBoolean(primaryVariable) ? GetEnvironmentBoolean(primaryVariable) : GetEnvironmentBoolean(secondaryVariable);
        }
    }
}