using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Annotations;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    public static class DirectoryExtensions
    {
        [CakeMethodAlias]
        public static void DeleteDirectories(this ICakeContext context, IEnumerable<DirectoryPath> directories, bool recursive = true)
        {
            foreach (var directory in directories)
            {
                DeleteDirectory(context, directory, recursive);
            }
        }

        [CakeMethodAlias]
        public static void DeleteDirectory(this ICakeContext context, DirectoryPath path, bool recursive = false)
        {
            DirectoryDeleter.Delete(context, path, recursive);
        }

        [CakeMethodAlias]
        public static void CleanDirectories(this ICakeContext context, string pattern)
        {
            var directories = context.GetDirectories(pattern);
            CleanDirectories(context, directories);
        }

        [CakeMethodAlias]
        public static void CleanDirectories(this ICakeContext context, IEnumerable<DirectoryPath> directories)
        {
            foreach (var directory in directories)
            {
                CleanDirectory(context, directory);
            }
        }

        [CakeMethodAlias]
        public static void CleanDirectory(this ICakeContext context, DirectoryPath path)
        {
            DirectoryCleaner.Clean(context, path);
        }

        [CakeMethodAlias]
        public static void CreateDirectory(this ICakeContext context, DirectoryPath path)
        {
            DirectoryCreator.Create(context, path);
        }
    }
}
