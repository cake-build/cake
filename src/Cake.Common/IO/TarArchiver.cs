// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Formats.Tar;
using System.Globalization;
using System.IO;
using System.IO.Compression;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Path = Cake.Core.IO.Path;

namespace Cake.Common.IO;

/// <summary>
/// Creates and extracts tar archives.
/// </summary>
public sealed class TarArchiver
{
    private readonly IFileSystem _fileSystem;
    private readonly ICakeEnvironment _environment;
    private readonly ICakeLog _log;
    private readonly StringComparison _comparison;

    /// <summary>
    /// Initializes a new instance of the <see cref="TarArchiver"/> class.
    /// </summary>
    /// <param name="fileSystem">The file system.</param>
    /// <param name="environment">The environment.</param>
    /// <param name="log">The log.</param>
    public TarArchiver(IFileSystem fileSystem, ICakeEnvironment environment, ICakeLog log)
    {
        ArgumentNullException.ThrowIfNull(fileSystem);
        ArgumentNullException.ThrowIfNull(environment);
        ArgumentNullException.ThrowIfNull(log);
        _fileSystem = fileSystem;
        _environment = environment;
        _log = log;
        _comparison = environment.Platform.IsUnix() ? StringComparison.Ordinal : StringComparison.OrdinalIgnoreCase;
    }

    /// <summary>
    /// Creates a tar archive from the specified paths.
    /// </summary>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="paths">The paths to archive.</param>
    /// <param name="settings">The settings.</param>
    public void Tar(DirectoryPath rootPath, FilePath outputPath, IEnumerable<Path> paths, TarSettings settings = null)
    {
        ArgumentNullException.ThrowIfNull(rootPath);
        ArgumentNullException.ThrowIfNull(outputPath);
        ArgumentNullException.ThrowIfNull(paths);
        settings ??= new TarSettings();

        rootPath = rootPath.MakeAbsolute(_environment);
        outputPath = outputPath.MakeAbsolute(_environment);

        using var archive = CreateArchive(outputPath, settings);
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
                WriteDirectoryEntry(archive.Writer, absoluteDirectoryPath, relativeDirectoryPath);
                directories.Add(relativeDirectoryPath);
            }

