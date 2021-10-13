// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using System.Linq;
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
            CopyFile(context, filePath, targetDirectoryPath.GetFilePath(filePath));
        }

        public static void CopyFile(ICakeContext context, FilePath filePath, FilePath targetFilePath)
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

            var targetDirectoryPath = targetFilePath.GetDirectory().MakeAbsolute(context.Environment);

            // Make sure the target directory exist.
            if (!context.FileSystem.Exist(targetDirectoryPath))
            {
                const string format = "The directory '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, targetDirectoryPath.FullPath);
                throw new DirectoryNotFoundException(message);
            }

            CopyFileCore(context, filePath, targetFilePath, null);
        }

        public static void CopyFiles(ICakeContext context, GlobPattern pattern, DirectoryPath targetDirectoryPath, bool preserverFolderStructure)
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

            CopyFiles(context, files, targetDirectoryPath, preserverFolderStructure);
        }

        public static void CopyFiles(ICakeContext context, IEnumerable<FilePath> filePaths, DirectoryPath targetDirectoryPath, bool preserverFolderStructure)
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

            // Make all path absolute
            var absoluteTargetDirectoryPath = targetDirectoryPath.MakeAbsolute(context.Environment);

            var absoluteFilePaths = filePaths.Select(x => x.MakeAbsolute(context.Environment)).ToList();

            // Make sure the target directory exist.
            if (!context.FileSystem.Exist(absoluteTargetDirectoryPath))
            {
                const string format = "The directory '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, absoluteTargetDirectoryPath.FullPath);
                throw new DirectoryNotFoundException(message);
            }

            if (preserverFolderStructure)
            {
                var commonPath = string.Empty;
                var separatedPath = absoluteFilePaths
                    .First(str => str.ToString().Length == absoluteFilePaths.Max(st2 => st2.ToString().Length)).ToString()
                    .Split(new[] { "/" }, StringSplitOptions.RemoveEmptyEntries)
                    .ToList();

                foreach (string pathSegment in separatedPath)
                {
                    if (commonPath.Length == 0 && absoluteFilePaths.All(str => str.ToString().StartsWith(pathSegment)))
                    {
                        commonPath = pathSegment;
                    }
                    else if (absoluteFilePaths.All(str => str.ToString().StartsWith(commonPath + "/" + pathSegment)))
                    {
                        commonPath += "/" + pathSegment;
                    }
                    else
                    {
                        break;
                    }
                }

                if (absoluteFilePaths.Count == 1 && absoluteFilePaths.First().FullPath.Contains(context.Environment.WorkingDirectory.FullPath))
                {
                    var relativePath = absoluteFilePaths.First().FullPath.Remove(0, context.Environment.WorkingDirectory.FullPath.Length + 1);
                    var relativePathParts = relativePath.Split('/').ToList();

                    if (relativePathParts.Count > 2)
                    {
                        relativePathParts.RemoveAt(0);
                        var workdirRelativeStructurePath = string.Join("/", relativePathParts.ToArray());

                        var index = commonPath.IndexOf(workdirRelativeStructurePath, StringComparison.Ordinal);
                        commonPath = index < 0
                            ? commonPath
                            : commonPath.Remove(index, workdirRelativeStructurePath.Length);
                    }
                }

                // Iterate all files and copy them.
                foreach (var filePath in absoluteFilePaths)
                {
                    CopyFileCore(context, filePath, absoluteTargetDirectoryPath.GetFilePath(filePath), context.DirectoryExists(commonPath) ? commonPath : null);
                }
            }
            else
            {
                // #1663: For empty enumerations, just return.
                if (!absoluteFilePaths.Any())
                {
                    return;
                }

                // Iterate all files and copy them.
                foreach (var filePath in absoluteFilePaths)
                {
                    CopyFileCore(context, filePath, absoluteTargetDirectoryPath.GetFilePath(filePath), null);
                }
            }
        }

        private static void CopyFileCore(ICakeContext context, FilePath filePath, FilePath targetFilePath, string commonPath)
        {
            var absoluteFilePath = filePath.MakeAbsolute(context.Environment);

            // Get the file.
            if (!context.FileSystem.Exist(absoluteFilePath))
            {
                const string format = "The file '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, absoluteFilePath.FullPath);
                throw new FileNotFoundException(message, absoluteFilePath.FullPath);
            }

            // Copy the file.
            var absoluteTargetPath = targetFilePath.MakeAbsolute(context.Environment);
            var file = context.FileSystem.GetFile(absoluteFilePath);

            if (!string.IsNullOrEmpty(commonPath))
            {
                // Get the parent folder structure and create it.
                var newRelativeFolderPath = context.Directory(commonPath).Path.GetRelativePath(filePath.GetDirectory());
                var newTargetPath = targetFilePath.GetDirectory().Combine(newRelativeFolderPath);
                var newAbsoluteTargetPath = newTargetPath.CombineWithFilePath(filePath.GetFilename());
                context.Log.Verbose("Copying file {0} to {1}", absoluteFilePath.GetFilename(), newAbsoluteTargetPath);

                if (!context.DirectoryExists(newTargetPath))
                {
                    context.CreateDirectory(newTargetPath);
                }

                file.Copy(newAbsoluteTargetPath, true);
            }
            else
            {
                context.Log.Verbose("Copying file {0} to {1}", absoluteFilePath.GetFilename(), absoluteTargetPath);
                file.Copy(absoluteTargetPath, true);
            }
        }
    }
}
