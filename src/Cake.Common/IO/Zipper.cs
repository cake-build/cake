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
using Directory = Cake.Core.IO.Directory;
using File = Cake.Core.IO.File;
using Path = Cake.Core.IO.Path;

namespace Cake.Common.IO
{
    /// <summary>
    /// Performs Zip compression.
    /// </summary>
    public sealed class Zipper
    {
        private const int ValidZipDateYearMin = 1980;
        private const int ValidZipDateYearMax = 2107;
        private static readonly DateTimeOffset InvalidZipDateIndicator = new DateTimeOffset(ValidZipDateYearMin, 1, 1, 0, 0, 0, TimeSpan.Zero);
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
                throw new ArgumentNullException(nameof(fileSystem));
            }
            if (environment == null)
            {
                throw new ArgumentNullException(nameof(environment));
            }
            if (log == null)
            {
                throw new ArgumentNullException(nameof(log));
            }
            _fileSystem = fileSystem;
            _environment = environment;
            _log = log;
            _comparison = environment.Platform.IsUnix() ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
        }

        /// <summary>
        /// Zips the specified directory.
        /// </summary>
        /// <param name="rootPath">The root path.</param>
        /// <param name="outputPath">The output path.</param>
        /// <param name="paths">The paths to zip.</param>
        [System.Diagnostics.CodeAnalysis.SuppressMessage("Microsoft.Usage", "CA2202:Do not dispose objects multiple times")]
        public void Zip(DirectoryPath rootPath, FilePath outputPath, IEnumerable<Path> paths)
        {
            if (rootPath == null)
            {
                throw new ArgumentNullException(nameof(rootPath));
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException(nameof(outputPath));
            }
            if (paths == null)
            {
                throw new ArgumentNullException(nameof(paths));
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
                var directories = new HashSet<DirectoryPath>((paths as PathCollection)?.Comparer ?? PathComparer.Default);
                foreach (var path in paths)
                {
                    var absoluteFilePath = (path as FilePath)?.MakeAbsolute(_environment);
                    var relativeFilePath = GetRelativePath(rootPath, absoluteFilePath);
                    var absoluteDirectoryPath = (path as DirectoryPath)?.MakeAbsolute(_environment) ?? absoluteFilePath?.GetDirectory();
                    var relativeDirectoryPath = GetRelativePath(rootPath, absoluteDirectoryPath);

                    if (absoluteDirectoryPath != null &&
                        !string.IsNullOrEmpty(relativeDirectoryPath) && !directories.Contains(relativeDirectoryPath))
                    {
                        // Create directory entry.
                        _log.Verbose("Storing directory {0}", absoluteDirectoryPath);
                        var directory = _fileSystem.GetDirectory(absoluteDirectoryPath);
                        var entry = archive.CreateEntry(relativeDirectoryPath + "/");
                        entry.LastWriteTime = (directory as Directory)?.LastWriteTime ?? DateTimeOffset.Now;
                        directories.Add(relativeDirectoryPath);
                    }

                    if (absoluteFilePath != null && !string.IsNullOrEmpty(relativeFilePath))
                    {
                        // Create file entry.
                        _log.Verbose("Compressing file {0}", absoluteFilePath);
                        var file = _fileSystem.GetFile(absoluteFilePath);
                        using (var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                        {
                            var entry = archive.CreateEntry(relativeFilePath);
                            entry.LastWriteTime = (file as File)?.LastWriteTime ?? DateTimeOffset.Now;
                            using (var entryStream = entry.Open())
                            {
                                fileStream.CopyTo(entryStream);
                            }
                        }
                    }
                }
            }
            _log.Verbose("Zip successfully created: {0}", outputPath.FullPath);
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
                throw new ArgumentNullException(nameof(rootPath));
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException(nameof(outputPath));
            }
            if (filePaths == null)
            {
                throw new ArgumentNullException(nameof(filePaths));
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
                var directories = new HashSet<DirectoryPath>((filePaths as FilePathCollection)?.Comparer ?? PathComparer.Default);
                foreach (var filePath in filePaths)
                {
                    var absoluteFilePath = filePath.MakeAbsolute(_environment);
                    var relativeFilePath = GetRelativePath(rootPath, absoluteFilePath);
                    var absoluteDirectoryPath = absoluteFilePath.GetDirectory();
                    var relativeDirectoryPath = GetRelativePath(rootPath, absoluteDirectoryPath);

                    if (absoluteDirectoryPath != null &&
                        !string.IsNullOrEmpty(relativeDirectoryPath) && !directories.Contains(relativeDirectoryPath))
                    {
                        // Create directory entry.
                        _log.Verbose("Storing directory {0}", absoluteDirectoryPath);
                        var directory = _fileSystem.GetDirectory(absoluteDirectoryPath);
                        var entry = archive.CreateEntry(relativeDirectoryPath + "/");
                        entry.LastWriteTime = GetValidZipDateTimeOffset((directory as Directory)?.LastWriteTime);
                        directories.Add(relativeDirectoryPath);
                    }

                    // Create file entry.
                    _log.Verbose("Compressing file {0}", absoluteFilePath);
                    var file = _fileSystem.GetFile(absoluteFilePath);
                    using (var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read))
                    {
                        var entry = archive.CreateEntry(relativeFilePath);
                        entry.LastWriteTime = GetValidZipDateTimeOffset((file as File)?.LastWriteTime);
                        using (var entryStream = entry.Open())
                        {
                            fileStream.CopyTo(entryStream);
                        }
                    }
                }
            }
            _log.Verbose("Zip successfully created: {0}", outputPath.FullPath);
        }

        /// <summary>
        /// Unzips the specified file to the specified output path.
        /// </summary>
        /// <param name="zipPath">Zip file path.</param>
        /// <param name="outputPath">Output directory path.</param>
        public void Unzip(FilePath zipPath, DirectoryPath outputPath)
        {
            if (zipPath == null)
            {
                throw new ArgumentNullException(nameof(zipPath));
            }
            if (outputPath == null)
            {
                throw new ArgumentNullException(nameof(outputPath));
            }

            // Make root path and output file path absolute.
            zipPath = zipPath.MakeAbsolute(_environment);
            outputPath = outputPath.MakeAbsolute(_environment);

            _log.Verbose("Unzipping file {0} to {1}", zipPath.FullPath, outputPath.FullPath);
            ZipFile.ExtractToDirectory(zipPath.FullPath, outputPath.FullPath);
        }

        private string GetRelativePath(DirectoryPath root, Path path)
        {
            if (path != null && !path.FullPath.StartsWith(root.FullPath, _comparison))
            {
                const string format = "Path '{0}' is not relative to root path '{1}'.";
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, format, path.FullPath, root.FullPath));
            }
            return path?.FullPath.Substring(root.FullPath.Length + (root.FullPath.Length > 1 && path.FullPath.Length > root.FullPath.Length ? 1 : 0));
        }

        private static DateTimeOffset GetValidZipDateTimeOffset(DateTime? value)
        {
            var offsetValue = value ?? DateTime.UtcNow;
            if (offsetValue.Year >= ValidZipDateYearMin && offsetValue.Year <= ValidZipDateYearMax)
            {
                return offsetValue;
            }

            return InvalidZipDateIndicator;
        }
    }
}