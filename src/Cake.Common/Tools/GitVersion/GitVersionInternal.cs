// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Globalization;
using System.Runtime.Serialization;

namespace Cake.Common.Tools.GitVersion
{
    [DataContract(Name = "GitVersion")]
    internal class GitVersionInternal
    {
        private GitVersion _gitVersion;

        internal GitVersion GitVersion => _gitVersion ?? (_gitVersion = new GitVersion());

        [DataMember]
        public string Major
        {
            get => ToString(GitVersion.Major);
            set => GitVersion.Major = ToInt(value);
        }

        [DataMember]
        public string Minor
        {
            get => ToString(GitVersion.Minor);
            set => GitVersion.Minor = ToInt(value);
        }

        [DataMember]
        public string Patch
        {
            get => ToString(GitVersion.Patch);
            set => GitVersion.Patch = ToInt(value);
        }

        [DataMember]
        public string PreReleaseTag
        {
            get => GitVersion.PreReleaseTag;
            set => GitVersion.PreReleaseTag = value;
        }

        [DataMember]
        public string PreReleaseTagWithDash
        {
            get => GitVersion.PreReleaseTagWithDash;
            set => GitVersion.PreReleaseTagWithDash = value;
        }

        [DataMember]
        public string PreReleaseLabel
        {
            get => GitVersion.PreReleaseLabel;
            set => GitVersion.PreReleaseLabel = value;
        }

        [DataMember]
        public string PreReleaseNumber
        {
            get => ToString(GitVersion.PreReleaseNumber);
            set => GitVersion.PreReleaseNumber = ToNullableInt(value);
        }

        [DataMember]
        public string BuildMetaData
        {
            get => GitVersion.BuildMetaData;
            set => GitVersion.BuildMetaData = value;
        }

        [DataMember]
        public string BuildMetaDataPadded
        {
            get => GitVersion.BuildMetaDataPadded;
            set => GitVersion.BuildMetaDataPadded = value;
        }

        [DataMember]
        public string FullBuildMetaData
        {
            get => GitVersion.FullBuildMetaData;
            set => GitVersion.FullBuildMetaData = value;
        }

        [DataMember]
        public string MajorMinorPatch
        {
            get => GitVersion.MajorMinorPatch;
            set => GitVersion.MajorMinorPatch = value;
        }

        [DataMember]
        public string SemVer
        {
            get => GitVersion.SemVer;
            set => GitVersion.SemVer = value;
        }

        [DataMember]
        public string LegacySemVer
        {
            get => GitVersion.LegacySemVer;
            set => GitVersion.LegacySemVer = value;
        }

        [DataMember]
        public string LegacySemVerPadded
        {
            get => GitVersion.LegacySemVerPadded;
            set => GitVersion.LegacySemVerPadded = value;
        }

        [DataMember]
        public string AssemblySemVer
        {
            get => GitVersion.AssemblySemVer;
            set => GitVersion.AssemblySemVer = value;
        }

        [DataMember]
        public string AssemblySemFileVer
        {
            get => GitVersion.AssemblySemFileVer;
            set => GitVersion.AssemblySemFileVer = value;
        }

        [DataMember]
        public string FullSemVer
        {
            get => GitVersion.FullSemVer;
            set => GitVersion.FullSemVer = value;
        }

        [DataMember]
        public string InformationalVersion
        {
            get => GitVersion.InformationalVersion;
            set => GitVersion.InformationalVersion = value;
        }

        [DataMember]
        public string BranchName
        {
            get => GitVersion.BranchName;
            set => GitVersion.BranchName = value;
        }

        [DataMember]
        public string Sha
        {
            get => GitVersion.Sha;
            set => GitVersion.Sha = value;
        }

        [DataMember]
        public string NuGetVersionV2
        {
            get => GitVersion.NuGetVersionV2;
            set => GitVersion.NuGetVersionV2 = value;
        }

        [DataMember]
        public string NuGetVersion
        {
            get => GitVersion.NuGetVersion;
            set => GitVersion.NuGetVersion = value;
        }

        [DataMember]
        public string CommitsSinceVersionSource
        {
            get => ToString(GitVersion.CommitsSinceVersionSource);
            set => GitVersion.CommitsSinceVersionSource = ToNullableInt(value);
        }

        [DataMember]
        public string CommitsSinceVersionSourcePadded
        {
            get => GitVersion.CommitsSinceVersionSourcePadded;
            set => GitVersion.CommitsSinceVersionSourcePadded = value;
        }

        [DataMember]
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