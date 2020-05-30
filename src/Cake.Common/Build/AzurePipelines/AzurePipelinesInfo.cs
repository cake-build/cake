// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.AzurePipelines.Data;
using Cake.Core;

namespace Cake.Common.Build.AzurePipelines
{
    /// <summary>
    /// Base class used to provide information about the Azure Pipelines environment.
    /// </summary>
    public abstract class AzurePipelinesInfo
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="AzurePipelinesInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        protected AzurePipelinesInfo(ICakeEnvironment environment)
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
        /// Gets an environment variable as a <see cref="System.Uri"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected Uri GetEnvironmentUri(string variable)
        {
            var value = GetEnvironmentString(variable);
            Uri uri;
            if (Uri.TryCreate(value, UriKind.Absolute, out uri))
            {
                return uri;
            }
            return null;
        }

        /// <summary>
        /// Gets the current repository type as a <see cref="AzurePipelinesRepositoryType"/> from an environment variable.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The current repository type.</returns>
        protected AzurePipelinesRepositoryType? GetRepositoryType(string variable)
        {
            var value = GetEnvironmentString(variable);
            AzurePipelinesRepositoryType type;
            if (Enum.TryParse(value, true, out type))
            {
                return type;
            }
            return null;
        }
    }
}
