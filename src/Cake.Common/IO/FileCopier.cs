// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
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
            if (!context.FileSystem.Exist(targetDirectoryPath))
            {
                const string format = "The directory '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, targetDirectoryPath.FullPath);
                throw new DirectoryNotFoundException(message);
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
            if (files.Count == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }
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

            var absoluteTargetDirectoryPath = targetDirectoryPath.MakeAbsolute(context.Environment);

            // Make sure the target directory exist.
            if (!context.FileSystem.Exist(absoluteTargetDirectoryPath))
            {
                const string format = "The directory '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, absoluteTargetDirectoryPath.FullPath);
                throw new DirectoryNotFoundException(message);
            }

            // Iterate all files and copy them.
            foreach (var filePath in filePaths)
            {
                CopyFileCore(context, filePath, absoluteTargetDirectoryPath.GetFilePath(filePath));
            }
        }

        private static void CopyFileCore(ICakeContext context, FilePath filePath, FilePath targetFilePath)
        {
            var absoluteFilePath = filePath.MakeAbsolute(context.Environment);

            // Get the file.
            if (!context.FileSystem.Exist(absoluteFilePath))
            {
                const string format = "The file '{0}' do not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, absoluteFilePath.FullPath);
                throw new FileNotFoundException(message, absoluteFilePath.FullPath);
            }

            // Copy the file.
            var absoluteTargetPath = targetFilePath.MakeAbsolute(context.Environment);
            context.Log.Verbose("Copying file {0} to {1}", absoluteFilePath.GetFilename(), absoluteTargetPath);
            var file = context.FileSystem.GetFile(absoluteFilePath);
            file.Copy(absoluteTargetPath, true);
        }
    }
}
