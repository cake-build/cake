﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using Cake.Core.IO;
using Cake.Core.Reflection;

namespace Cake.Core.Polyfill
{
    internal static class AssemblyHelper
    {
        public static Assembly GetExecutingAssembly()
        {
            return typeof(CakeEnvironment).GetTypeInfo().Assembly;
        }

        public static Assembly LoadAssembly(AssemblyName assemblyName)
        {
            if (assemblyName == null)
            {
                throw new ArgumentNullException(nameof(assemblyName));
            }
            return Assembly.Load(assemblyName);
        }

        public static Assembly LoadAssembly(ICakeEnvironment environment, IFileSystem fileSystem, FilePath path)
        {
            if (path == null)
            {
                throw new ArgumentNullException(nameof(path));
            }

            if (path.Segments.Length == 1 && !fileSystem.Exist(path))
            {
                // Not a valid path. Try loading it by its name.
                return Assembly.Load(new AssemblyName(path.FullPath));
            }

            // Make the path absolute.
            path = path.MakeAbsolute(environment);

            var loader = new CakeAssemblyLoadContext(fileSystem, path.GetDirectory());
            return loader.LoadFromAssemblyPath(path.FullPath);
        }
    }
}
