// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Globalization;
using System.IO;
using System.Linq;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    internal static class DirectoryDeleter
    {
        public static void Delete(ICakeContext context, DirectoryPath path, DeleteDirectorySettings settings)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.IsRelative)
            {
                path = path.MakeAbsolute(context.Environment);
            }

            var directory = context.FileSystem.GetDirectory(path);
            if (!directory.Exists)
            {
                const string format = "The directory '{0}' does not exist.";
                throw new IOException(string.Format(CultureInfo.InvariantCulture, format, path.FullPath));
            }

            var hasDirectories = directory.GetDirectories("*", SearchScope.Current).Any();
            var hasFiles = directory.GetFiles("*", SearchScope.Current).Any();
            if (!settings.Recursive && (hasDirectories || hasFiles))
            {
                const string format = "Cannot delete directory '{0}' without recursion since it's not empty.";
                throw new IOException(string.Format(CultureInfo.InvariantCulture, format, path.FullPath));
            }

            context.Log.Verbose("Deleting directory {0}", path);

            if (settings.Force)
            {
                context.Log.Verbose("Removing ReadOnly attributes on all files in directory {0}", path);
                var searchOption = settings.Recursive ? SearchScope.Recursive : SearchScope.Current;
                foreach (var file in directory.GetFiles("*", searchOption))
                {
                    file.Attributes &= ~FileAttributes.ReadOnly;
                }
            }

            directory.Delete(settings.Recursive);
        }
    }
}