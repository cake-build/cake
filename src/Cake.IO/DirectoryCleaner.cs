using System;
using System.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.IO
{
    internal static class DirectoryCleaner
    {
        public static void Clean(ICakeContext context, DirectoryPath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException("path");
            }

            // Get the root directory.
            var root = context.FileSystem.GetDirectory(path);
            if (!root.Exists)
            {
                const string format = "The directory '{0}' do not exist.";
                throw new IOException(string.Format(format, root.Path));
            }

            context.Log.Verbose("Deleting contents of {0}", path);

            // Delete all files.
            foreach (var file in root.GetFiles("*", SearchScope.Recursive))
            {
                file.Delete();
            }

            // Delete all directories.
            foreach (var directory in root.GetDirectories("*", SearchScope.Recursive))
            {
                directory.Delete(false);
            }
        }
    }
}