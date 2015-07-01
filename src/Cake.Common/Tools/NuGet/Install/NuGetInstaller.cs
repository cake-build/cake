using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NuGet.Install
{
    /// <summary>
    /// The NuGet package installer used to install NuGet packages.
    /// </summary>
    public sealed class NuGetInstaller : Tool<NuGetInstallSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IToolResolver _nugetToolResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetInstaller"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param> 
        /// <param name="nugetToolResolver">The NuGet tool resolver.</param>
        public NuGetInstaller(IFileSystem fileSystem, ICakeEnvironment environment, 
            IProcessRunner processRunner, IGlobber globber, IToolResolver nugetToolResolver)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
            _nugetToolResolver = nugetToolResolver;
        }

        /// <summary>
        /// Installs NuGet packages using the specified package configuration file and settings.
        /// </summary>
        /// <param name="packageConfigPath">Path to package configuration to use for install.</param>
        /// <param name="settings">The settings.</param>
        public void InstallFromConfig(FilePath packageConfigPath, NuGetInstallSettings settings)
        {
            if (packageConfigPath == null)
            {
                throw new ArgumentNullException("packageConfigPath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            var packageId = packageConfigPath.MakeAbsolute(_environment).FullPath;

            Run(settings, GetArguments(packageId, settings), settings.ToolPath);
        }

        /// <summary>
        /// Installs NuGet packages using the specified package id and settings.
        /// </summary>
        /// <param name="packageId">The source package id.</param>
        /// <param name="settings">The settings.</param>
        public void Install(string packageId, NuGetInstallSettings settings)
        {
            if (string.IsNullOrWhiteSpace(packageId))
            {
                throw new ArgumentNullException("packageId");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(packageId, settings), settings.ToolPath);
        }

        private ProcessArgumentBuilder GetArguments(string packageId, NuGetInstallSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("install");
            builder.AppendQuoted(packageId);

            // Output Directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("-OutputDirectory");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Version
            if (settings.Version != null)
            {
                builder.Append("-Version");
                builder.AppendQuoted(settings.Version);
            }

            // ExcludeVersion?
            if (settings.ExcludeVersion)
            {
                builder.Append("-ExcludeVersion");
            }

            // Prerelease?
            if (settings.Prerelease)
            {
                builder.Append("-Prerelease");
            }

            // RequireConsent?
            if (settings.RequireConsent)
            {
                builder.Append("-RequireConsent");
            }

            // Solution Directory
            if (settings.SolutionDirectory != null)
            {
                builder.Append("-SolutionDirectory");
                builder.AppendQuoted(settings.SolutionDirectory.MakeAbsolute(_environment).FullPath);
            }

            // List of package sources
            if (settings.Source != null && settings.Source.Count > 0)
            {
                builder.Append("-Source");
                builder.AppendQuoted(string.Join(";", settings.Source));
            }

            // No Cache?
            if (settings.NoCache)
            {
                builder.Append("-NoCache");
            }

            // Disable Parallel Processing?
            if (settings.DisableParallelProcessing)
            {
                builder.Append("-DisableParallelProcessing");
            }

            // Verbosity?
            if (settings.Verbosity.HasValue)
            {
                builder.Append("-Verbosity");
                builder.Append(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            // Configuration file
            if (settings.ConfigFile != null)
            {
                builder.Append("-ConfigFile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            builder.Append("-NonInteractive");

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return _nugetToolResolver.Name;
        }

        /// <summary>
        /// Gets the possible names of the tool executable.
        /// </summary>
        /// <returns>The tool executable name.</returns>
        protected override IEnumerable<string> GetToolExecutableNames()
        {
            return new[] { "NuGet.exe", "nuget.exe" };
        }

        /// <summary>
        /// Gets alternative file paths which the tool may exist in
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(NuGetInstallSettings settings)
        {
            var path = _nugetToolResolver.ResolveToolPath();
            if (path != null)
            {
                return new[] { path };
            }

            return Enumerable.Empty<FilePath>();
        }
    }
}
