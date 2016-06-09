// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using Cake.Core;

namespace Cake.Common.Build.ContinuaCI
{
    /// <summary>
    /// Base class used to provide information about the Continua CI environment.
    /// </summary>
    public abstract class ContinuaCIInfo
    {
        private readonly ICakeEnvironment _environment;
        private IDictionary<string, string> _allEnvironmentVariables;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContinuaCIInfo"/> class.
        /// </summary>
        /// <param name="environment">The environment.</param>
        protected ContinuaCIInfo(ICakeEnvironment environment)
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
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            int result;
            if (int.TryParse(value, out result))
            {
                return result;
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
            if (string.IsNullOrWhiteSpace(value))
            {
                return false;
            }

            return value.Equals("true", StringComparison.OrdinalIgnoreCase);
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.DateTime"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected DateTime? GetEnvironmentDateTime(string variable)
        {
            var value = GetEnvironmentString(variable);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            DateTime result;
            if (DateTime.TryParse(value, out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.Int64"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected long GetEnvironmentLong(string variable)
        {
            var value = GetEnvironmentString(variable);
            if (string.IsNullOrWhiteSpace(value))
            {
                return 0;
            }

            long result;
            if (long.TryParse(value, out result))
            {
                return result;
            }

            return 0;
        }

        /// <summary>
        /// Gets an environment variable as a <see cref="System.TimeSpan"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable.</returns>
        protected TimeSpan? GetEnvironmentTimeSpan(string variable)
        {
            var value = GetEnvironmentString(variable);
            if (string.IsNullOrWhiteSpace(value))
            {
                return null;
            }

            TimeSpan result;
            if (TimeSpan.TryParse(value, out result))
            {
                return result;
            }

            return null;
        }

        /// <summary>
        /// Gets an environment variable as an array of <see cref="System.String"/>.
        /// </summary>
        /// <param name="variable">The environment variable name.</param>
        /// <returns>The environment variable value split by comma into an array of values.</returns>
        protected IEnumerable<string> GetEnvironmentStringList(string variable)
        {
            var value = _environment.GetEnvironmentVariable(variable);
            return string.IsNullOrWhiteSpace(value)
                ? Enumerable.Empty<string>()
                : value.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
        }

        /// <summary>
        /// Gets matching list of environment variables as an dictionary of <see cref="System.String"/>.
        /// </summary>
        /// <param name="variablePrefix">The prefix for the environment variables name.</param>
        /// <returns>A dictionary of environment variables starting with variablePrefix</returns>
        protected IDictionary<string, string> GetEnvironmentStringDictionary(string variablePrefix)
        {
            if (_allEnvironmentVariables == null)
            {
                _allEnvironmentVariables = _environment.GetEnvironmentVariables();
            }

            var startsWith = string.Format(CultureInfo.InvariantCulture, "{0}.", variablePrefix);
            var matchingVariables = _allEnvironmentVariables.Where(v => v.Key.StartsWith(startsWith)).ToDictionary(p => p.Key.Substring(startsWith.Length), p => p.Value);

            return matchingVariables;
        }
    }
}
