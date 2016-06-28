/////////////////////////////////////////////////////////////////////////////////////////////////////
// This code was taken and adapted from the MSBuild project.
// https://github.com/Microsoft/msbuild/blob/xplat/src/Shared/FileUtilities.GetFolderPath.cs
//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
/////////////////////////////////////////////////////////////////////////////////////////////////////

#if NETCORE
using System;
using System.Collections.Generic;
using System.Runtime.InteropServices;
using System.Text;
using Cake.Core.IO;

namespace Cake.Core.Polyfill
{
    internal static partial class Native
    {
        public static class Windows
        {
            private static readonly Dictionary<SpecialPath, int> _lookup = new Dictionary<SpecialPath, int>
            {
                { SpecialPath.ApplicationData, 0x001a },
                { SpecialPath.CommonApplicationData, 0x0023 },
                { SpecialPath.LocalApplicationData, 0x001c },
                { SpecialPath.ProgramFiles, 0x0026 },
                { SpecialPath.ProgramFilesX86, 0x002a },
                { SpecialPath.Windows, 0x0024 }
            };

            [DllImport("shell32.dll", CharSet = CharSet.Unicode, BestFitMapping = false)]
            public static extern int SHGetFolderPath(IntPtr hwndOwner, int nFolder, IntPtr hToken, int dwFlags, [Out]StringBuilder lpszPath);

            public static string GetFolder(SpecialPath folder)
            {
                if (!_lookup.ContainsKey(folder))
                {
                    return null;
                }

                var builder = new StringBuilder(260);
                var result = SHGetFolderPath(IntPtr.Zero, _lookup[folder], IntPtr.Zero, 0, builder);
                if (result < 0)
                {
                    if (result == unchecked((int)0x80131539))
                    {
                        throw new PlatformNotSupportedException();
                    }
                    return null;
                }

                return builder.ToString();
            }
        }
    }
}
#endif