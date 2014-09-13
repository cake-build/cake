using System.Globalization;
using System.Xml;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.Tools.NuGet.Pack
{
    internal sealed class NuspecProcessor
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;

        public NuspecProcessor(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
        {
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
        }

        public FilePath Process(FilePath nuspecFilePath, NuGetPackSettings settings)
        {
            // Make the nuspec file path absolute.
            nuspecFilePath = nuspecFilePath.MakeAbsolute(_environment);

            // Make sure the nuspec file exist.
            var nuspecFile = _fileSystem.GetFile(nuspecFilePath);
            if (!nuspecFile.Exists)
            {
                const string format = "Could not find nuspec file '{0}'.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, nuspecFilePath.FullPath));
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
            var resultPath = nuspecFilePath.GetDirectory().GetFilePath(filename).MakeAbsolute(_environment);

            // Make sure the new nuspec file does not exist.
            var nuspecFile = _fileSystem.GetFile(resultPath);
            if (nuspecFile.Exists)
            {
                const string format = "Could not create the nuspec file '{0}' since it already exist.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, resultPath.FullPath));
            }

            // Now create the file.
            using (var stream = nuspecFile.OpenWrite())
            {
                document.Save(stream);
            }

            // Return the new path.
            return nuspecFile.Path;
        }
    }
}
