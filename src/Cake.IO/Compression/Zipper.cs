using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.IO.Compression
{
    public sealed class Zipper
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly StringComparison _comparison;        

        public Zipper(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
        {
            if (fileSystem == null)
            {
                throw new ArgumentNullException("fileSystem");
            }
            if (environment == null)
            {
                throw new ArgumentNullException("environment");
            }
            if (log == null)
            {
                throw new ArgumentNullException("log");
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
            _comparison = environment.IsUnix() ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        }

        public void Zip(DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths)
        {
            if (rootPath == null)
            {
                throw new ArgumentNullException("rootPath");
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException("outputPath");
            }
            if (filePaths == null)
            {
                throw new ArgumentNullException("filePaths");
            }

            // Make root path and output file path absolute.
            rootPath = rootPath.MakeAbsolute(_environment);
            outputPath = outputPath.MakeAbsolute(_environment);

            // Get the output file.
            var outputFile = _fileSystem.GetFile(outputPath);

            // Open up a stream to the output file.
            _log.Verbose("Creating zip file: {0}", outputPath.FullPath);
            using (var outputStream = outputFile.Open(FileMode.Create, FileAccess.Write, FileShare.None))
            using (var archive = new ZipArchive(outputStream, ZipArchiveMode.Create))
            {
                foreach (var inputPath in filePaths)
                {                    
                    var file = _fileSystem.GetFile(inputPath.MakeAbsolute(_environment));
                    using (var inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Get the relative filename to the rootPath.
                        var relativeFilePath = GetRelativeFilePath(rootPath, inputPath);
                        _log.Verbose("Adding file: {0}", relativeFilePath);

                        // Create the zip archive entry.
                        var entry = archive.CreateEntry(relativeFilePath.FullPath);
                        using (var entryStream = entry.Open())
                        {
                            // Copy the content of the input stream to the entry stream.
                            inputStream.CopyTo(entryStream);
                        }   
                    }
                }
            }
            _log.Verbose("Zip successfully created: {0}", outputPath.FullPath);
        }

        private FilePath GetRelativeFilePath(DirectoryPath root, FilePath file)
        {
            if (!file.FullPath.StartsWith(root.FullPath, _comparison))
            {
                const string format = "File '{0}' is not relative to root path '{1}'.";
                throw new CakeException(string.Format(format, file.FullPath, root.FullPath));
            }
            return file.FullPath.Substring(root.FullPath.Length + 1);
        }
    }
}
