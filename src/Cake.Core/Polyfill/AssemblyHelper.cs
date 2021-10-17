// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Cake.Core.IO;
using Cake.Core.Reflection;

namespace Cake.Core.Polyfill
{
    internal static class AssemblyHelper
    {
        public static Assembly GetExecutingAssembly()
        {
            return Assembly.GetExecutingAssembly();
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

            try
            {
                if (fileSystem.GetFile(path).IsClrAssembly())
                {
                    return Assembly.LoadFrom(path.FullPath);
                }

                if (environment.Platform.IsUnix())
                {
                    LoadUnixLibrary(path.FullPath, RTLD_NOW);
                }
                else
                {
                    LoadWindowsLibrary(path.FullPath);
                }

                return null;
            }
            catch (System.IO.FileLoadException)
            {
                // TODO: LOG
                return null;
            }
        }

#pragma warning disable SA1310 // Field names should not contain underscore
        private const int RTLD_NOW = 0x002;
#pragma warning restore SA1310 // Field names should not contain underscore

        [DllImport("libdl", EntryPoint = "dlopen")]
        private static extern IntPtr LoadUnixLibrary(string path, int flags);

        [DllImport("kernel32", EntryPoint = "LoadLibrary")]
        private static extern IntPtr LoadWindowsLibrary(string path);
    }
}