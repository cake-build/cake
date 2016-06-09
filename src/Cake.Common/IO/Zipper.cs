// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    /// <summary>
    /// Performs Zip compression.
    /// </summary>
    public sealed class Zipper
    {
        private readonly IFileSystem _fileSystem;
        private readonly ICakeEnvironment _environment;
        private readonly ICakeLog _log;
        private readonly StringComparison _comparison;

        /// <summary>
        /// Initializes a new instance of the <see cref="Zipper"/> class.
        /// </summary>
        /// <param name="fileSystem">The file system.</param>
        /// <param name="environment">The environment.</param>
        /// <param name="log">The log.</param>
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

        /// <summary>
        /// Zips the specified directory.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="filePaths">The files to zip.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
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
                    var absoluteInputPath = inputPath.MakeAbsolute(_environment);
                    var file = _fileSystem.GetFile(absoluteInputPath);
                    using (var inputStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        // Get the relative filename to the rootPath.
                        var relativeFilePath = GetRelativeFilePath(rootPath, absoluteInputPath);
                        _log.Verbose("Compressing file {0}", absoluteInputPath);

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

        /// <summary>
        /// Unzips the specified file to the specified output path
        /// </summary>
        /// <param name="zipPath">Zip file path.</param>
        /// <param name="outputPath">Output directory path.</param>
        public void Unzip(FilePath zipPath, DirectoryPath outputPath)
        {
            if (zipPath == null)
            {
                throw new ArgumentNullException("zipPath");
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException("outputPath");
            }

            // Make root path and output file path absolute.
            zipPath = zipPath.MakeAbsolute(_environment);
            outputPath = outputPath.MakeAbsolute(_environment);

            _log.Verbose("Unzipping file {0} to {1}", zipPath.FullPath, outputPath.FullPath);
            ZipFile.ExtractToDirectory(zipPath.FullPath, outputPath.FullPath);
        }

        private FilePath GetRelativeFilePath(DirectoryPath root, FilePath file)
        {
            if (!file.FullPath.StartsWith(root.FullPath, _comparison))
            {
                const string format = "File '{0}' is not relative to root path '{1}'.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, file.FullPath, root.FullPath));
            }
            return file.FullPath.Substring(root.FullPath.Length + 1);
        }
    }
}
