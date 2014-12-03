﻿using System;
using System.Linq;
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
        private readonly IGlobber _globber;
        private readonly NuspecProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="NuGetPacker"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="globber">The globber.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="log">The log.</param>
        public NuGetPacker(IFileSystem fileSystem, ICakeEnvironment environment,
            IGlobber globber, IProcessRunner processRunner, ICakeLog log)
            : base(fileSystem, environment, processRunner)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
            _processor = new NuspecProcessor(_fileSystem, _environment, log);    
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
            return "NuGet";
        }

        /// <summary>
        /// Gets the default tool path.
        /// </summary>
        /// <returns>The default tool path.</returns>
        protected override FilePath GetDefaultToolPath(NuGetPackSettings settings)
        {
            const string expression = "./tools/**/NuGet.exe";
            return _globber.GetFiles(expression).FirstOrDefault();
        }
    }
}
