﻿using System;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;

namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// The NuGet packer.
    /// </summary>
    public sealed class NuGetPacker : NuGetTool<NuGetPackSettings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly NuspecProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPacker"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="log">The log.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="resolver">The NuGet tool resolver</param>
        public NuGetPacker(IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, ICakeLog log, IGlobber globber,
            INuGetToolResolver resolver)
            : base(fileSystem, environment, processRunner, globber, resolver)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _processor = new NuspecProcessor(_fileSystem, _environment, log);
        }

        /// <summary>
        /// Creates a NuGet package from the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Pack(NuGetPackSettings settings)
        {
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }
            if (settings.OutputDirectory == null || !_fileSystem.Exist(settings.OutputDirectory))
            {
                throw new CakeException("Required setting OutputDirectory not specified or doesn't exists.");
            }
            if (string.IsNullOrWhiteSpace(settings.Id))
            {
                throw new CakeException("Required setting Id not specified.");
            }
            if (string.IsNullOrWhiteSpace(settings.Version))
            {
                throw new CakeException("Required setting Version not specified.");
            }
            if (settings.Authors == null || settings.Authors.Count == 0)
            {
                throw new CakeException("Required setting Authors not specified.");
            }
            if (string.IsNullOrWhiteSpace(settings.Description))
            {
                throw new CakeException("Required setting Description not specified.");
            }
            if (settings.Files == null || settings.Files.Count == 0)
            {
                throw new CakeException("Required setting Files not specified.");
            }

            Pack(settings, () => _processor.Process(settings));
        }

        /// <summary>
        /// Creates a NuGet package from the specified Nuspec or project file.
        /// </summary>
        /// <param name="filePath">The nuspec or project file path.</param>
        /// <param name="settings">The settings.</param>
        public void Pack(FilePath filePath, NuGetPackSettings settings)
        {
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            string extension = filePath.GetExtension();
            if (extension == ".csproj"
                || extension == ".vbproj"
                || extension == ".fsproj")
            {
                // This is a project file, just run the pack process (no nuspec processing needed)
                Run(settings, GetArguments(filePath, settings));
            }
            else
            {
                Pack(settings, () => _processor.Process(filePath, settings));
            }
        }

        private void Pack(NuGetPackSettings settings, Func<FilePath> process)
        {
            FilePath processedNuspecFilePath = null;
            try
            {
                // Transform the nuspec and return the new filename.
                processedNuspecFilePath = process();

                // Start the process.
                Run(settings, GetArguments(processedNuspecFilePath, settings));
            }
            finally
            {
                if (processedNuspecFilePath != null)
                {
                    // Delete the processed file.
                    var file = _fileSystem.GetFile(processedNuspecFilePath);
                    if (file.Exists)
                    {
                        file.Delete();
                    }
                }
            }
        }

        private ProcessArgumentBuilder GetArguments(FilePath filePath, NuGetPackSettings settings)
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
            builder.AppendQuoted(filePath.MakeAbsolute(_environment).FullPath);

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

            // Verbosity
            if (settings.Verbosity != null)
            {
                builder.Append("-Verbosity");
                builder.Append(settings.Verbosity.Value.ToString().ToLowerInvariant());
            }

            // MSBuildVersion?
            if (settings.MSBuildVersion.HasValue)
            {
                builder.Append("-MSBuildVersion");
                builder.Append(settings.MSBuildVersion.Value.ToString("D"));
            }

            // Properties
            if (settings.Properties != null && settings.Properties.Count > 0)
            {
                if (settings.Properties.Values.Any(string.IsNullOrWhiteSpace))
                {
                    throw new CakeException("Properties values can not be null or empty.");
                }

                if (settings.Properties.Keys.Any(string.IsNullOrWhiteSpace))
                {
                    throw new CakeException("Properties keys can not be null or empty.");
                }
                builder.Append("-Properties");
                builder.Append(string.Join(";",
                    settings.Properties.Select(property => string.Concat(property.Key, "=", property.Value))));
            }

            return builder;
        }
    }
}