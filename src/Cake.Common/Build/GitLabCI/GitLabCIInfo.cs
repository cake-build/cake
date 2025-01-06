// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Linq;
using System.Reflection;
using System.Runtime.Serialization;
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

        /// <summary>
        /// Gets an environment variable as an enum.
        /// </summary>
        /// <remarks>
        /// By default, the environment variable value is presumed to be identical to the enum value name.
        /// To define a mapping between environment variable value and enum name, apply the <see cref="EnumMemberAttribute"/> attribue in the enum definition.
        /// <para>
        /// Parsing is case-insensitive.
        /// </para>
        /// </remarks>
        /// <param name="variable">The primary environment variable name.</param>
        /// <typeparam name="TEnum">The type of the enum to return.</typeparam>
        /// <returns>
        /// The environment variable value converted to the corresponding value of <typeparamref name="TEnum"/> or
        /// <c>null</c> if the variable is not set or the value could not be converted to the the specified enum type.
        /// </returns>
        protected TEnum? GetEnvironmentEnum<TEnum>(string variable) where TEnum : struct, Enum
        {
            var value = GetEnvironmentString(variable);
            if (!string.IsNullOrWhiteSpace(value))
            {
                // Instead of using Enum.TryParse(), enumerate the enum fields using reflection.
                // This defining enums where the environment variable value differs from the name of the corresponding enum member:
                // - If the enum member has a [EnumMember] attribute, use the value defined by the attribute instead of the enum member name
                // - Otherwise, use the enum member name (matching the behavior of Enum.TryParse())
                foreach (var field in typeof(TEnum).GetFields().Where(fi => fi.FieldType == typeof(TEnum)))
                {
                    var enumMemberName = field.Name;
                    if (field.GetCustomAttribute<EnumMemberAttribute>() is { Value: { } customName } && !string.IsNullOrEmpty(customName))
                    {
                        enumMemberName = customName;
                    }

                    if (StringComparer.OrdinalIgnoreCase.Equals(enumMemberName, value))
                    {
                        return (TEnum?)field.GetValue(null);
                    }
                }
            }

            return null;
        }
    }
}