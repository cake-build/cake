using System;

#if DOTNET5_4
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
#if DOTNET5_4
            // TODO: Use RuntimeInformation.OSArchitecture/ProcessArchitecture when that API lands.
            return Marshal.SizeOf(typeof(IntPtr)) == 8;
#else
            return Environment.Is64BitOperatingSystem;
#endif
        }

        /// <summary>
        /// Determines whether the current machine is running Unix.
        /// </summary>
        /// <returns>Whether or not the current machine is running Unix.</returns>
        public static bool IsUnix()
        {
#if DOTNET5_4
            // We assume that !Windows == UNIX. This might not be correct.
            return !RuntimeInformation.IsOSPlatform(OSPlatform.Windows);
#else
            var platform = (int)Environment.OSVersion.Platform;
            return (platform == 4) || (platform == 6) || (platform == 128);
#endif
        }
    }
}