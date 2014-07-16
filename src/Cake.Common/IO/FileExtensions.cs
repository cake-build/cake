using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    /// <summary>
    /// Contains functionality related to file operations.
    /// </summary>
    public static class FileExtensions
    {
        /// <summary>
        /// Copies the specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        [CakeMethodAlias]
        public static void CopyFileToDirectory(this ICakeContext context, FilePath filePath, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFileToDirectory(context, filePath, targetDirectoryPath);
        }

        /// <summary>
        /// Copies the specified file.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetFilePath">The target file path.</param>
        [CakeMethodAlias]
        public static void CopyFile(this ICakeContext context, FilePath filePath, FilePath targetFilePath)
        {
            FileCopier.CopyFile(context, filePath, targetFilePath);
        }

        /// <summary>
        /// Copies the files matching the specified pattern.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        [CakeMethodAlias]
        public static void CopyFiles(this ICakeContext context, string pattern, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFiles(context, pattern, targetDirectoryPath);
        }

        /// <summary>
        /// Copies the specified files.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        [CakeMethodAlias]
        public static void CopyFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFiles(context, filePaths, targetDirectoryPath);
        }

        /// <summary>
        /// Moves the specified file to the specified directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        [CakeMethodAlias]
        public static void MoveFileToDirectory(this ICakeContext context, FilePath filePath, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFileToDirectory(context, filePath, targetDirectoryPath);
        }

        /// <summary>
        /// Moves the files matching the specified pattern to the specified directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        [CakeMethodAlias]
        public static void MoveFiles(this ICakeContext context, string pattern, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFiles(context, pattern, targetDirectoryPath);
        }

        /// <summary>
        /// Moves the specified files to the specified directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePaths">The file paths.</param>
        /// <param name="targetDirectoryPath">The target directory path.</param>
        [CakeMethodAlias]
        public static void MoveFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFiles(context, filePaths, targetDirectoryPath);
        }

        /// <summary>
        /// Moves the specified file to the specified directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="filePath">The file path.</param>
        /// <param name="targetFilePath">The target file path.</param>
        [CakeMethodAlias]
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
        public static void DeleteFile(this ICakeContext context, FilePath filePath)
        {
            FileDeleter.DeleteFile(context, filePath);
        }
    }
}
