// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    internal static class DirectoryCleaner
    {
        public static void Clean(ICakeContext context, DirectoryPath path)
        {
            Clean(context, path, null, null);
        }

        public static void Clean(ICakeContext context, DirectoryPath path, CleanDirectorySettings settings)
        {
            Clean(context, path, null, settings);
        }

        public static void Clean(ICakeContext context, DirectoryPath path, Func<IFileSystemInfo, bool> predicate)
        {
            Clean(context, path, predicate, null);
        }

        public static void Clean(ICakeContext context, DirectoryPath path, Func<IFileSystemInfo, bool> predicate, CleanDirectorySettings settings)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(path);

            if (path.IsRelative)
            {
                path = path.MakeAbsolute(context.Environment);
            }

            // Get the root directory.
            var root = context.FileSystem.GetDirectory(path);
            if (!root.Exists)
            {
                context.Log.Verbose("Creating directory {0}", path);
                root.Create();
                return;
            }

            context.Log.Verbose("Cleaning directory {0}", path);
            predicate = predicate ?? (info => true);
            settings = settings ?? new CleanDirectorySettings();
            CleanDirectory(root, predicate, 0, settings);
        }

        private static bool CleanDirectory(IDirectory root, Func<IFileSystemInfo, bool> predicate, int level, CleanDirectorySettings settings)
        {
            var shouldDeleteRoot = predicate(root);

            // Delete all child directories.
            var directories = root.GetDirectories("*", SearchScope.Current);
            foreach (var directory in directories)
            {
                if (!directory.Exists)
                {
                    if (!TryDeleteDirectoryEntry(directory, predicate, level))
                    {
                        shouldDeleteRoot = false;
                    }

                    continue;
                }

                if (!TryCleanChildDirectory(directory, predicate, level + 1, settings))
                {
                    // Since the child directory reported it shouldn't be
                    // removed, we should not remove the current directory either.
                    shouldDeleteRoot = false;
                }
            }

            // Delete all files in the directory.
            var files = root.GetFiles("*", SearchScope.Current);
            foreach (var file in files)
            {
                if (!predicate(file))
                {
                    shouldDeleteRoot = false;
                    continue;
                }

                if (!file.Exists)
                {
                    if (!TryDeleteFileEntry(file))
                    {
                        shouldDeleteRoot = false;
                    }

                    continue;
                }

                if (settings.Force)
                {
                    // Remove the ReadOnly attribute on file (if set)
                    file.Attributes &= ~FileAttributes.ReadOnly;
                }

                try
                {
                    file.Delete();
                }
                catch (IOException)
                {
                    shouldDeleteRoot = false;
                }
            }

            // Should we delete current directory?
            // Make sure it's not the initial directory.
            if (shouldDeleteRoot && level > 0)
            {
                root.Delete(false);
                return true;
            }

            // We did not delete this directory.
            return false;
        }

        private static bool TryCleanChildDirectory(IDirectory directory, Func<IFileSystemInfo, bool> predicate, int level, CleanDirectorySettings settings)
        {
            try
            {
                return CleanDirectory(directory, predicate, level, settings);
            }
            catch (DirectoryNotFoundException)
            {
                return TryDeleteDirectoryEntry(directory, predicate, level);
            }
            catch (IOException)
            {
                return TryDeleteDirectoryEntry(directory, predicate, level);
            }
        }

        private static bool TryDeleteDirectoryEntry(IDirectory directory, Func<IFileSystemInfo, bool> predicate, int level)
        {
            if (!predicate(directory) || level <= 0)
            {
                return false;
            }

            try
            {
                directory.Delete(false);
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }

        private static bool TryDeleteFileEntry(IFile file)
        {
            try
            {
                file.Delete();
                return true;
            }
            catch (IOException)
            {
                return false;
            }
        }
    }
}