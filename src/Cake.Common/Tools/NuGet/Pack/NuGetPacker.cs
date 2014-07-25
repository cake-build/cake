using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Xml;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.Pack
{
    /// <summary>
    /// The NuGet packer.
    /// </summary>
    public sealed class NuGetPacker
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly IGlobber _globber;
        private readonly IProcessRunner _processRunner;
        private readonly ICakeLog _log;

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
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _globber = globber;
            _processRunner = processRunner;
            _log = log;
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

            // Find the NuGet executable.
            var toolPath = NuGetResolver.GetToolPath(_environment, _globber, settings.ToolPath);

            FilePath procesedNuspecFilePath = null;
            try
            {
                // Transform the nuspec and return the new filename.
                procesedNuspecFilePath = Process(nuspecFilePath, settings);

                // Start the process.
                var processInfo = GetProcessStartInfo(toolPath, procesedNuspecFilePath, settings);
                var process = _processRunner.Start(processInfo);
                if (process == null)
                {
                    throw new CakeException("NuGet.exe was not started.");
                }

                // Wait for the process to exit.
                process.WaitForExit();

                // Did an error occur?
                if (process.GetExitCode() != 0)
                {
                    throw new CakeException("NuGet packager failed.");
                }
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

        private ProcessStartInfo GetProcessStartInfo(FilePath toolPath, FilePath nuspacFilePath, NuGetPackSettings settings)
        {
            return NuGetResolver.GetProcessStartInfo(_environment, toolPath,
                () => GetPackParameters(nuspacFilePath, settings));
        }

        private FilePath Process(FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            // Make the nuspec file path absolute.
            nuspecFilePath = nuspecFilePath.MakeAbsolute(_environment);

            // Make sure the nuspec file exist.
            var nuspecFile = _fileSystem.GetFile(nuspecFilePath);
            if (!nuspecFile.Exists)
            {
                const string format = "Could not find nuspec file '{0}'.";
                throw new CakeException(string.Format(format, nuspecFilePath.FullPath));
            }

            // Load the content of the nuspec file.
            _log.Debug("Parsing nuspec...");
            var xml = LoadNuspecXml(nuspecFile);

            // Process the XML.
            _log.Debug("Transforming nuspec...");
            NuspecTransformer.Transform(xml, settings);

            // Return the file of the new nuspec.
            _log.Debug("Writing temporary nuspec...");
            return SaveNuspecXml(nuspecFilePath, settings, xml);
        }

        private static XmlDocument LoadNuspecXml(IFile nuspecFile)
        {
            using (var stream = nuspecFile.OpenRead())
            {
                var document = new XmlDocument();
                document.Load(stream);
                return document;
            }
        }

        private FilePath SaveNuspecXml(FilePath nuspecFilePath, NuGetPackSettings settings, XmlDocument document)
        {
            // Get the new nuspec path.
            var filename = nuspecFilePath.GetFilename();
            filename = filename.ChangeExtension("temp.nuspec");
            var outputDirectory = GetOutputDirectory(nuspecFilePath, settings);
            var resultPath = outputDirectory.GetFilePath(filename).MakeAbsolute(_environment);

            // Make sure the new nuspec file does not exist.
            var nuspecFile = _fileSystem.GetFile(resultPath);
            if (nuspecFile.Exists)
            {
                const string format = "Could not create the nuspec file '{0}' since it already exist.";
                throw new CakeException(string.Format(format, resultPath.FullPath));
            }

            // Now create the file.
            using (var stream = nuspecFile.OpenWrite())
            {
                document.Save(stream);
            }

            // Return the new path.
            return nuspecFile.Path;
        }

        private DirectoryPath GetOutputDirectory(FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            return settings.OutputDirectory != null
                ? settings.OutputDirectory.MakeAbsolute(_environment)
                : nuspecFilePath.GetDirectory().MakeAbsolute(_environment);
        }

        private ICollection<string> GetPackParameters(FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            var parameters = new List<string> { "pack" };

            // Version
            if (!string.IsNullOrWhiteSpace(settings.Version))
            {
                parameters.Add("-Version");
                parameters.Add(settings.Version.Quote());
            }

            // Base path
            if (settings.BasePath != null)
            {
                parameters.Add("-BasePath");
                parameters.Add(settings.BasePath.MakeAbsolute(_environment).FullPath.Quote());
            }

            // Output directory
            if (settings.OutputDirectory != null)
            {
                parameters.Add("-OutputDirectory");
                parameters.Add(settings.OutputDirectory.MakeAbsolute(_environment).FullPath.Quote());
            }

            // Nuspec file
            parameters.Add(nuspecFilePath.MakeAbsolute(_environment).FullPath.Quote());

            // No package analysis?
            if (settings.NoPackageAnalysis)
            {
                parameters.Add("-NoPackageAnalysis");
            }

            // Symbols?
            if (settings.Symbols)
            {
                parameters.Add("-Symbols");
            }
            return parameters;
        }
    }
}
