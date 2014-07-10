using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    public static class FileExtensions
    {
        [CakeMethodAlias]
        public static void CopyFileToDirectory(this ICakeContext context, FilePath filePath, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFileToDirectory(context, filePath, targetDirectoryPath);
        }

        [CakeMethodAlias]
        public static void CopyFile(this ICakeContext context, FilePath filePath, FilePath targetFilePath)
        {
            FileCopier.CopyFile(context, filePath, targetFilePath);
        }

        [CakeMethodAlias]
        public static void CopyFiles(this ICakeContext context, string pattern, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFiles(context, pattern, targetDirectoryPath);
        }

        [CakeMethodAlias]
        public static void CopyFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
        {
            FileCopier.CopyFiles(context, filePaths, targetDirectoryPath);
        }

        [CakeMethodAlias]
        public static void MoveFileToDirectory(this ICakeContext context, FilePath filePath, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFileToDirectory(context, filePath, targetDirectoryPath);
        }

        [CakeMethodAlias]
        public static void MoveFiles(this ICakeContext context, string pattern, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFiles(context, pattern, targetDirectoryPath);
        }

        [CakeMethodAlias]
        public static void MoveFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
        {
            FileMover.MoveFiles(context, filePaths, targetDirectoryPath);
        }

        [CakeMethodAlias]
        public static void MoveFile(this ICakeContext context, FilePath filePath, FilePath targetFilePath)
        {
            FileMover.MoveFile(context, filePath, targetFilePath);
        }

        [CakeMethodAlias]
        public static void DeleteFiles(this ICakeContext context, string pattern)
        {
            FileDeleter.DeleteFiles(context, pattern);
        }

        [CakeMethodAlias]
        public static void DeleteFiles(this ICakeContext context, IEnumerable<FilePath> filePaths)
        {
            FileDeleter.DeleteFiles(context, filePaths);
        }

        [CakeMethodAlias]
        public static void DeleteFile(this ICakeContext context, FilePath filePath)
        {
            FileDeleter.DeleteFile(context, filePath);
        }
    }
}
