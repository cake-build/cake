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
    internal static class FileMover
    {
        public static void MoveFileToDirectory(ICakeContext context, FilePath filePath, DirectoryPath targetDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (targetDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(targetDirectoryPath));
            }
            MoveFile(context, filePath, targetDirectoryPath.GetFilePath(filePath));
        }

        public static void MoveFiles(ICakeContext context, GlobPattern pattern, DirectoryPath targetDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (pattern == null)
            {
                throw new ArgumentNullException(nameof(pattern));
            }

            var files = context.GetFiles(pattern);
            if (files.Count == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }

            MoveFiles(context, files, targetDirectoryPath);
        }

        public static void MoveFiles(this ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (filePaths == null)
            {
                throw new ArgumentNullException(nameof(filePaths));
            }
            if (targetDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(targetDirectoryPath));
            }

            targetDirectoryPath = targetDirectoryPath.MakeAbsolute(context.Environment);

            // Make sure the target directory exist.
            if (!context.FileSystem.Exist(targetDirectoryPath))
            {
                const string format = "The directory '{0}' does not exist.";
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
                throw new ArgumentNullException(nameof(context));
            }
            if (filePath == null)
            {
                throw new ArgumentNullException(nameof(filePath));
            }
            if (targetFilePath == null)
            {
                throw new ArgumentNullException(nameof(targetFilePath));
            }

            filePath = filePath.MakeAbsolute(context.Environment);
            targetFilePath = targetFilePath.MakeAbsolute(context.Environment);

            // Make sure the target directory exist.
            var targetDirectoryPath = targetFilePath.GetDirectory().MakeAbsolute(context.Environment);
            if (!context.FileSystem.Exist(targetDirectoryPath))
            {
                const string format = "The directory '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, targetDirectoryPath.FullPath);
                throw new DirectoryNotFoundException(message);
            }

            // Get the file and verify it exist.
            var file = context.FileSystem.GetFile(filePath);
            if (!file.Exists)
            {
                const string format = "The file '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, filePath.FullPath);
                throw new FileNotFoundException(message, filePath.FullPath);
            }

            // Move the file.
            context.Log.Verbose("Moving file {0} to {1}", filePath.GetFilename(), targetFilePath);
            file.Move(targetFilePath.MakeAbsolute(context.Environment));
        }
    }
}