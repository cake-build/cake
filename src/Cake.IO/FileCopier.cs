using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.IO
{
    internal static class FileCopier
    {
        public static void CopyFileToDirectory(ICakeContext context, FilePath filePath, DirectoryPath targetDirectoryPath)
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
            CopyFile(context, filePath, targetDirectoryPath.GetFilePath(filePath));
        }

        public static void CopyFile(ICakeContext context, FilePath filePath, FilePath targetFilePath)
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

            var targetDirectoryPath = targetFilePath.GetDirectory().MakeAbsolute(context.Environment);

            // Make sure the target directory exist.
            var targetDirectory = context.FileSystem.GetDirectory(targetDirectoryPath);
            if (targetDirectory == null || !targetDirectory.Exists)
            {
                const string format = "The directory '{0}' do not exist.";
                throw new DirectoryNotFoundException(string.Format(format, targetDirectoryPath.FullPath));
            }

            CopyFileCore(context, filePath, targetFilePath);
        }

        public static void CopyFiles(ICakeContext context, string pattern, DirectoryPath targetDirectoryPath)
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
            CopyFiles(context, files, targetDirectoryPath);
        }


        public static void CopyFiles(ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
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

            // Make sure the target directory exist.
            var absolutTargetDirectoryPath = targetDirectoryPath.MakeAbsolute(context.Environment);
            var targetDirectory = context.FileSystem.GetDirectory(absolutTargetDirectoryPath);
            if (targetDirectory == null || !targetDirectory.Exists)
            {
                const string format = "The directory '{0}' do not exist.";
                throw new DirectoryNotFoundException(string.Format(format, absolutTargetDirectoryPath.FullPath));
            }

            // Iterate all files and copy them.
            foreach (var filePath in filePaths)
            {
                CopyFileCore(context, filePath, absolutTargetDirectoryPath.GetFilePath(filePath));
            }
        }

        private static void CopyFileCore(ICakeContext context, FilePath filePath, FilePath targetFilePath)
        {
            var absolutFilePath = filePath.MakeAbsolute(context.Environment);

            // Get the file.
            var file = context.FileSystem.GetFile(absolutFilePath);
            if (file == null || !file.Exists)
            {
                const string format = "The file '{0}' do not exist.";
                throw new FileNotFoundException(string.Format(format, absolutFilePath.FullPath),
                    absolutFilePath.FullPath);
            }

            // Copy the file.
            file.Copy(targetFilePath.MakeAbsolute(context.Environment));
        } 
    }
}
