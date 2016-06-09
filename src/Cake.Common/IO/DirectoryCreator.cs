// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

namespace Cake.Common.IO
{
    internal static class DirectoryCreator
    {
        public static void Create(ICakeContext context, DirectoryPath path)
        {
            if (context == null)
            {
                throw new ArgumentNullException("context");
            }
            if (path == null)
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
                context.Log.Verbose("Creating directory {0}", path);
                directory.Create();
            }
        }
    }
}
