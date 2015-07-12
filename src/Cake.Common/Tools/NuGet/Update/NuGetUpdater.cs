using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NuGet.Update
{
    /// <summary>
    /// The NuGet package updater.
    /// </summary>
    public sealed class NuGetUpdater : Tool<NuGetUpdateSettings>
    {
        private readonly ICakeEnvironment _environment;
        private readonly IToolResolver _nugetToolResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetUpdater"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="nugetToolResolver">The nuget tool resolver.</param>
        public NuGetUpdater(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber,
            IToolResolver nugetToolResolver) : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
            _nugetToolResolver = nugetToolResolver;
        }

        /// <summary>
        /// Restores NuGet packages using the specified settings.
        /// </summary>
        /// <param name="targetFile">The target file path.</param>
        /// <param name="settings">The settings.</param>
        public void Update(FilePath targetFile, NuGetUpdateSettings settings)
        {
            if (targetFile == null)
            {
                throw new ArgumentNullException("targetFile");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            Run(settings, GetArguments(targetFile, settings), settings.ToolPath);
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
        protected override IEnumerable<FilePath> GetAlternativeToolPaths(NuGetUpdateSettings settings)
        {
            var path = _nugetToolResolver.ResolveToolPath();
            if (path != null)
            {
                return new[] { path };
            }

            return Enumerable.Empty<FilePath>();
        }

        private ProcessArgumentBuilder GetArguments(FilePath targetFile, NuGetUpdateSettings settings)
        {
            var builder = new ProcessArgumentBuilder();

            builder.Append("update");
            builder.AppendQuoted(targetFile.MakeAbsolute(_environment).FullPath);

            // Packages?
            if (settings.Id != null && settings.Id.Count > 0)
            {
                builder.Append("-Id");
                builder.AppendQuoted(string.Join(";", settings.Id));
            }

            // List of package sources
            if (settings.Source != null && settings.Source.Count > 0)
            {
                builder.Append("-Source");
                builder.AppendQuoted(string.Join(";", settings.Source));
            }

            // Verbosity?
            if (settings.Verbosity.HasValue)
            {
                builder.Append("-Verbosity");
                builder.Append(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            // Safe?
            if (settings.Safe)
            {
                builder.Append("-Safe");
            }

            // Prerelease?
            if (settings.Prerelease)
            {
                builder.Append("-Prerelease");
            }

            builder.Append("-NonInteractive");

            return builder;
        }
    }
}
