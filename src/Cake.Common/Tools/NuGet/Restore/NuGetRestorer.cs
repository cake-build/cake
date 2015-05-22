using System;
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
        /// <param name="nugetToolResolver">The NuGet tool resolver</param>
        public NuGetRestorer(IFileSystem fileSystem, ICakeEnvironment environment, 
            IProcessRunner processRunner, IToolResolver nugetToolResolver)
            : base(fileSystem, environment, processRunner)
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
            var builder = new ProcessArgumentBuilder()
                    .Append("restore")
                    .AppendQuoted(targetFilePath.MakeAbsolute(_environment).FullPath);

            // RequireConsent?
            if (settings.RequireConsent)
            {
                builder.Append("-RequireConsent");
            }

            // Packages Directory
            if (settings.PackagesDirectory != null)
            {
                builder.AppendNamedQuoted("PackagesDirectory", settings.PackagesDirectory.MakeAbsolute(_environment).FullPath);
            }

            // List of package sources
            if (settings.Source != null && settings.Source.Count > 0)
            {
                builder.AppendNamedQuoted("Source", string.Join(";", settings.Source));
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
                builder.AppendNamed("Verbosity", settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            // Configuration file
            if (settings.ConfigFile != null)
            {
                builder.AppendNamedQuoted("ConfigFile", settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            return builder.Append("-NonInteractive");
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
        /// Gets the default tool path.
        /// </summary>
        /// <param name="settings">The settings.</param>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(NuGetRestoreSettings settings)
        {
            return _nugetToolResolver.ResolveToolPath();
        }
    }
}
