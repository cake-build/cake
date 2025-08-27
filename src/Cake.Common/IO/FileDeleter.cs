// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Collections.Generic;
using System.Globalization;
using System.IO;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    internal static class FileDeleter
    {
        public static void DeleteFiles(ICakeContext context, GlobPattern pattern)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(pattern);

            var files = context.GetFiles(pattern);
            if (files.Count == 0)
            {
                context.Log.Verbose("The provided pattern did not match any files.");
                return;
            }

            DeleteFiles(context, files);
        }

        public static void DeleteFiles(ICakeContext context, IEnumerable<FilePath> filePaths)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(filePaths);

            foreach (var filePath in filePaths)
            {
                DeleteFile(context, filePath);
            }
        }

        public static void DeleteFile(ICakeContext context, FilePath filePath)
        {
            ArgumentNullException.ThrowIfNull(context);
            ArgumentNullException.ThrowIfNull(filePath);

            filePath = filePath.MakeAbsolute(context.Environment);

            var file = context.FileSystem.GetFile(filePath);
            if (!file.Exists)
            {
                const string format = "The file '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, filePath.FullPath);
                throw new FileNotFoundException(message, filePath.FullPath);
            }

            context.Log.Verbose("Deleting file {0}", filePath);
            file.Delete();
        }
    }
}