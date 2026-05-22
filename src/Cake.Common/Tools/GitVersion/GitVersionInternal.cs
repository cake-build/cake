// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Text.Json.Serialization;

namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// Internal DTO for deserializing GitVersion JSON output. Property names match GitVersion CLI output (PascalCase).
    /// </summary>
    internal class GitVersionInternal
    {
        private GitVersion _gitVersion;

        internal GitVersion GitVersion => _gitVersion ?? (_gitVersion = new GitVersion());

        [JsonPropertyName("Major")]
        public string Major
        {
            get => ToString(GitVersion.Major);
            set => GitVersion.Major = ToInt(value);
        }

        [JsonPropertyName("Minor")]
        public string Minor
        {
            get => ToString(GitVersion.Minor);
            set => GitVersion.Minor = ToInt(value);
        }

        [JsonPropertyName("Patch")]
        public string Patch
        {
            get => ToString(GitVersion.Patch);
            set => GitVersion.Patch = ToInt(value);
        }

        [JsonPropertyName("PreReleaseTag")]
        public string PreReleaseTag
        {
            get => GitVersion.PreReleaseTag;
            set => GitVersion.PreReleaseTag = value;
        }

        [JsonPropertyName("PreReleaseTagWithDash")]
        public string PreReleaseTagWithDash
        {
            get => GitVersion.PreReleaseTagWithDash;
            set => GitVersion.PreReleaseTagWithDash = value;
        }

        [JsonPropertyName("PreReleaseLabel")]
        public string PreReleaseLabel
        {
            get => GitVersion.PreReleaseLabel;
            set => GitVersion.PreReleaseLabel = value;
        }

        [JsonPropertyName("PreReleaseLabelWithDash")]
        public string PreReleaseLabelWithDash
        {
            get => GitVersion.PreReleaseLabelWithDash;
            set => GitVersion.PreReleaseLabelWithDash = value;
        }

        [JsonPropertyName("PreReleaseNumber")]
        public string PreReleaseNumber
        {
            get => ToString(GitVersion.PreReleaseNumber);
            set => GitVersion.PreReleaseNumber = ToNullableInt(value);
        }

        [JsonPropertyName("WeightedPreReleaseNumber")]
        public string WeightedPreReleaseNumber
        {
            get => ToString(GitVersion.WeightedPreReleaseNumber);
            set => GitVersion.WeightedPreReleaseNumber = ToNullableInt(value);
        }

        [JsonPropertyName("BuildMetaData")]
        public string BuildMetaData
        {
            get => GitVersion.BuildMetaData;
            set => GitVersion.BuildMetaData = value;
        }

        [JsonPropertyName("BuildMetaDataPadded")]
        public string BuildMetaDataPadded
        {
            get => GitVersion.BuildMetaDataPadded;
            set => GitVersion.BuildMetaDataPadded = value;
        }

        [JsonPropertyName("FullBuildMetaData")]
        public string FullBuildMetaData
        {
            get => GitVersion.FullBuildMetaData;
            set => GitVersion.FullBuildMetaData = value;
        }

        [JsonPropertyName("MajorMinorPatch")]
        public string MajorMinorPatch
        {
            get => GitVersion.MajorMinorPatch;
            set => GitVersion.MajorMinorPatch = value;
        }

        [JsonPropertyName("SemVer")]
        public string SemVer
        {
            get => GitVersion.SemVer;
            set => GitVersion.SemVer = value;
        }

        [JsonPropertyName("LegacySemVer")]
        public string LegacySemVer
        {
            get => GitVersion.LegacySemVer;
            set => GitVersion.LegacySemVer = value;
        }

        [JsonPropertyName("LegacySemVerPadded")]
        public string LegacySemVerPadded
        {
            get => GitVersion.LegacySemVerPadded;
            set => GitVersion.LegacySemVerPadded = value;
        }

        [JsonPropertyName("AssemblySemVer")]
        public string AssemblySemVer
        {
            get => GitVersion.AssemblySemVer;
            set => GitVersion.AssemblySemVer = value;
        }

        [JsonPropertyName("AssemblySemFileVer")]
        public string AssemblySemFileVer
        {
            get => GitVersion.AssemblySemFileVer;
            set => GitVersion.AssemblySemFileVer = value;
        }

        [JsonPropertyName("FullSemVer")]
        public string FullSemVer
        {
            get => GitVersion.FullSemVer;
            set => GitVersion.FullSemVer = value;
        }

        [JsonPropertyName("InformationalVersion")]
        public string InformationalVersion
        {
            get => GitVersion.InformationalVersion;
            set => GitVersion.InformationalVersion = value;
        }

        [JsonPropertyName("BranchName")]
        public string BranchName
        {
            get => GitVersion.BranchName;
            set => GitVersion.BranchName = value;
        }

        [JsonPropertyName("EscapedBranchName")]
        public string EscapedBranchName
        {
            get => GitVersion.EscapedBranchName;
            set => GitVersion.EscapedBranchName = value;
        }

        [JsonPropertyName("Sha")]
        public string Sha
        {
            get => GitVersion.Sha;
            set => GitVersion.Sha = value;
        }

        [JsonPropertyName("ShortSha")]
        public string ShortSha
        {
            get => GitVersion.ShortSha;
            set => GitVersion.ShortSha = value;
        }

        [JsonPropertyName("NuGetVersionV2")]
        public string NuGetVersionV2
        {
            get => GitVersion.NuGetVersionV2;
            set => GitVersion.NuGetVersionV2 = value;
        }

        [JsonPropertyName("NuGetVersion")]
        public string NuGetVersion
        {
            get => GitVersion.NuGetVersion;
            set => GitVersion.NuGetVersion = value;
        }

        [JsonPropertyName("NuGetPreReleaseTagV2")]
        public string NuGetPreReleaseTagV2
        {
            get => GitVersion.NuGetPreReleaseTagV2;
            set => GitVersion.NuGetPreReleaseTagV2 = value;
        }

        [JsonPropertyName("NuGetPreReleaseTag")]
        public string NuGetPreReleaseTag
        {
            get => GitVersion.NuGetPreReleaseTag;
            set => GitVersion.NuGetPreReleaseTag = value;
        }

        [JsonPropertyName("VersionSourceSha")]
        public string VersionSourceSha
        {
            get => GitVersion.VersionSourceSha;
            set => GitVersion.VersionSourceSha = value;
        }

        [JsonPropertyName("CommitsSinceVersionSource")]
        public string CommitsSinceVersionSource
        {
            get => ToString(GitVersion.CommitsSinceVersionSource);
            set => GitVersion.CommitsSinceVersionSource = ToNullableInt(value);
        }

        [JsonPropertyName("CommitsSinceVersionSourcePadded")]
        public string CommitsSinceVersionSourcePadded
        {
            get => GitVersion.CommitsSinceVersionSourcePadded;
            set => GitVersion.CommitsSinceVersionSourcePadded = value;
        }

        [JsonPropertyName("UncommittedChanges")]
        public string UncommittedChanges
        {
            get => ToString(GitVersion.UncommittedChanges);
            set => GitVersion.UncommittedChanges = ToNullableInt(value);
        }

        [JsonPropertyName("CommitDate")]
        public string CommitDate
        {
            get => GitVersion.CommitDate;
            set => GitVersion.CommitDate = value;
        }

        private static int? ToNullableInt(string value) => int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture,
            out int numericValue)
            ? numericValue
            : null as int?;

        private static int ToInt(string value) => int.TryParse(value, NumberStyles.Any, CultureInfo.InvariantCulture,
            out int numericValue)
            ? numericValue
            : -1;

        private static string ToString(int value) => value.ToString(CultureInfo.InvariantCulture);

        private static string ToString(int? value) => value.HasValue
            ? ToString(value.Value)
            : null;
    }
}