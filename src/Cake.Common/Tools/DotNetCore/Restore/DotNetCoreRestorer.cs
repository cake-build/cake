using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.DotNetCore.Restore
{
    /// <summary>
    /// .NET Core project restorer.
    /// </summary>
    public sealed class DotNetCoreRestorer : DotNetCoreTool<DotNetCoreRestoreSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="DotNetCoreRestorer" /> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        public DotNetCoreRestorer(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber)
            : base(fileSystem, environment, processRunner, globber)
        {
            _environment = environment;
        }

        /// <summary>
        /// Restore the project using the specified path and settings.
        /// </summary>
        /// <param name="root">List of projects and project folders to restore. Each value can be: a path to a project.json or global.json file, or a folder to recursively search for project.json files.</param>
        /// <param name="settings">The settings.</param>
        public void Restore(string root, DotNetCoreRestoreSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Run(settings, GetArguments(root, settings));
        }

        private ProcessArgumentBuilder GetArguments(string root, DotNetCoreRestoreSettings settings)
        {
            var builder = CreateArgumentBuilder(settings);

            builder.Append("restore");

            // Specific root?
            if (root != null)
            {
                builder.Append(root);
            }

            // Output directory
            if (settings.PackagesDirectory != null)
            {
                builder.Append("--packages");
                builder.AppendQuoted(settings.PackagesDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Source
            if (!string.IsNullOrEmpty(settings.Source))
            {
                builder.Append("--source");
                builder.Append(settings.Source);
            }

            // List of fallback package sources
            if (settings.FallbackSources != null && settings.FallbackSources.Count > 0)
            {
                builder.Append("--fallbacksource");
                builder.AppendQuoted(string.Join(";", settings.FallbackSources));
            }

            // Config file
            if (settings.ConfigFile != null)
            {
                builder.Append("--configfile");
                builder.AppendQuoted(settings.ConfigFile.MakeAbsolute(_environment).FullPath);
            }

            // List of runtime identifiers
            if (settings.InferRuntimes != null && settings.InferRuntimes.Count > 0)
            {
                builder.Append("--infer-runtimes");
                builder.AppendQuoted(string.Join(";", settings.InferRuntimes));
            }

            // Quiet
            if (settings.Quiet)
            {
                builder.Append("--quiet");
            }

            // Ignore failed sources
            if (settings.NoCache)
            {
                builder.Append("--no-cache");
            }

            // Disable parallel
            if (settings.DisableParallel)
            {
                builder.Append("--disable-parallel");
            }

            // Ignore failed sources
            if (settings.IgnoreFailedSources)
            {
                builder.Append("--ignore-failed-sources");
            }

            // Force english output
            if (settings.ForceEnglishOutput)
            {
                builder.Append("--force-english-output");
            }

            // Verbosity
            if (settings.Verbosity.HasValue)
            {
                builder.Append("--verbosity");
                builder.Append(settings.Verbosity.ToString());
            }

            return builder;
        }
    }
}
