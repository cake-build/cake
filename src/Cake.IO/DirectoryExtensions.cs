using Cake.Core;
using System.Collections.Generic;
using Cake.Core.IO;

namespace Cake.IO
{
    public static class DirectoryExtensions
    {
        public static void DeleteDirectories(this ICakeContext context, IEnumerable<DirectoryPath> directories, bool recursive = true)
        {
            foreach (var directory in directories)
            {
                DeleteDirectory(context, directory, recursive);
            }
        }

        public static void DeleteDirectory(this ICakeContext context, DirectoryPath path, bool recursive = false)
        {
            DirectoryDeleter.Delete(context, path, recursive);
        }

        public static void CleanDirectories(this ICakeContext context, string pattern)
        {
            var directories = context.GetDirectories(pattern);
            CleanDirectories(context, directories);
        }

        public static void CleanDirectories(this ICakeContext context, IEnumerable<DirectoryPath> directories)
        {
            foreach (var directory in directories)
            {
                CleanDirectory(context, directory);
            }
        }

        public static void CleanDirectory(this ICakeContext context, DirectoryPath path)
        {
            DirectoryCleaner.Clean(context, path);
        }
    }
}
