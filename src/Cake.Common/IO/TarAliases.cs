// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO;

/// <summary>
/// Contains functionality related to creating and extracting tar archives.
/// </summary>
[CakeAliasCategory("Compression")]
public static class TarAliases
{
    /// <summary>
    /// Creates a tar archive from the specified directory.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <example>
    /// <code>
    /// Tar("./publish", "publish.tar");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Tar(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath)
        => context.Tar(rootPath, outputPath, new TarSettings());

    /// <summary>
    /// Creates a tar archive from the specified directory.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// Tar("./publish", "publish.tar.gz", new TarSettings {
    ///     Compression = TarCompression.GZip,
    ///     Overwrite = true
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Tar(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, TarSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        var paths = context.GetPaths(string.Concat(rootPath, "/**/*"));
        var archiver = new TarArchiver(context.FileSystem, context.Environment, context.Log);
        archiver.Tar(rootPath, outputPath, paths, settings);
    }

    /// <summary>
    /// Creates a tar archive from the files matching the specified pattern.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="pattern">The pattern.</param>
    /// <example>
    /// <code>
    /// Tar("./", "XmlFiles.tar", "./*.xml");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Tar(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern)
        => context.Tar(rootPath, outputPath, pattern, new TarSettings());

    /// <summary>
    /// Creates a tar archive from the files matching the specified pattern.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="pattern">The pattern.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// Tar("./", "XmlFiles.tar.gz", "./*.xml", new TarSettings {
    ///     Compression = TarCompression.GZip
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Tar(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, string pattern, TarSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        var filePaths = context.GetFiles(pattern);
        if (filePaths.Count == 0)
        {
            context.Log.Verbose("The provided pattern did not match any files.");
            return;
        }
        Tar(context, rootPath, outputPath, filePaths, settings);
    }

    /// <summary>
    /// Creates a tar archive from the specified files.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="filePaths">The file paths.</param>
    /// <example>
    /// <code>
    /// var files = GetFiles("./**/Cake.*.dll");
    /// Tar("./", "CakeAssemblies.tar", files);
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Tar(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths)
        => context.Tar(rootPath, outputPath, filePaths, new TarSettings());

    /// <summary>
    /// Creates a tar archive from the specified files.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="filePaths">The file paths.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// var files = GetFiles("./**/Cake.*.dll");
    /// Tar("./", "CakeAssemblies.tar.gz", files, new TarSettings {
    ///     Compression = TarCompression.GZip
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Tar(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<FilePath> filePaths, TarSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        var archiver = new TarArchiver(context.FileSystem, context.Environment, context.Log);
        archiver.Tar(rootPath, outputPath, filePaths, settings);
    }

    /// <summary>
    /// Creates a tar archive from the specified files.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="filePaths">The file paths.</param>
    /// <example>
    /// <code>
    /// var files = new [] {
    ///     "./src/Cake/bin/Debug/Cake.Common.dll",
    ///     "./src/Cake/bin/Debug/Cake.Core.dll"
    /// };
    /// Tar("./", "CakeBinaries.tar", files);
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Tar(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<string> filePaths)
        => context.Tar(rootPath, outputPath, filePaths, new TarSettings());

    /// <summary>
    /// Creates a tar archive from the specified files.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="rootPath">The root path.</param>
    /// <param name="outputPath">The output path.</param>
    /// <param name="filePaths">The file paths.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// var files = new [] {
    ///     "./src/Cake/bin/Debug/Cake.Common.dll",
    ///     "./src/Cake/bin/Debug/Cake.Core.dll"
    /// };
    /// Tar("./", "CakeBinaries.tar.gz", files, new TarSettings {
    ///     Compression = TarCompression.GZip
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Tar(this ICakeContext context, DirectoryPath rootPath, FilePath outputPath, IEnumerable<string> filePaths, TarSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        var paths = filePaths.Select(p => new FilePath(p));
        var archiver = new TarArchiver(context.FileSystem, context.Environment, context.Log);
        archiver.Tar(rootPath, outputPath, paths, settings);
    }

    /// <summary>
    /// Extracts the specified tar archive.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="archiveFile">The archive file to extract.</param>
    /// <param name="outputPath">The output path to extract into.</param>
    /// <example>
    /// <code>
    /// Untar("publish.tar.gz", "./publish");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Untar(this ICakeContext context, FilePath archiveFile, DirectoryPath outputPath)
        => context.Untar(archiveFile, outputPath, new UntarSettings());

    /// <summary>
    /// Extracts the specified tar archive.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="archiveFile">The archive file to extract.</param>
    /// <param name="outputPath">The output path to extract into.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// Untar("publish.tar.gz", "./publish", new UntarSettings {
    ///     SkipUnchangedFiles = true,
    ///     OverwriteReadOnlyFiles = false
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Untar(this ICakeContext context, FilePath archiveFile, DirectoryPath outputPath, UntarSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        var archiver = new TarArchiver(context.FileSystem, context.Environment, context.Log);
        archiver.Untar(archiveFile, outputPath, settings);
    }

    /// <summary>
    /// Extracts the specified tar archives.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="archiveFiles">The archive files to extract.</param>
    /// <param name="outputPath">The output path to extract into.</param>
    /// <example>
    /// <code>
    /// Untar(new[] { "a.tar", "b.tar.gz" }, "./dependencies");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Untar(this ICakeContext context, IEnumerable<FilePath> archiveFiles, DirectoryPath outputPath)
        => context.Untar(archiveFiles, outputPath, new UntarSettings());

    /// <summary>
    /// Extracts the specified tar archives.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="archiveFiles">The archive files to extract.</param>
    /// <param name="outputPath">The output path to extract into.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// Untar(new[] { "a.tar", "b.tar.gz" }, "./dependencies", new UntarSettings {
    ///     SkipUnchangedFiles = true,
    ///     OverwriteReadOnlyFiles = false
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Untar(this ICakeContext context, IEnumerable<FilePath> archiveFiles, DirectoryPath outputPath, UntarSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        var archiver = new TarArchiver(context.FileSystem, context.Environment, context.Log);
        archiver.Untar(archiveFiles, outputPath, settings);
    }

    /// <summary>
    /// Extracts the specified tar archives.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="archiveFiles">The archive files to extract.</param>
    /// <param name="outputPath">The output path to extract into.</param>
    /// <example>
    /// <code>
    /// Untar(new[] { "a.tar", "b.tar.gz" }, "./dependencies");
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Untar(this ICakeContext context, IEnumerable<string> archiveFiles, DirectoryPath outputPath)
        => context.Untar(archiveFiles, outputPath, new UntarSettings());

    /// <summary>
    /// Extracts the specified tar archives.
    /// </summary>
    /// <param name="context">The context.</param>
    /// <param name="archiveFiles">The archive files to extract.</param>
    /// <param name="outputPath">The output path to extract into.</param>
    /// <param name="settings">The settings.</param>
    /// <example>
    /// <code>
    /// Untar(new[] { "a.tar", "b.tar.gz" }, "./dependencies", new UntarSettings {
    ///     SkipUnchangedFiles = true,
    ///     OverwriteReadOnlyFiles = false
    /// });
    /// </code>
    /// </example>
    [CakeMethodAlias]
    public static void Untar(this ICakeContext context, IEnumerable<string> archiveFiles, DirectoryPath outputPath, UntarSettings settings)
    {
        ArgumentNullException.ThrowIfNull(context);

        var paths = archiveFiles.Select(p => new FilePath(p));
        var archiver = new TarArchiver(context.FileSystem, context.Environment, context.Log);
        archiver.Untar(paths, outputPath, settings);
    }
}
