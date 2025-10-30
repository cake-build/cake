// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Tooling;

namespace Cake.Common.Tools.Chocolatey.Pack
{
    /// <summary>
    /// The Chocolatey packer.
    /// </summary>
    public sealed class ChocolateyPacker : ChocolateyTool<ChocolateyPackSettings>
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ChocolateyNuSpecProcessor _processor;

        /// <summary>
        /// Initializes a new instance of the <see cref="ChocolateyPacker"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="processRunner">The process runner.</param>
        /// <param name="log">The log.</param>
        /// <param name="tools">The tool locator.</param>
        /// <param name="resolver">The Chocolatey tool resolver.</param>
        public ChocolateyPacker(
            IFileSystem fileSystem,
            ICakeEnvironment environment,
            IProcessRunner processRunner,
            ICakeLog log,
            IToolLocator tools,
            IChocolateyToolResolver resolver) : base(fileSystem, environment, processRunner, tools, resolver)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _processor = new ChocolateyNuSpecProcessor(_fileSystem, _environment, log);
        }

        /// <summary>
        /// Creates a Chocolatey package from the specified settings.
        /// </summary>
        /// <param name="settings">The settings.</param>
        public void Pack(ChocolateyPackSettings settings)
        {
            ArgumentNullException.ThrowIfNull(settings);

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
        /// Creates a Chocolatey package from the specified Nuspec file.
        /// </summary>
        /// <param name="nuspecFilePath">The nuspec file path.</param>
        /// <param name="settings">The settings.</param>
        public void Pack(FilePath nuspecFilePath, ChocolateyPackSettings settings)
        {
            ArgumentNullException.ThrowIfNull(nuspecFilePath);

            ArgumentNullException.ThrowIfNull(settings);

            Pack(settings, () => _processor.Process(nuspecFilePath, settings));
        }

        private void Pack(ChocolateyPackSettings settings, Func<FilePath> process)
        {
            FilePath processedNuspecFilePath = null;
            try
            {
                // Transform the nuspec and return the new filename.
                processedNuspecFilePath = process();

                // Start the process.
                Run(settings, GetArguments(processedNuspecFilePath, settings), null, null);
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

        private ProcessArgumentBuilder GetArguments(FilePath nuspecFilePath, ChocolateyPackSettings settings)
        {
            const string separator = "=";
            var builder = new ProcessArgumentBuilder();

            builder.Append("pack");

            // Nuspec file
            builder.AppendQuoted(nuspecFilePath.MakeAbsolute(_environment).FullPath);

            // Add common arguments using the inherited method
            AddGlobalArguments(settings, builder);

            // Version
            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                builder.AppendSwitchQuoted("--version", separator, settings.Version);
            }

            // Output Directory
            if (settings.OutputDirectory != null)
            {
                builder.AppendSwitchQuoted("--output-directory", separator, settings.OutputDirectory.MakeAbsolute(_environment).FullPath);
            }

            return builder;
        }
    }
}