            if (absoluteFilePath != null && !string.IsNullOrEmpty(relativeFilePath))
            {
                WriteFileEntry(archive.Writer, absoluteFilePath, relativeFilePath);
            }
        }

        _log.Verbose("Tar successfully created: {0}", outputPath.FullPath);
    }

    /// <summary>
    /// Creates a tar archive from the specified files.
    /// </summary>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="filePaths">The files to archive.</param>
    /// <param name="settings">The settings.</param>
    public void Tar(DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths, TarSettings settings = null)
    {
        ArgumentNullException.ThrowIfNull(rootPath);
        ArgumentNullException.ThrowIfNull(outputPath);
        ArgumentNullException.ThrowIfNull(filePaths);
        settings ??= new TarSettings();

        rootPath = rootPath.MakeAbsolute(_environment);
        outputPath = outputPath.MakeAbsolute(_environment);

        using var archive = CreateArchive(outputPath, settings);
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
                WriteDirectoryEntry(archive.Writer, absoluteDirectoryPath, relativeDirectoryPath);
                directories.Add(relativeDirectoryPath);
            }

            WriteFileEntry(archive.Writer, absoluteFilePath, relativeFilePath);
        }

        _log.Verbose("Tar successfully created: {0}", outputPath.FullPath);
    }

    /// <summary>
    /// Extracts the specified archive to the specified output path.
    /// </summary>
    /// <param name="archivePath">The archive path.</param>
    /// <param name="outputPath">The output directory path.</param>
    /// <param name="settings">The settings.</param>
    public void Untar(FilePath archivePath, DirectoryPath outputPath, UntarSettings settings = null)
    {
        ArgumentNullException.ThrowIfNull(archivePath);
        Untar([archivePath], outputPath, settings);
    }

    /// <summary>
    /// Extracts the specified archives to the specified output path.
    /// </summary>
    /// <param name="archivePaths">The archive paths.</param>
    /// <param name="outputPath">The output directory path.</param>
    /// <param name="settings">The settings.</param>
    public void Untar(IEnumerable<FilePath> archivePaths, DirectoryPath outputPath, UntarSettings settings = null)
    {
        ArgumentNullException.ThrowIfNull(archivePaths);
        ArgumentNullException.ThrowIfNull(outputPath);
        settings ??= new UntarSettings();

        outputPath = outputPath.MakeAbsolute(_environment);
        var outputDirectory = _fileSystem.GetDirectory(outputPath);
        if (!outputDirectory.Exists)
        {
            outputDirectory.Create();
        }

        foreach (var archivePath in archivePaths)
        {
            ArgumentNullException.ThrowIfNull(archivePath);
            ExtractArchive(archivePath.MakeAbsolute(_environment), outputPath, settings);
        }
    }

    private ArchiveWriter CreateArchive(FilePath outputPath, TarSettings settings)
    {
        var outputFile = _fileSystem.GetFile(outputPath);
        if (!settings.Overwrite && outputFile.Exists)
        {
            throw new CakeException(string.Format(CultureInfo.InvariantCulture, "The file '{0}' already exists.", outputPath.FullPath));
        }

        _log.Verbose("Creating tar file: {0}", outputPath.FullPath);
        var outputStream = outputFile.Open(FileMode.Create, FileAccess.Write, FileShare.None);
        Stream archiveStream = outputStream;
        GZipStream gzipStream = null;
        if (settings.Compression == TarCompression.GZip)
        {
            gzipStream = new GZipStream(outputStream, CompressionMode.Compress, leaveOpen: true);
            archiveStream = gzipStream;
        }

        return new ArchiveWriter(new TarWriter(archiveStream, TarEntryFormat.Pax, leaveOpen: true), gzipStream, outputStream);
    }

    private void WriteDirectoryEntry(TarWriter writer, DirectoryPath absoluteDirectoryPath, string relativeDirectoryPath)
    {
        _log.Verbose("Storing directory {0}", absoluteDirectoryPath);
        var directory = _fileSystem.GetDirectory(absoluteDirectoryPath);
        var entry = new PaxTarEntry(TarEntryType.Directory, NormalizeEntryName(relativeDirectoryPath));
        if (directory.LastWriteTimeUtc.HasValue)
        {
            entry.ModificationTime = new DateTimeOffset(DateTime.SpecifyKind(directory.LastWriteTimeUtc.Value, DateTimeKind.Utc));
        }
        if (_environment.Platform.IsUnix() && directory.UnixFileMode.HasValue)
        {
            entry.Mode = directory.UnixFileMode.Value;
        }
        writer.WriteEntry(entry);
    }

    private void WriteFileEntry(TarWriter writer, FilePath absoluteFilePath, string relativeFilePath)
    {
        _log.Verbose("Archiving file {0}", absoluteFilePath);
        var file = _fileSystem.GetFile(absoluteFilePath);
        using var fileStream = file.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        var entry = new PaxTarEntry(TarEntryType.RegularFile, NormalizeEntryName(relativeFilePath))
        {
            DataStream = fileStream
        };
        if (file.LastWriteTimeUtc.HasValue)
        {
            entry.ModificationTime = new DateTimeOffset(DateTime.SpecifyKind(file.LastWriteTimeUtc.Value, DateTimeKind.Utc));
        }
        if (_environment.Platform.IsUnix() && file.UnixFileMode.HasValue)
        {
            entry.Mode = file.UnixFileMode.Value;
        }
        writer.WriteEntry(entry);
    }

    private void ExtractArchive(FilePath archivePath, DirectoryPath outputPath, UntarSettings settings)
    {
        _log.Verbose("Extracting tar file {0} to {1}", archivePath.FullPath, outputPath.FullPath);
        var archiveFile = _fileSystem.GetFile(archivePath);
        using var inputStream = archiveFile.Open(FileMode.Open, FileAccess.Read, FileShare.Read);
        var gzipStream = IsGZip(inputStream) ? new GZipStream(inputStream, CompressionMode.Decompress, leaveOpen: true) : null;
        using (gzipStream)
        using (var reader = new TarReader(gzipStream ?? inputStream, leaveOpen: true))
        {
            TarEntry entry;
            while ((entry = reader.GetNextEntry()) != null)
            {
                ExtractEntry(entry, outputPath, settings);
            }
        }
        _log.Verbose("Tar successfully extracted: {0}", archivePath.FullPath);
    }

    private void ExtractEntry(TarEntry entry, DirectoryPath outputPath, UntarSettings settings)
    {
        if (entry.EntryType is TarEntryType.SymbolicLink or TarEntryType.HardLink)
        {
            _log.Verbose("Skipping {0} entry {1}", entry.EntryType, entry.Name);
            return;
        }

        var entryName = NormalizeEntryName(entry.Name);
        if (string.IsNullOrEmpty(entryName))
        {
            return;
        }

        ValidateEntryName(entryName);

        if (entry.EntryType == TarEntryType.Directory)
        {
            var directoryPath = outputPath.Combine(entryName);
            EnsureRelativeToOutput(outputPath, directoryPath);
            var directory = _fileSystem.GetDirectory(directoryPath);
            if (!directory.Exists)
            {
                directory.Create();
            }
            ApplyDirectoryMetadata(directory, entry);
            return;
        }

        if (entry.DataStream == null)
        {
            return;
        }

        var filePath = outputPath.CombineWithFilePath(entryName);
        EnsureRelativeToOutput(outputPath, filePath);

        var file = _fileSystem.GetFile(filePath);
        if (settings.SkipUnchangedFiles &&
            file.Exists &&
            file.LastWriteTimeUtc.HasValue &&
            file.LastWriteTimeUtc.Value >= entry.ModificationTime.UtcDateTime)
        {
            _log.Verbose("Skipping unchanged file {0}", filePath.FullPath);
            return;
        }

        if (file.Exists && file.Attributes.HasFlag(FileAttributes.ReadOnly))
        {
            if (!settings.OverwriteReadOnlyFiles)
            {
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, "The file '{0}' is read-only.", filePath.FullPath));
            }

            file.Attributes &= ~FileAttributes.ReadOnly;
        }

        var parent = _fileSystem.GetDirectory(filePath.GetDirectory());
        if (!parent.Exists)
        {
            parent.Create();
        }

        using (var outputStream = file.Open(FileMode.Create, FileAccess.Write, FileShare.None))
        {
            entry.DataStream.CopyTo(outputStream);
        }

        file.SetLastWriteTimeUtc(entry.ModificationTime.UtcDateTime);
        if (_environment.Platform.IsUnix())
        {
            file.SetUnixFileMode(entry.Mode);
        }
    }

    private void ApplyDirectoryMetadata(IDirectory directory, TarEntry entry)
    {
        directory.SetLastWriteTimeUtc(entry.ModificationTime.UtcDateTime);
        if (_environment.Platform.IsUnix())
        {
            directory.SetUnixFileMode(entry.Mode);
        }
    }

    private void EnsureRelativeToOutput(DirectoryPath outputPath, Path path)
    {
        var absolute = path is FilePath filePath
            ? filePath.MakeAbsolute(_environment).FullPath
            : ((DirectoryPath)path).MakeAbsolute(_environment).FullPath;
        var root = outputPath.FullPath.TrimEnd('/');
        if (!absolute.Equals(root, _comparison) &&
            !absolute.StartsWith(root + "/", _comparison))
        {
            throw new CakeException(string.Format(CultureInfo.InvariantCulture, "The archive entry '{0}' is not a valid relative path.", path.FullPath));
        }
    }

    private static void ValidateEntryName(string entryName)
    {
        if (System.IO.Path.IsPathRooted(entryName) ||
            entryName.StartsWith('/') ||
            entryName.Contains(':'))
        {
            throw new CakeException(string.Format(CultureInfo.InvariantCulture, "The archive entry '{0}' is not a valid relative path.", entryName));
        }

        foreach (var segment in entryName.Split('/'))
        {
            if (segment == "..")
            {
                throw new CakeException(string.Format(CultureInfo.InvariantCulture, "The archive entry '{0}' is not a valid relative path.", entryName));
            }
        }
    }

    private static bool IsGZip(Stream stream)
    {
        if (!stream.CanSeek || stream.Length < 2)
        {
            return false;
        }

        var position = stream.Position;
        var header = stream.ReadByte() == 0x1F && stream.ReadByte() == 0x8B;
        stream.Position = position;
        return header;
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

    private static string NormalizeEntryName(string name)
    {
        return name?.Replace('\\', '/').Trim('/');
    }

    private sealed class ArchiveWriter : IDisposable
    {
        private readonly GZipStream _gzipStream;
        private readonly Stream _outputStream;

        public TarWriter Writer { get; }

        public ArchiveWriter(TarWriter writer, GZipStream gzipStream, Stream outputStream)
        {
            Writer = writer;
            _gzipStream = gzipStream;
            _outputStream = outputStream;
        }

        public void Dispose()
        {
            Writer.Dispose();
            _gzipStream?.Dispose();
            _outputStream.Dispose();
        }
    }
}
