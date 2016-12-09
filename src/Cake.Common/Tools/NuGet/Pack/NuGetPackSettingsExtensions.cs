using System;
using System.Collections.Generic;

namespace Cake.Common.Tools.NuGet.Pack
{
    public static class NuGetPackSettingsExtensions
    {
        public static NuGetPackSettings WithBasePath (this NuGetPackSettings settings, string basePath)
        {
            var copy = settings.Copy();
            copy.BasePath = basePath;
            return copy;
        }

        public static NuGetPackSettings WithOutputDirectory (this NuGetPackSettings settings, string outputDirectory)
        {
            var copy = settings.Copy();
            copy.OutputDirectory = outputDirectory;
            return copy;
        }

        public static NuGetPackSettings WithNoPackageAnalysis (this NuGetPackSettings settings, bool noPackageAnalysis)
        {
            var copy = settings.Copy();
            copy.NoPackageAnalysis = noPackageAnalysis;
            return copy;
        }

        public static NuGetPackSettings WithIncludeReferencedProjects (this NuGetPackSettings settings, bool includeReferencedProjects)
        {
            var copy = settings.Copy();
            copy.IncludeReferencedProjects = includeReferencedProjects;
            return copy;
        }

        public static NuGetPackSettings WithSymbols (this NuGetPackSettings settings, bool symbols)
        {
            var copy = settings.Copy();
            copy.Symbols = symbols;
            return copy;
        }

        public static NuGetPackSettings WithId (this NuGetPackSettings settings, string id)
        {
            var copy = settings.Copy();
            copy.Id = id;
            return copy;
        }


        public static NuGetPackSettings WithVersion (this NuGetPackSettings settings, string version)
        {
            var copy = settings.Copy();
            copy.Version = version;
            return copy;
        }

        public static NuGetPackSettings WithTitle (this NuGetPackSettings settings, string title)
        {
            var copy = settings.Copy();
            copy.Title = title;
            return copy;
        }

        public static NuGetPackSettings WithAuthors (this NuGetPackSettings settings, ICollection<string> authors)
        {
            var copy = settings.Copy();
            copy.Authors = authors;
            return copy;
        }

        public static NuGetPackSettings WithOwners (this NuGetPackSettings settings, ICollection<string> owners)
        {
            var copy = settings.Copy();
            copy.Owners = owners;
            return copy;
        }

        public static NuGetPackSettings WithDescription (this NuGetPackSettings settings, string description)
        {
            var copy = settings.Copy();
            copy.Description = description;
            return copy;
        }

        public static NuGetPackSettings WithSummary (this NuGetPackSettings settings, string summary)
        {
            var copy = settings.Copy();
            copy.Summary = summary;
            return copy;
        }

        public static NuGetPackSettings WithProjectUrl (this NuGetPackSettings settings, Uri projectUrl)
        {
            var copy = settings.Copy();
            copy.ProjectUrl = projectUrl;
            return copy;
        }

        public static NuGetPackSettings WithIconUrl (this NuGetPackSettings settings, Uri iconUrl)
        {
            var copy = settings.Copy();
            copy.IconUrl = iconUrl;
            return copy;
        }

        public static NuGetPackSettings WithLicenseUrl (this NuGetPackSettings settings, Uri licenseUrl)
        {
            var copy = settings.Copy();
            copy.LicenseUrl = licenseUrl;
            return copy;
        }

        public static NuGetPackSettings WithCopyright (this NuGetPackSettings settings, string copyright)
        {
            var copy = settings.Copy();
            copy.Copyright = copyright;
            return copy;
        }

        public static NuGetPackSettings WithReleaseNotes (this NuGetPackSettings settings, ICollection<string> releaseNotes)
        {
            var copy = settings.Copy();
            copy.ReleaseNotes = releaseNotes;
            return copy;
        }

        public static NuGetPackSettings WithTags (this NuGetPackSettings settings, ICollection<string> tags)
        {
            var copy = settings.Copy();
            copy.Tags = tags;
            return copy;
        }

        public static NuGetPackSettings WithDevelopmentDependency (this NuGetPackSettings settings, bool developmentDependency)
        {
            var copy = settings.Copy();
            copy.DevelopmentDependency = developmentDependency;
            return copy;
        }

        public static NuGetPackSettings WithRequireLicenseAcceptance (this NuGetPackSettings settings, bool requireLicenseAcceptance)
        {
            var copy = settings.Copy();
            copy.RequireLicenseAcceptance = requireLicenseAcceptance;
            return copy;
        }

        public static NuGetPackSettings WithFiles (this NuGetPackSettings settings, ICollection<NuSpecContent> files)
        {
            var copy = settings.Copy();
            copy.Files = files;
            return copy;
        }

        public static NuGetPackSettings WithDependencies (this NuGetPackSettings settings, ICollection<NuSpecDependency> dependencies)
        {
            var copy = settings.Copy();
            copy.Dependencies = dependencies;
            return copy;
        }

        public static NuGetPackSettings WithVerbosity (this NuGetPackSettings settings, NuGetVerbosity? verbosity)
        {
            var copy = settings.Copy();
            copy.Verbosity = verbosity;
            return copy;
        }

        public static NuGetPackSettings WithProperties (this NuGetPackSettings settings, IDictionary<string, string> properties)
        {
            var copy = settings.Copy();
            copy.Properties = properties;
            return copy;
        }

        public static NuGetPackSettings WithMSBuildVersion (this NuGetPackSettings settings, NuGetMSBuildVersion? mSBuildVersion)
        {
            var copy = settings.Copy();
            copy.MSBuildVersion = mSBuildVersion;
            return copy;
        }

        public static NuGetPackSettings WithKeepTemporaryNuSpecFile (this NuGetPackSettings settings, bool keepTemporaryNuSpecFile)
        {
            var copy = settings.Copy();
            copy.KeepTemporaryNuSpecFile = keepTemporaryNuSpecFile;
            return copy;
        }

        private static NuGetPackSettings Copy (this NuGetPackSettings settings)
        {
            return new NuGetPackSettings(
                       settings.BasePath,
                       settings.OutputDirectory,
                       settings.NoPackageAnalysis,
                       settings.IncludeReferencedProjects,
                       settings.Symbols,
                       settings.Id,
                       settings.Version,
                       settings.Title,
                       settings.Authors,
                       settings.Owners,
                       settings.Description,
                       settings.Summary,
                       settings.ProjectUrl,
                       settings.IconUrl,
                       settings.LicenseUrl,
                       settings.Copyright,
                       settings.ReleaseNotes,
                       settings.Tags,
                       settings.DevelopmentDependency,
                       settings.RequireLicenseAcceptance,
                       settings.Files,
                       settings.Dependencies,
                       settings.Verbosity,
                       settings.Properties,
                       settings.MSBuildVersion,
                       settings.KeepTemporaryNuSpecFile);
        }
    }
}