// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// Best-effort population of legacy GitVersion properties removed in GitVersion 6.
    /// When the tool omits these fields (e.g. GitVersion 6+), they are computed from
    /// the properties that are still present so existing Cake scripts keep working.
    /// </summary>
    internal static class GitVersionLegacyCompat
    {
        private const int DefaultPadding = 4;

        /// <summary>
        /// Populates legacy properties on the given <see cref="GitVersion"/> instance when they are null.
        /// Uses the same formulas as GitVersion 5 where possible (e.g. LegacySemVerPadded, NuGetVersion).
        /// </summary>
        /// <param name="version">The GitVersion instance to fill; not modified if null.</param>
        public static void PopulateLegacyProperties(GitVersion version)
        {
            if (version == null)
            {
                return;
            }

            var majorMinorPatch = version.MajorMinorPatch ?? string.Empty;

            if (string.IsNullOrEmpty(version.CommitsSinceVersionSourcePadded) && version.CommitsSinceVersionSource.HasValue)
            {
                version.CommitsSinceVersionSourcePadded = version.CommitsSinceVersionSource.Value.ToString("D" + DefaultPadding, CultureInfo.InvariantCulture);
            }

            if (string.IsNullOrEmpty(version.BuildMetaDataPadded))
            {
                version.BuildMetaDataPadded = version.CommitsSinceVersionSource.HasValue
                    ? version.CommitsSinceVersionSource.Value.ToString("D" + DefaultPadding, CultureInfo.InvariantCulture)
                    : (version.BuildMetaData ?? string.Empty);
            }

            if (string.IsNullOrEmpty(version.LegacySemVer))
            {
                version.LegacySemVer = GetLegacySemVer(majorMinorPatch, version.PreReleaseLabelWithDash, version.PreReleaseNumber);
            }

            if (string.IsNullOrEmpty(version.LegacySemVerPadded))
            {
                version.LegacySemVerPadded = GetLegacySemVerPadded(majorMinorPatch, version.PreReleaseLabelWithDash, version.PreReleaseNumber);
            }

            if (string.IsNullOrEmpty(version.NuGetVersionV2) || string.IsNullOrEmpty(version.NuGetVersion))
            {
                var nuGetVersion = version.LegacySemVerPadded ?? version.SemVer ?? majorMinorPatch;
                if (string.IsNullOrEmpty(version.NuGetVersionV2))
                {
                    version.NuGetVersionV2 = nuGetVersion.ToLowerInvariant();
                }

                if (string.IsNullOrEmpty(version.NuGetVersion))
                {
                    version.NuGetVersion = version.NuGetVersionV2;
                }
            }

            if (string.IsNullOrEmpty(version.NuGetPreReleaseTagV2) || string.IsNullOrEmpty(version.NuGetPreReleaseTag))
            {
                var nuGetPreRelease = (version.PreReleaseLabelWithDash ?? string.Empty).ToLowerInvariant();
                if (string.IsNullOrEmpty(version.NuGetPreReleaseTagV2))
                {
                    version.NuGetPreReleaseTagV2 = nuGetPreRelease;
                }

                if (string.IsNullOrEmpty(version.NuGetPreReleaseTag))
                {
                    version.NuGetPreReleaseTag = nuGetPreRelease;
                }
            }
        }

        /// <summary>
        /// Legacy semantic version: {MajorMinorPatch}-{PreReleaseLabel}{PreReleaseNumber} (no dot between label and number).
        /// </summary>
        private static string GetLegacySemVer(string majorMinorPatch, string preReleaseLabelWithDash, int? preReleaseNumber)
        {
            if (string.IsNullOrEmpty(majorMinorPatch))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(preReleaseLabelWithDash) || !preReleaseNumber.HasValue)
            {
                return majorMinorPatch;
            }

            return majorMinorPatch + preReleaseLabelWithDash + preReleaseNumber.Value.ToString(CultureInfo.InvariantCulture);
        }

        /// <summary>
        /// Legacy semantic version with padded pre-release number (4 digits).
        /// Format: {MajorMinorPatch}-{PreReleaseLabel}{PreReleaseNumber:D4}.
        /// Example: 6.1.0-alpha0041.
        /// </summary>
        private static string GetLegacySemVerPadded(string majorMinorPatch, string preReleaseLabelWithDash, int? preReleaseNumber)
        {
            if (string.IsNullOrEmpty(majorMinorPatch))
            {
                return string.Empty;
            }

            if (string.IsNullOrEmpty(preReleaseLabelWithDash) || !preReleaseNumber.HasValue)
            {
                return majorMinorPatch;
            }

            return majorMinorPatch + preReleaseLabelWithDash + preReleaseNumber.Value.ToString("D" + DefaultPadding, CultureInfo.InvariantCulture);
        }
    }
}
