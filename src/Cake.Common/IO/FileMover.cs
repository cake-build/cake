using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    internal static class FileMover
    {
        public static void MoveFileToDirectory(ICakeContext context, FilePath filePath, DirectoryPath targetDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }
            if (targetDirectoryPath == null)
            {
                throw new ArgumentNullException("targetDirectoryPath");
            }
            MoveFile(context, filePath, targetDirectoryPath.GetFilePath(filePath));
        }

        public static void MoveFiles(ICakeContext context, string pattern, DirectoryPath targetDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (pattern == null)
            {
                throw new ArgumentNullException("pattern");
            }
            var files = context.GetFiles(pattern);
            MoveFiles(context, files, targetDirectoryPath);
        }

        public static void MoveFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (filePaths == null)
            {
                throw new ArgumentNullException("filePaths");
            }
            if (targetDirectoryPath == null)
            {
                throw new ArgumentNullException("targetDirectoryPath");
            }

            targetDirectoryPath = targetDirectoryPath.MakeAbsolute(context.Environment);

            // Make sure the target directory exist.            
            if (!context.FileSystem.Exist(targetDirectoryPath))
            {
                const string format = "The directory '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, targetDirectoryPath.FullPath);
                throw new DirectoryNotFoundException(message);
            }

            // Iterate all files and copy them.
            foreach (var filePath in filePaths)
            {
                MoveFile(context, filePath, targetDirectoryPath.GetFilePath(filePath));
            }
        }

        public static void MoveFile(ICakeContext context, FilePath filePath, FilePath targetFilePath)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (filePath == null)
            {
                throw new ArgumentNullException("filePath");
            }
            if (targetFilePath == null)
            {
                throw new ArgumentNullException("targetFilePath");
            }

            filePath = filePath.MakeAbsolute(context.Environment);
            targetFilePath = targetFilePath.MakeAbsolute(context.Environment);

            // Make sure the target directory exist.            
            var targetDirectoryPath = targetFilePath.GetDirectory().MakeAbsolute(context.Environment);
            if (!context.FileSystem.Exist(targetDirectoryPath))
            {
                const string format = "The directory '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, targetDirectoryPath.FullPath);
                throw new DirectoryNotFoundException(message);
            }            

            // Get the file and verify it exist.
            var file = context.FileSystem.GetFile(filePath);
            if (!file.Exists)
            {
                const string format = "The file '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, filePath.FullPath);
                throw new FileNotFoundException(message ,filePath.FullPath);
            }

            // Move the file.            
            file.Move(targetFilePath.MakeAbsolute(context.Environment));
        }
    }
}
