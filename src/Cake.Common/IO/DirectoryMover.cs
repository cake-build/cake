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
    internal static class DirectoryMover
    {
        public static void MoveDirectory(ICakeContext context, DirectoryPath directoryPath, DirectoryPath targetDirectoryPath)
        {
            if (context == null)
            {
                throw new ArgumentNullException(nameof(context));
            }
            if (directoryPath == null)
            {
                throw new ArgumentNullException(nameof(directoryPath));
            }
            if (targetDirectoryPath == null)
            {
                throw new ArgumentNullException(nameof(targetDirectoryPath));
            }

            directoryPath = directoryPath.MakeAbsolute(context.Environment);
            targetDirectoryPath = targetDirectoryPath.MakeAbsolute(context.Environment);

            // Get the directory and verify it exist.
            var directory = context.FileSystem.GetDirectory(directoryPath);
            if (!directory.Exists)
            {
                const string format = "The directory '{0}' does not exist.";
                var message = string.Format(CultureInfo.InvariantCulture, format, directoryPath.FullPath);
                throw new DirectoryNotFoundException(message);
            }

            // Move the directory.
            context.Log.Verbose("Moving directory {0} to {1}", directoryPath.GetDirectoryName(), targetDirectoryPath);
            directory.Move(targetDirectoryPath);
        }
    }
}