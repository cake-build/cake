// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using Cake.Core.Diagnostics;
using Cake.Core.IO;

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

        public static Assembly LoadAssembly(ICakeEnvironment environment, IFileSystem fileSystem, ICakeLog log, FilePath path)
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

                if (_useLoadMacOsLibrary)
                {
                    LoadUnixLibrary(LoadMacOSLibrary, path);
                    LogNativeLoad(log, nameof(LoadMacOSLibrary), path);
                }
                else if (_useLoadUnixLibrary1)
                {
                    LoadUnixLibrary(LoadUnixLibrary1, path);
                    LogNativeLoad(log, nameof(LoadUnixLibrary1), path);
                }
                else if (_useLoadUnixLibrary2)
                {
                    LoadUnixLibrary(LoadUnixLibrary2, path);
                    LogNativeLoad(log, nameof(LoadUnixLibrary2), path);
                }
                else if (environment.Platform.IsUnix())
                {
                    if (environment.Platform.IsOSX() && TryLoadUnixLibrary(LoadMacOSLibrary, path, out _useLoadMacOsLibrary))
                    {
                        LogNativeLoad(log, nameof(LoadMacOSLibrary), path);
                        return null;
                    }

                    if (TryLoadUnixLibrary(LoadUnixLibrary2, path, out _useLoadUnixLibrary2))
                    {
                        LogNativeLoad(log, nameof(LoadUnixLibrary2), path);
                        return null;
                    }

                    TryLoadUnixLibrary(LoadUnixLibrary1, path, out _useLoadUnixLibrary1);
                    LogNativeLoad(log, nameof(LoadUnixLibrary1), path);
                }
                else
                {
                    LoadWindowsLibrary(path.FullPath);
                }

                return null;
            }
            catch (System.IO.FileLoadException ex)
            {
                log.Debug(Verbosity.Diagnostic,
                    logAction => logAction("Caught error while loading {0}\r\n{1}",
                        path.FullPath,
                        ex));
                return null;
            }
        }

        private static void LogNativeLoad(ICakeLog log, string loadUnixLibrary, FilePath path)
        {
            log.Debug(Verbosity.Diagnostic,
                logAction => logAction("Native {0}: {1}",
                    loadUnixLibrary,
                    path.FullPath));
        }

#pragma warning disable SA1310 // Field names should not contain underscore
        private const int RTLD_NOW = 0x002;
#pragma warning restore SA1310 // Field names should not contain underscore
#pragma warning disable SA1303 // Field names should start with uppercase
        private const string dlopen = nameof(dlopen);
#pragma warning restore SA1303 // Field names should start with uppercase

        private static bool _useLoadUnixLibrary1;
        private static bool _useLoadUnixLibrary2;
        private static bool _useLoadMacOsLibrary;

        [DllImport("libdl.so", EntryPoint = dlopen)]
        private static extern IntPtr LoadUnixLibrary1(string path, int flags);

        [DllImport("libdl.so.2", EntryPoint = dlopen)]
        private static extern IntPtr LoadUnixLibrary2(string path, int flags);

        [DllImport("/usr/lib/libSystem.dylib", EntryPoint = dlopen)]
        private static extern IntPtr LoadMacOSLibrary(string path, int flags);

        [DllImport("kernel32", EntryPoint = "LoadLibrary")]
        private static extern IntPtr LoadWindowsLibrary(string path);

        private static void LoadUnixLibrary(Func<string, int, IntPtr> dlOpen, FilePath path) => dlOpen(path.FullPath, RTLD_NOW);

        private static bool TryLoadUnixLibrary(Func<string, int, IntPtr> dlOpen, FilePath path, out bool result)
        {
            try
            {
                LoadUnixLibrary(dlOpen, path);
                return result = true;
            }
            catch (DllNotFoundException)
            {
                return result = false;
            }
        }
    }
}