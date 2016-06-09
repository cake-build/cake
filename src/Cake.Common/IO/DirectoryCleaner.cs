// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    internal static class DirectoryCleaner
    {
        public static void Clean(ICakeContext context, DirectoryPath path)
        {
            Clean(context, path, null);
        }

        public static void Clean(ICakeContext context, DirectoryPath path, Func<IFileSystemInfo, bool> predicate)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

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
            CleanDirectory(root, predicate, 0);
        }

        private static bool CleanDirectory(IDirectory root, Func<IFileSystemInfo, bool> predicate, int level)
        {
            var shouldDeleteRoot = predicate(root);

            // Delete all child directories.
            var directories = root.GetDirectories("*", SearchScope.Current);
            foreach (var directory in directories)
            {
                if (!CleanDirectory(directory, predicate, level + 1))
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
                if (predicate(file))
                {
                    file.Delete();
                }
                else
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
    }
}
