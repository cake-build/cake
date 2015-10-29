using System;
using System.Globalization;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

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
        /// <param name="globber">The globber.</param>
        /// <param name="resolver">The Chocolatey tool resolver</param>
        public ChocolateyPacker(IFileSystem fileSystem, ICakeEnvironment environment,
            IProcessRunner processRunner, ICakeLog log, IGlobber globber,
            IChocolateyToolResolver resolver)
            : base(fileSystem, environment, processRunner, globber, resolver)
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
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
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
        /// Creates a Chocolatey package from the specified Nuspec file.
        /// </summary>
        /// <param name="nuspecFilePath">The nuspec file path.</param>
        /// <param name="settings">The settings.</param>
        public void Pack(FilePath nuspecFilePath, ChocolateyPackSettings settings)
        {
            if (nuspecFilePath == null)
            {
                throw new ArgumentNullException("nuspecFilePath");
            }
            if (settings == null)
            {
                throw new ArgumentNullException("settings");
            }

            Pack(settings, () => _processor.Process(nuspecFilePath, settings));
        }

        private void Pack(ChocolateyPackSettings settings, Func<FilePath> process)
        {
            FilePath procesedNuspecFilePath = null;
            try
            {
                // Transform the nuspec and return the new filename.
                procesedNuspecFilePath = process();

                // Start the process.
                Run(settings, GetArguments(procesedNuspecFilePath, settings));
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

        private ProcessArgumentBuilder GetArguments(FilePath nuspecFilePath, ChocolateyPackSettings settings)
        {
            var builder = new ProcessArgumentBuilder();
            builder.Append("pack");

            // Debug
            if (settings.Debug)
            {
                builder.Append("-d");
            }

            // Verbose
            if (settings.Verbose)
            {
                builder.Append("-v");
            }

            // Always say yes, so as to not show interactive prompt
            builder.Append("-y");

            // Force
            if (settings.Force)
            {
                builder.Append("-f");
            }

            // Noop
            if (settings.Noop)
            {
                builder.Append("--noop");
            }

            // Limit Output
            if (settings.LimitOutput)
            {
                builder.Append("-r");
            }

            // Execution Timeout
            if (settings.ExecutionTimeout != 0)
            {
                builder.Append("--execution-timeout");
                builder.AppendQuoted(settings.ExecutionTimeout.ToString(CultureInfo.InvariantCulture));
            }

            // Cache Location
            if (!string.IsNullOrWhiteSpace(settings.CacheLocation))
            {
                builder.Append("-c");
                builder.AppendQuoted(settings.CacheLocation);
            }

            // Allow Unoffical
            if (settings.AllowUnoffical)
            {
                builder.Append("--allowunofficial");
            }

            // Version
            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                builder.Append("--version");
                builder.AppendQuoted(settings.Version);
            }

            // Nuspec file
            builder.AppendQuoted(nuspecFilePath.MakeAbsolute(_environment).FullPath);

            return builder;
        }
    }
}