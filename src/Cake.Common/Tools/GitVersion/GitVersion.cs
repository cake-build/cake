namespace Cake.Common.Tools.GitVersion
{
    /// <summary>
    /// GitVersion information
    /// </summary>
    public sealed class GitVersion
    {
        /// <summary>
        /// Gets or sets the major version.
        /// </summary>
        public int Major { get; set; }

        /// <summary>
        /// Gets or sets the minor version.
        /// </summary>
        public int Minor { get; set; }

        /// <summary>
        /// Gets or sets the patch version.
        /// </summary>
        public int Patch { get; set; }

        /// <summary>
        /// Gets or sets the pre-release tag.
        /// </summary>
        public string PreReleaseTag { get; set; }

        /// <summary>
        /// Gets or sets the pre-release tag with dash.
        /// </summary>
        public string PreReleaseTagWithDash { get; set; }

        /// <summary>
        /// Gets or sets the build metadata.
        /// </summary>
        public string BuildMetaData { get; set; }

        /// <summary>
        /// Gets or sets the major version.
        /// </summary>
        public string FullBuildMetaData { get; set; }

        /// <summary>
        /// Gets or sets the major, minor, and path.
        /// </summary>
        public string MajorMinorPatch { get; set; }

        /// <summary>
        /// Gets or sets the Semantic Version.
        /// </summary>
        public string SemVer { get; set; }

        /// <summary>
        /// Gets or sets the legacy Semantic Version.
        /// </summary>
        public string LegacySemVer { get; set; }

        /// <summary>
        /// Gets or sets the padded legacy Semantic Version.
        /// </summary>
        public string LegacySemVerPadded { get; set; }

        /// <summary>
        /// Gets or sets the assembly Semantic Version.
        /// </summary>
        public string AssemblySemVer { get; set; }

        /// <summary>
        /// Gets or sets the full Semantic Version.
        /// </summary>
        public string FullSemVer { get; set; }

        /// <summary>
        /// Gets or sets the informational version.
        /// </summary>
        public string InformationalVersion { get; set; }

        /// <summary>
        /// Gets or sets the branch name.
        /// </summary>
        public string BranchName { get; set; }

        /// <summary>
        /// Gets or sets the git sha.
        /// </summary>
        public string Sha { get; set; }

        /// <summary>
        /// Gets or sets the nuget version for v2.
        /// </summary>
        public string NuGetVersionV2 { get; set; }

        /// <summary>
        /// Gets or sets the nuget version.
        /// </summary>
        public string NuGetVersion { get; set; }
    }
}
