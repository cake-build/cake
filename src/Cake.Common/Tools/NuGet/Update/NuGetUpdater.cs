using System;
using Cake.Core;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;

namespace Cake.Common.Tools.NuGet.Update
{
    /// <summary>
    /// The NuGet package updater.
    /// </summary>
    public sealed class NuGetUpdater : NuGetTool<NuGetUpdateSettings>
    {
        private readonly ICakeEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetUpdater"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="resolver">The nuget tool resolver.</param>
        public NuGetUpdater(IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            IGlobber globber,
            INuGetToolResolver resolver) 
            : base(fileSystem, environment, processRunner, globber, resolver)
        {
            _environment = environment;
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
