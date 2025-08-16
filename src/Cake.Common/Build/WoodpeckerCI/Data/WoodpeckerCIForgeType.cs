// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;

namespace Cake.Common.Build.WoodpeckerCI.Data
{
    /// <summary>
    /// Represents the type of forge used by WoodpeckerCI.
    /// </summary>
    public enum WoodpeckerCIForgeType
    {
        /// <summary>
        /// Unknown forge type.
        /// </summary>
        Unknown = 0,

        /// <summary>
        /// Bitbucket forge.
        /// </summary>
        Bitbucket = 1,

        /// <summary>
        /// Bitbucket Data Center forge.
        /// </summary>
        BitbucketDC = 2,

        /// <summary>
        /// Forgejo forge.
        /// </summary>
        Forgejo = 3,

        /// <summary>
        /// Gitea forge.
        /// </summary>
        Gitea = 4,

        /// <summary>
        /// GitHub forge.
        /// </summary>
        GitHub = 5,

        /// <summary>
        /// GitLab forge.
        /// </summary>
        GitLab = 6
    }

    /// <summary>
    /// Extension methods for <see cref="WoodpeckerCIForgeType"/>.
    /// </summary>
    public static class WoodpeckerCIForgeTypeExtensions
    {
        /// <summary>
        /// Parses a string value to a <see cref="WoodpeckerCIForgeType"/> enum value.
        /// </summary>
        /// <param name="value">The string value to parse.</param>
        /// <returns>The parsed enum value, or <see cref="WoodpeckerCIForgeType.Unknown"/> if parsing fails.</returns>
        public static WoodpeckerCIForgeType ParseForgeType(string value)
        {
            if (string.IsNullOrWhiteSpace(value))
            {
                return WoodpeckerCIForgeType.Unknown;
            }

            // Try to parse case-insensitively
            if (Enum.TryParse<WoodpeckerCIForgeType>(value, true, out var result))
            {
                return result;
            }

            // Handle special cases for known values that don't match enum names exactly
            var normalizedValue = value.ToLowerInvariant();
            return normalizedValue switch
            {
                "bitbucket" => WoodpeckerCIForgeType.Bitbucket,
                "bitbucket_dc" => WoodpeckerCIForgeType.BitbucketDC,
                "forgejo" => WoodpeckerCIForgeType.Forgejo,
                "gitea" => WoodpeckerCIForgeType.Gitea,
                "github" => WoodpeckerCIForgeType.GitHub,
                "gitlab" => WoodpeckerCIForgeType.GitLab,
                _ => WoodpeckerCIForgeType.Unknown
            };
        }
    }
}
