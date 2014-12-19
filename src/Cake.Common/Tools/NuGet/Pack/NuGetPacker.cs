using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Utilities;

namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// The NuGet packer.
    /// </summary>
    public sealed class NuGetPacker : Tool<NuGetPackSettings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly NuspecProcessor _processor;
        private readonly IToolResolver _nuGetToolResolver;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPacker"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="log">The log.</param>
        /// <param name="nuGetToolResolver">The NuGet tool resolver</param>
        public NuGetPacker(IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, ICakeLog log,
            IToolResolver nuGetToolResolver)
            : base(fileSystem, environment, processRunner)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _processor = new NuspecProcessor(_fileSystem, _environment, log);
            _nuGetToolResolver = nuGetToolResolver;
        }

        /// <summary>
        /// Creates a NuGet package from the specified Nuspec file.
        /// </summary>
        /// <param name="nuspecFilePath">The nuspec file path.</param>
        /// <param name="settings">The settings.</param>
        public void Pack(FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            if (nuspecFilePath == null)
            {
                throw new ArgumentNullException("nuspecFilePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            FilePath procesedNuspecFilePath = null;
            try
            {
                // Transform the nuspec and return the new filename.
                procesedNuspecFilePath = _processor.Process(nuspecFilePath, settings);

                // Start the process.
                Run(settings, GetArguments(procesedNuspecFilePath, settings), settings.ToolPath);
            }
            finally
            {
                if (procesedNuspecFilePath != null)
                {
                    // Delete the processed file.
                    var file = _fileSystem.GetFile(procesedNuspecFilePath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
            }
        }

        private ProcessArgumentBuilder GetArguments(FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("pack");

            // Version
            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                builder.Append("-Version");
                builder.AppendQuoted(settings.Version);
            }

            // Base path
            if (settings.BasePath != null)
            {
                builder.Append("-BasePath");
                builder.AppendQuoted(settings.BasePath.MakeAbsolute(_environment).FullPath);
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                builder.Append("-OutputDirectory");
                builder.AppendQuoted(settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            // Nuspec file
            builder.AppendQuoted(nuspecFilePath.MakeAbsolute(_environment).FullPath);

            // No package analysis?
            if (settings.NoPackageAnalysis)
            {
                builder.Append("-NoPackageAnalysis");
            }

            // Symbols?
            if (settings.Symbols)
            {
                builder.Append("-Symbols");
            }

            return builder;
        }

        /// <summary>
        /// Gets the name of the tool.
        /// </summary>
        /// <returns>The name of the tool.</returns>
        protected override string GetToolName()
        {
            return _nuGetToolResolver.Name;
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(NuGetPackSettings settings)
        {
            return _nuGetToolResolver.ResolveToolPath();
        }
    }
}
