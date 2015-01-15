using System;
using System.Collections.Generic;
using System.Linq;
using Cake.Core;
using Cake.Core.Annotations;
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
        /// Deletes the specified directories.
        /// </summary>
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
        /// Cleaning the directory will remove all it's content but not the directory iteself.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="pattern">The pattern to match.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        public static void CleanDirectories(this ICakeContext context, string pattern)
        {
            var directories = context.GetDirectories(pattern);
            CleanDirectories(context, directories);
        }

        /// <summary>
        /// Cleans the specified directories.
        /// Cleaning a directory will remove all it's content but not the directory iteself.
        /// </summary>
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
        /// Cleaning a directory will remove all it's content but not the directory iteself.
        /// </summary>
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
        /// <param name="context">The context.</param>
        /// <param name="path">The directory path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Clean")]
        public static void CleanDirectory(this ICakeContext context, DirectoryPath path)
        {
            DirectoryCleaner.Clean(context, path);
        }

        /// <summary>
        /// Creates the specified directory.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <param name="path">The directory path.</param>
        [CakeMethodAlias]
        [CakeAliasCategory("Create")]
        public static void CreateDirectory(this ICakeContext context, DirectoryPath path)
        {
            DirectoryCreator.Create(context, path);
        }

        /// <summary>
        /// Copies a directory to the specified location.
        /// </summary>
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
            // If the destination directory doesn't exist, create it. 
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
    }
}
