// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;
#if !NET462
using System.Runtime.InteropServices;
#endif

namespace Cake.Core.IO
{
    /// <summary>
    /// Responsible for retrieving information about the current machine.
    /// </summary>
    public static class Machine
    {
        /// <summary>
        /// Determines if the current operative system is 64 bit.
        /// </summary>
        /// <returns>Whether or not the current operative system is 64 bit.</returns>
        public static bool Is64BitOperativeSystem()
        {
#if NET462
            return Environment.Is64BitOperatingSystem;
#else
            return RuntimeInformation.OSArchitecture == Architecture.X64
                || RuntimeInformation.OSArchitecture == Architecture.Arm64;
#endif
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        public static bool IsUnix()
        {
#if NET462
            var platform = (int)Environment.OSVersion.Platform;
            return (platform == 4) || (platform == 6) || (platform == 128);
#else
            return RuntimeInformation.IsOSPlatform(OSPlatform.Linux)
                || RuntimeInformation.IsOSPlatform(OSPlatform.OSX);
#endif
        }
    }
}
