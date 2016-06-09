// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System;

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
            return Environment.Is64BitOperatingSystem;
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        public static bool IsUnix()
        {
            var platform = (int)Environment.OSVersion.Platform;
            return (platform == 4) || (platform == 6) || (platform == 128);
        }
    }
}
