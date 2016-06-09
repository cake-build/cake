// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Common.IO.Paths;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    /// <summary>
    /// Contains extension methods for working with directories.
    /// </summary>
    [CakeAliasCategory("Directory Operations")]
    public static class DirectoryAliases
    {
        /// <summary>
        /// Gets a directory path from string.
        /// </summary>
        /// <example>
        /// <code>
        /// // Get the temp directory.
        /// var root = Directory("./");
        /// var temp = root + Directory("temp");
        ///
        /// // Clean the directory.
        /// CleanDirectory(temp);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="path">The path.</param>
        /// <returns>A directory path.</returns>
        [CakeMethodAlias]
        [CakeNamespaceImport("Cake.Common.IO.Paths")]
        public static ConvertableDirectoryPath Directory(this ICakeContext context, string path)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }
            return new ConvertableDirectoryPath(new DirectoryPath(path));
        }

        /// <summary>
        /// Deletes the specified directories.
        /// </summary>
        /// <example>
        /// <code>
        /// var directoriesToDelete = new DirectoryPath[]{
        ///     Directory("be"),
        ///     Directory("gone")
        /// };
        /// DeleteDirectories(directoriesToDelete, recursive:true);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="directories">The directory paths.</param>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Delete")]
        public static void DeleteDirectories(this ICakeContext context, IEnumerable<DirectoryPath> directories, bool recursive = false)
        {
            if (directories == null)
            {
                throw new ArgumentNullException("directories");
            }

            foreach (var directory in directories)
            {
                DeleteDirectory(context, directory, recursive);
            }
        }

        /// <summary>
        /// Deletes the specified directories.
        /// </summary>
        /// <example>
        /// <code>
        /// var directoriesToDelete = new []{
        ///     "be",
        ///     "gone"
        /// };
        /// DeleteDirectories(directoriesToDelete, recursive:true);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="directories">The directory paths.</param>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Delete")]
        public static void DeleteDirectories(this ICakeContext context, IEnumerable<string> directories, bool recursive = false)
        {
            if (directories == null)
            {
                throw new ArgumentNullException("directories");
            }

            var paths = directories.Select(p => new DirectoryPath(p));
            foreach (var directory in paths)
            {
                DeleteDirectory(context, directory, recursive);
            }
        }

        /// <summary>
        /// Deletes the specified directory.
        /// </summary>
        /// <example>
        /// <code>
        /// DeleteDirectory("./be/gone", recursive:true);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="path">The directory path.</param>
        /// <param name="recursive">Will perform a recursive delete if set to <c>true</c>.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Delete")]
        public static void DeleteDirectory(this ICakeContext context, DirectoryPath path, bool recursive = false)
        {
            DirectoryDeleter.Delete(context, path, recursive);
        }

        /// <summary>
        /// Cleans the directories matching the specified pattern.
        /// Cleaning the directory will remove all it's content but not the directory itself.
        /// </summary>
        /// <example>
        /// <code>
        /// CleanDirectories("./src/**/bin/debug");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern to match.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        public static void CleanDirectories(this ICakeContext context, string pattern)
        {
            var directories = context.GetDirectories(pattern);
            if (directories.Count == 0)
            {
                context.Log.Verbose("The provided pattern did not match any directories.");
                return;
            }
            CleanDirectories(context, directories);
        }

        /// <summary>
        /// Cleans the directories matching the specified pattern.
        /// Cleaning the directory will remove all it's content but not the directory itself.
        /// </summary>
        /// <example>
        /// <code>
        /// Func&lt;IFileSystemInfo, bool&gt; exclude_node_modules =
        /// fileSystemInfo=>!fileSystemInfo.Path.FullPath.EndsWith(
        ///                 "node_modules",
        ///                 StringComparison.OrdinalIgnoreCase);
        /// CleanDirectories("./src/**/bin/debug", exclude_node_modules);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern to match.</param>
        /// <param name="predicate">The predicate used to filter directories based on file system information.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        public static void CleanDirectories(this ICakeContext context, string pattern, Func<IFileSystemInfo, bool> predicate)
        {
            var directories = context.GetDirectories(pattern, predicate);
            if (directories.Count == 0)
            {
                context.Log.Verbose("The provided pattern did not match any directories.");
                return;
            }
            CleanDirectories(context, directories);
        }

        /// <summary>
        /// Cleans the specified directories.
        /// Cleaning a directory will remove all it's content but not the directory itself.
        /// </summary>
        /// <example>
        /// <code>
        /// var directoriesToClean = GetDirectories("./src/**/bin/");
        /// CleanDirectories(directoriesToClean);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="directories">The directory paths.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        public static void CleanDirectories(this ICakeContext context, IEnumerable<DirectoryPath> directories)
        {
            if (directories == null)
            {
                throw new ArgumentNullException("directories");
            }
            foreach (var directory in directories)
            {
                CleanDirectory(context, directory);
            }
        }

        /// <summary>
        /// Cleans the specified directories.
        /// Cleaning a directory will remove all it's content but not the directory itself.
        /// </summary>
        /// <example>
        /// <code>
        /// var directoriesToClean = new []{
        ///     "./src/Cake/obj",
        ///     "./src/Cake.Common/obj"
        /// };
        /// CleanDirectories(directoriesToClean);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="directories">The directory paths.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        public static void CleanDirectories(this ICakeContext context, IEnumerable<string> directories)
        {
            if (directories == null)
            {
                throw new ArgumentNullException("directories");
            }
            var paths = directories.Select(p => new DirectoryPath(p));
            foreach (var directory in paths)
            {
                CleanDirectory(context, directory);
            }
        }

        /// <summary>
        /// Cleans the specified directory.
        /// </summary>
        /// <example>
        /// <code>
        /// CleanDirectory("./src/Cake.Common/obj");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="path">The directory path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        public static void CleanDirectory(this ICakeContext context, DirectoryPath path)
        {
            DirectoryCleaner.Clean(context, path);
        }

        /// <summary>
        /// Cleans the specified directory.
        /// </summary>
        /// <example>
        /// <code>
        /// CleanDirectory("./src/Cake.Common/obj", fileSystemInfo=>!fileSystemInfo.Hidden);
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="path">The directory path.</param>
        /// <param name="predicate">Predicate used to determine which files/directories should get deleted.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        public static void CleanDirectory(this ICakeContext context, DirectoryPath path, Func<IFileSystemInfo, bool> predicate)
        {
            DirectoryCleaner.Clean(context, path, predicate);
        }

        /// <summary>
        /// Creates the specified directory.
        /// </summary>
        /// <example>
        /// <code>
        /// CreateDirectory("publish");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="path">The directory path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Create")]
        public static void CreateDirectory(this ICakeContext context, DirectoryPath path)
        {
            DirectoryCreator.Create(context, path);
        }

        /// <summary>
        /// Creates the specified directory if it does not exist.
        /// </summary>
        /// <example>
        /// <code>
        /// EnsureDirectoryExists("publish");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="path">The directory path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Exists")]
        public static void EnsureDirectoryExists(this ICakeContext context, DirectoryPath path)
        {
            if (!DirectoryExists(context, path))
            {
                CreateDirectory(context, path);
            }
        }

        /// <summary>
        /// Copies the contents of a directory to the specified location.
        /// </summary>
        /// <example>
        /// <code>
        /// CopyDirectory("source_path", "destination_path");
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="source">The source directory path.</param>
        /// <param name="destination">The destination directory path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Copy")]
        public static void CopyDirectory(this ICakeContext context, DirectoryPath source, DirectoryPath destination)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (source == null)
            {
                throw new ArgumentNullException("source");
            }

            if (destination == null)
            {
                throw new ArgumentNullException("destination");
            }

            if (source.IsRelative)
            {
                source = source.MakeAbsolute(context.Environment);
            }

            // Get the subdirectories for the specified directory.
            var sourceDir = context.FileSystem.GetDirectory(source);
            if (!sourceDir.Exists)
            {
                throw new System.IO.DirectoryNotFoundException(
                    "Source directory does not exist or could not be found: "
                    + source.FullPath);
            }

            var dirs = sourceDir.GetDirectories("*", SearchScope.Current);

            var destinationDir = context.FileSystem.GetDirectory(destination);
            if (!destinationDir.Exists)
            {
                destinationDir.Create();
            }

            // Get the files in the directory and copy them to the new location.
            var files = sourceDir.GetFiles("*", SearchScope.Current);
            foreach (var file in files)
            {
                var temppath = destinationDir.Path.CombineWithFilePath(file.Path.GetFilename());
                file.Copy(temppath, true);
            }

            // Copy all of the subdirectories
            foreach (var subdir in dirs)
            {
                var temppath = destination.Combine(subdir.Path.GetDirectoryName());
                CopyDirectory(context, subdir.Path, temppath);
            }
        }

        /// <summary>
        /// Determines whether the given path refers to an existing directory.
        /// </summary>
        /// <example>
        /// <code>
        /// var dir = "publish";
        /// if (!DirectoryExists(dir))
        /// {
        ///     CreateDirectory(dir);
        /// }
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="path">The <see cref="DirectoryPath"/> to check.</param>
        /// <returns><c>true</c> if <paramref name="path"/> refers to an existing directory;
        /// <c>false</c> if the directory does not exist or an error occurs when trying to
        /// determine if the specified path exists.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Exists")]
        public static bool DirectoryExists(this ICakeContext context, DirectoryPath path)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return context.FileSystem.GetDirectory(path).Exists;
        }

        /// <summary>
        /// Makes the path absolute (if relative) using the current working directory.
        /// </summary>
        /// <example>
        /// <code>
        /// var path = MakeAbsolute(Directory("./resources"));
        /// </code>
        /// </example>
        /// <param name="context">The context.</param>
        /// <param name="path">The path.</param>
        /// <returns>An absolute directory path.</returns>
        [CakeMethodAlias]
        [CakeAliasCategory("Path")]
        public static DirectoryPath MakeAbsolute(this ICakeContext context, DirectoryPath path)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }

            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            return path.MakeAbsolute(context.Environment);
        }
    }
}
