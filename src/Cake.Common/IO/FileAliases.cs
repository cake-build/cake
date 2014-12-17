using System;
using System.Collections.Generic;
using System.Linq;
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
        /// Copies an existing file to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFiles(this ICakeContext context, string pattern, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFiles(context, pattern, targetDirectoryPath);
        }

        /// <summary>
        /// Copies existing files to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFiles(context, filePaths, targetDirectoryPath);
        }

        /// <summary>
        /// Copies existing files to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyFiles(this ICakeContext context, IEnumerable<string> filePaths, DirectoryPath targetDirectoryPath)
        {
            if (filePaths == null)
            {
                throw new ArgumentNullException("filePaths");
            }
            var paths = filePaths.Select(p => new FilePath(p));
            FileCopier.CopyFiles(context, paths, targetDirectoryPath);
        }

        /// <summary>
        /// Moves an existing file to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("Move")]
        public static void MoveFiles(this ICakeContext context, string pattern, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFiles(context, pattern, targetDirectoryPath);
        }

        /// <summary>
        /// Moves existing files to a new location.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("Delete")]
        public static void DeleteFiles(this ICakeContext context, string pattern)
        {
            FileDeleter.DeleteFiles(context, pattern);
        }

        /// <summary>
        /// Deletes the specified files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
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
        [CakeMethodAlias]
        [CakeAliasCategory("Delete")]
        public static void DeleteFile(this ICakeContext context, FilePath filePath)
        {
            FileDeleter.DeleteFile(context, filePath);
        }
    }
}
