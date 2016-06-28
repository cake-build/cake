/////////////////////////////////////////////////////////////////////////////////////////////////////
// This code was taken and adapted from the MSBuild project.
// https://github.com/Microsoft/msbuild/blob/xplat/src/Shared/FileUtilities.GetFolderPath.cs
//
// Copyright (c) Microsoft. All rights reserved.
// Licensed under the MIT license. See LICENSE file in the project root for full license information.
/////////////////////////////////////////////////////////////////////////////////////////////////////

#if NETCORE
using System;
using System.Runtime.InteropServices;
using System.Text;
using Cake.Core.IO;

namespace Cake.Core.Polyfill
{
    internal static partial class Native
    {
        public static class Unix
        {
            [DllImport("libc", SetLastError = true)]
            public static extern IntPtr getenv([MarshalAs(UnmanagedType.LPStr)] string name);

            public static string GetFolder(SpecialPath folder)
            {
                switch (folder)
                {
                    case SpecialPath.ProgramFiles:
                    case SpecialPath.ProgramFilesX86:
                        return "/user/bin";
                    case SpecialPath.LocalApplicationData:
                    case SpecialPath.ApplicationData:
                        var value = getenv("HOME");
                        if (value == IntPtr.Zero)
                        {
                            return null;
                        }

                        var size = 0;
                        while (Marshal.ReadByte(value, size) != 0)
                        {
                            size++;
                        }

                        if (size == 0)
                        {
                            var buffer = new byte[size];
                            Marshal.Copy(value, buffer, 0, size);
                            return Encoding.UTF8.GetString(buffer);
                        }

                        return string.Empty;
                    default:
                        return null;
                }
            }
        }
    }
}
#endif