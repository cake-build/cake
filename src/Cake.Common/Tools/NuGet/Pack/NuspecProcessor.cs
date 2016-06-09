// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
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

        public FilePath Process(NuGetPackSettings settings)
        {
            var nuspecFilePath = settings.OutputDirectory
                                    .CombineWithFilePath(string.Concat(settings.Id, ".nuspec"))
                                    .MakeAbsolute(_environment);

            var xml = LoadEmptyNuSpec();

            return ProcessXml(nuspecFilePath, settings, xml);
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

            return ProcessXml(nuspecFilePath, settings, xml);
        }

        private FilePath ProcessXml(FilePath nuspecFilePath, NuGetPackSettings settings, XmlDocument xml)
        {
            // Process the XML.
            _log.Debug("Transforming nuspec...");
            NuspecTransformer.Transform(xml, settings);

            // Return the file of the new nuspec.
            _log.Debug("Writing temporary nuspec...");
            return SaveNuspecXml(nuspecFilePath, xml);
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

        private static XmlDocument LoadEmptyNuSpec()
        {
            XmlDocument xml = new XmlDocument();
            xml.LoadXml(@"<?xml version=""1.0"" encoding=""utf-8""?>
<package xmlns:xsi=""http://www.w3.org/2001/XMLSchema-instance"" xmlns:xsd=""http://www.w3.org/2001/XMLSchema"">
  <metadata xmlns=""http://schemas.microsoft.com/packaging/2010/07/nuspec.xsd"">
    <id></id>
    <version>0.0.0</version>
    <authors></authors>
    <description></description>
  </metadata>
  <files>
  </files>
</package>");
            return xml;
        }

        private FilePath SaveNuspecXml(FilePath nuspecFilePath, XmlDocument document)
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
