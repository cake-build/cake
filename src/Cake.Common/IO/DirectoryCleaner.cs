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
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // Make path absolute.
            path = path.MakeAbsolute(context.Environment);

            // Get the root directory.
            var root = context.FileSystem.GetDirectory(path);
            if (!root.Exists)
            {
                context.Log.Verbose("Creating directory {0}", path);
                root.Create();
                return;
            }

            context.Log.Verbose("Deleting contents of {0}", path);

            // Delete all directories.
            foreach (var directory in root.GetDirectories("*", SearchScope.Current))
            {
                Clean(context, directory.Path.FullPath);
                directory.Delete(false);
            }

            // Delete all files.
            foreach (var file in root.GetFiles("*", SearchScope.Current))
            {
                file.Delete();
            }
        }
    }
}
