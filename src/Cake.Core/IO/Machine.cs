// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

namespace Cake.Core.IO
{
    /// <summary>
    /// Responsible for retrieving information about the current machine.
    /// </summary>
    internal static class Machine
    {
        /// <summary>
        /// Determines if the current operative system is 64 bit.
        /// </summary>
        /// <returns>Whether or not the current operative system is 64 bit.</returns>
        public static bool Is64BitOperativeSystem()
        {
            return Environment.Is64BitOperatingSystem;
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        public static bool IsUnix()
        {
            return IsUnix(GetPlatformFamily());
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <param name="family">The platform family.</param>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        public static bool IsUnix(PlatformFamily family)
        {
            return family == PlatformFamily.Linux
                || family == PlatformFamily.OSX;
        }

        /// <summary>
        /// Gets the platform family.
        /// </summary>
        /// <returns>The platform family.</returns>
        public static PlatformFamily GetPlatformFamily()
        {
            var platform = (int)Environment.OSVersion.Platform;
            if (platform == (int)PlatformID.MacOSX)
            {
                return PlatformFamily.OSX;
            }
            if (platform == 4 || platform == 6 || platform == 128)
            {
                return PlatformFamily.Linux;
            }
            if (platform <= 3 || platform == 5)
            {
                return PlatformFamily.Windows;
            }
            return PlatformFamily.Unknown;
        }
    }
}
