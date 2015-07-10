using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NuGet.Restore
{
    /// <summary>
    /// The NuGet package restorer used to restore solution packages.
    /// </summary>
    public sealed class NuGetRestorer : Tool<NuGetRestoreSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IToolResolver _nugetToolResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetRestorer"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="nugetToolResolver">The NuGet tool resolver</param>
        public NuGetRestorer(IFileSystem fileSystem, ICakeEnvironment environment, 
            IProcessRunner processRunner, IGlobber globber, IToolResolver nugetToolResolver)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
            _nugetToolResolver = nugetToolResolver;
        }

        /// <summary>
        /// Restores NuGet packages using the specified settings.
        /// </summary>
        /// <param name="targetFilePath">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Restore(FilePath targetFilePath, NuGetRestoreSettings settings)
        {
            if (targetFilePath == null)
            {
                throw new ArgumentNullException("targetFilePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(targetFilePath, settings), settings.ToolPath);
        }

        private ProcessArgumentBuilder GetArguments(FilePath targetFilePath, NuGetRestoreSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("restore");
            builder.AppendQuoted(targetFilePath.MakeAbsolute(_environment).FullPath);

            // RequireConsent?
            if (settings.RequireConsent)
            {
                builder.Append("-RequireConsent");
            }

            // Packages Directory
            if (settings.PackagesDirectory != null)
            {
                builder.Append("-PackagesDirectory");
                builder.AppendQuoted(settings.PackagesDirectory.MakeAbsolute(_environment).FullPath);
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
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(NuGetRestoreSettings settings)
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
