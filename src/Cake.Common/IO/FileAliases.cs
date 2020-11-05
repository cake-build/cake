// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using Cake.Common.IO.Paths;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    /// <summary>
    /// Contains functionality related to file operations.
    /// </summary>
    [CakeAliasCategory("File Operations")]
    public static class FileAliases
    {
        /// <summary>
        /// Gets a file path from string.
        /// </summary>
        /// <example>
        /// <code>
        /// // Get the temp file.
        /// var root = Directory("./");
        /// var temp = root + File("temp");
        ///
        /// // Delete the file.
        /// CleanDirectory(temp);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="path">The path.</param>
        /// <returns>A file path.</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("Cake.Common.IO.Paths")]
        public static ConvertableFilePath File(this ICakeContext context, string path)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            return new ConvertableFilePath(new FilePath(path));
        }

        /// <summary>
        /// Copies an existing file to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <example>
        /// <code>
        /// CopyFileToDirectory("test.txt", "./targetdir");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFileToDirectory(this ICakeContext context, FilePath filePath, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFileToDirectory(context, filePath, targetDirectoryPath);
        }

        /// <summary>
        /// Copies an existing file to a new file, providing the option to specify a new file name.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetFilePath">The target file path.</param>
        /// <example>
        /// <code>
        /// CopyFile("test.tmp", "test.txt");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFile(this ICakeContext context, FilePath filePath, FilePath targetFilePath)
        {
            FileCopier.CopyFile(context, filePath, targetFilePath);
        }

        /// <summary>
        /// Copies all files matching the provided pattern to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <example>
        /// <code>
        /// CopyFiles("Cake.*", "./publish");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFiles(this ICakeContext context, GlobPattern pattern, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFiles(context, pattern, targetDirectoryPath, false);
        }

        /// <summary>
        /// Copies existing files to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <example>
        /// <code>
        /// var files = GetFiles("./**/Cake.*");
        /// CopyFiles(files, "destination");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFiles(context, filePaths, targetDirectoryPath, false);
        }

        /// <summary>
        /// Copies existing files to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <example>
        /// <code>
        /// CreateDirectory("destination");
        /// var files = new [] {
        ///     "Cake.exe",
        ///     "Cake.pdb"
        /// };
        /// CopyFiles(files, "destination");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFiles(this ICakeContext context, IEnumerable<string> filePaths, DirectoryPath targetDirectoryPath)
        {
            if (filePaths == null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }
            var paths = filePaths.Select(p => new FilePath(p));
            FileCopier.CopyFiles(context, paths, targetDirectoryPath, false);
        }

        /// <summary>
        /// Copies all files matching the provided pattern to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <param name="preserveFolderStructure">Keep the folder structure.</param>
        /// <example>
        /// <code>
        /// CopyFiles("Cake.*", "./publish");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFiles(this ICakeContext context, GlobPattern pattern, DirectoryPath targetDirectoryPath, bool preserveFolderStructure)
        {
            FileCopier.CopyFiles(context, pattern, targetDirectoryPath, preserveFolderStructure);
        }

        /// <summary>
        /// Copies existing files to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <param name="preserveFolderStructure">Keep the folder structure.</param>
        /// <example>
        /// <code>
        /// var files = GetFiles("./**/Cake.*");
        /// CopyFiles(files, "destination");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath, bool preserveFolderStructure)
        {
            FileCopier.CopyFiles(context, filePaths, targetDirectoryPath, preserveFolderStructure);
        }

        /// <summary>
        /// Copies existing files to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <param name="preserveFolderStructure">Keep the folder structure.</param>
        /// <example>
        /// <code>
        /// CreateDirectory("destination");
        /// var files = new [] {
        ///     "Cake.exe",
        ///     "Cake.pdb"
        /// };
        /// CopyFiles(files, "destination");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFiles(this ICakeContext context, IEnumerable<string> filePaths, DirectoryPath targetDirectoryPath, bool preserveFolderStructure)
        {
            if (filePaths == null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }
            var paths = filePaths.Select(p => new FilePath(p));
            FileCopier.CopyFiles(context, paths, targetDirectoryPath, preserveFolderStructure);
        }

        /// <summary>
        /// Moves an existing file to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <example>
        /// <code>
        /// MoveFileToDirectory("test.txt", "./targetdir");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Move")]
        public static void MoveFileToDirectory(this ICakeContext context, FilePath filePath, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFileToDirectory(context, filePath, targetDirectoryPath);
        }

        /// <summary>
        /// Moves existing files matching the specified pattern to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <example>
        /// <code>
        /// MoveFiles("./publish/Cake.*", "./destination");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Move")]
        public static void MoveFiles(this ICakeContext context, GlobPattern pattern, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFiles(context, pattern, targetDirectoryPath);
        }

        /// <summary>
        /// Moves existing files to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        /// <example>
        /// <code>
        /// var files = GetFiles("./publish/Cake.*");
        /// MoveFiles(files, "destination");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Move")]
        public static void MoveFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFiles(context, filePaths, targetDirectoryPath);
        }

        /// <summary>
        /// Moves an existing file to a new location, providing the option to specify a new file name.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetFilePath">The target file path.</param>
        /// <example>
        /// <code>
        /// MoveFile("test.tmp", "test.txt");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Move")]
        public static void MoveFile(this ICakeContext context, FilePath filePath, FilePath targetFilePath)
        {
            FileMover.MoveFile(context, filePath, targetFilePath);
        }

        /// <summary>
        /// Deletes the specified files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <example>
        /// <code>
        /// DeleteFiles("./publish/Cake.*");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Delete")]
        public static void DeleteFiles(this ICakeContext context, GlobPattern pattern)
        {
            FileDeleter.DeleteFiles(context, pattern);
        }

        /// <summary>
        /// Deletes the specified files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <example>
        /// <code>
        /// var files = GetFiles("./destination/Cake.*");
        /// DeleteFiles(files);
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Delete")]
        public static void DeleteFiles(this ICakeContext context, IEnumerable<FilePath> filePaths)
        {
            FileDeleter.DeleteFiles(context, filePaths);
        }

        /// <summary>
        /// Deletes the specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <example>
        /// <code>
        /// DeleteFile("deleteme.txt");
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Delete")]
        public static void DeleteFile(this ICakeContext context, FilePath filePath)
        {
            FileDeleter.DeleteFile(context, filePath);
        }

        /// <summary>
        /// Determines whether the given path refers to an existing file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The <see cref="FilePath"/> to check.</param>
        /// <returns><c>true</c> if <paramref name="filePath"/> refers to an existing file;
        /// <c>false</c> if the file does not exist or an error occurs when trying to
        /// determine if the specified file exists.</returns>
        /// <example>
        /// <code>
        /// if (FileExists("findme.txt"))
        /// {
        ///     Information("File exists!");
        /// }
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Exists")]
        public static bool FileExists(this ICakeContext context, FilePath filePath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return context.FileSystem.GetFile(filePath.MakeAbsolute(context.Environment)).Exists;
        }

        /// <summary>
        /// Makes the path absolute (if relative) using the current working directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The path.</param>
        /// <returns>An absolute file path.</returns>
        /// <example>
        /// <code>
        /// var path = MakeAbsolute(File("./resources"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Path")]
        public static FilePath MakeAbsolute(this ICakeContext context, FilePath filePath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return filePath.MakeAbsolute(context.Environment);
        }

        /// <summary>
        /// Gets the size of a file in bytes.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The path.</param>
        /// <returns>Size of file in bytes or -1 if file doesn't exist.</returns>
        /// <example>
        /// <code>
        /// Information("File size: {0}", FileSize("./build.cake"));
        /// </code>
        /// </example>
        [CakeMethodAlias]
        [CakeAliasCategory("Exists")]
        public static long FileSize(this ICakeContext context, FilePath filePath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            var file = context.FileSystem.GetFile(filePath.MakeAbsolute(context.Environment));
            if (!file.Exists)
            {
                throw new FileNotFoundException("Unable to find the specified file.", filePath.FullPath);
            }

            return file.Length;
        }

        /// <summary>
        /// Expands all environment variables in the provided <see cref="FilePath"/>.
        /// </summary>
        /// <example>
        /// <code>
        /// var path = new FilePath("%APPDATA%/foo.bar");
        /// var expanded = path.ExpandEnvironmentVariables(environment);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The path.</param>
        /// <returns>A new <see cref="FilePath"/> with each environment variable replaced by its value.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Path")]
        public static FilePath ExpandEnvironmentVariables(this ICakeContext context, FilePath filePath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }

            return filePath.ExpandEnvironmentVariables(context.Environment);
        }
    }
}