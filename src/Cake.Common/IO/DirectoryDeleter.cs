using System.Globalization;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using System;

namespace Cake.Common.IO
{
    internal static class DirectoryDeleter
    {
        public static void Delete(ICakeContext context, DirectoryPath path, bool recursive)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if(path == null)
            {
                throw new ArgumentNullException("path");
            }

            if (path.IsRelative)
            {
                path = path.MakeAbsolute(context.Environment);
            }

            var directory = context.FileSystem.GetDirectory(path);
            if (!directory.Exists)
            {
                const string format = "The directory '{0}' do not exist.";
                throw new IOException(string.Format(CultureInfo.InvariantCulture, format, path.FullPath));
            }

            var hasDirectories = directory.GetDirectories("*", SearchScope.Current).Any();
            var hasFiles = directory.GetFiles("*", SearchScope.Current).Any();
            if (!recursive && (hasDirectories || hasFiles))
            {
                const string format = "Cannot delete directory '{0}' without recursion since it's not empty.";
                throw new IOException(string.Format(CultureInfo.InvariantCulture, format, path.FullPath));
            }

            context.Log.Verbose("Deleting {0}", path);           
            directory.Delete(recursive);            
        }
    }
}
