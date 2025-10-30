// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Reflection;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Cake.Core.Polyfill
{
    internal static class EnvironmentHelper
    {
        private static readonly FrameworkName NetStandardFramework = new FrameworkName(".NETStandard,Version=v2.0");
        private static bool? _isCoreClr;
        private static FrameworkName netCoreAppFramwork;

        public static bool Is64BitOperativeSystem()
        {
            return RuntimeInformation.OSArchitecture == Architecture.X64
                   || RuntimeInformation.OSArchitecture == Architecture.Arm64;
        }

        public static PlatformFamily GetPlatformFamily()
        {
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.OSX))
                {
                    return PlatformFamily.OSX;
                }
            }
            catch (PlatformNotSupportedException)
            {
            }
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Linux))
                {
                    return PlatformFamily.Linux;
                }
            }
            catch (PlatformNotSupportedException)
            {
            }
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Windows))
                {
                    return PlatformFamily.Windows;
                }
            }
            catch (PlatformNotSupportedException)
            {
            }
            try
            {
                if (RuntimeInformation.IsOSPlatform(OSPlatform.Create("FREEBSD")))
                {
                    return PlatformFamily.FreeBSD;
                }
            }
            catch (PlatformNotSupportedException)
            {
            }

            return PlatformFamily.Unknown;
        }

        public static bool IsCoreClr()
        {
            if (_isCoreClr == null)
            {
                _isCoreClr = Environment.Version.Major >= 5
                             || RuntimeInformation.FrameworkDescription.StartsWith(".NET Core");
            }
            return _isCoreClr.Value;
        }

        public static bool IsWindows(PlatformFamily family)
        {
            return family == PlatformFamily.Windows;
        }

        public static bool IsUnix()
        {
            return IsUnix(GetPlatformFamily());
        }

        public static bool IsUnix(PlatformFamily family)
        {
            return family == PlatformFamily.Linux
                   || family == PlatformFamily.OSX
                   || family == PlatformFamily.FreeBSD;
        }

        public static bool IsOSX(PlatformFamily family)
        {
            return family == PlatformFamily.OSX;
        }

        public static bool IsLinux(PlatformFamily family)
        {
            return family == PlatformFamily.Linux;
        }

        public static bool IsFreeBSD(PlatformFamily family)
        {
            return family == PlatformFamily.FreeBSD;
        }

        public static Runtime GetRuntime()
        {
            if (IsCoreClr())
            {
                return Runtime.CoreClr;
            }
            return Runtime.Clr;
        }

        public static FrameworkName GetBuiltFramework()
        {
            if (netCoreAppFramwork != null)
            {
                return netCoreAppFramwork;
            }

            var assembly = typeof(System.Runtime.GCSettings)?.GetTypeInfo()
                            ?.Assembly;

            var assemblyPath = assembly
#pragma warning disable IL3000 // Avoid accessing Assembly file path when publishing as a single file
                ?.Location;
#pragma warning restore IL3000 // Avoid accessing Assembly file path when publishing as a single file

            if (string.IsNullOrEmpty(assemblyPath))
            {
                if (assembly.GetName().Version is Version assemblyVersion)
                {
                    return netCoreAppFramwork = new FrameworkName(".NETCoreApp", new Version(assemblyVersion.Major, assemblyVersion.Minor));
                }

                return netCoreAppFramwork = NetStandardFramework;
            }

            const string microsoftNetCoreApp = "Microsoft.NETCore.App";
            const int microsoftNetCoreAppLengthPlusOne = 22;
            var versionStart = Math.Max(0, assemblyPath.IndexOf(microsoftNetCoreApp) + microsoftNetCoreAppLengthPlusOne);
            var versionEnd = versionStart + Math.Max(3, assemblyPath.IndexOfAny(['-', '/', '\\'], versionStart) - versionStart);
            var netCoreAppVersion = assemblyPath[versionStart..versionEnd];

            if (string.IsNullOrEmpty(netCoreAppVersion))
            {
                return netCoreAppFramwork = NetStandardFramework;
            }

            return netCoreAppFramwork = Version.TryParse(netCoreAppVersion, out var version)
                                            ? new FrameworkName(".NETCoreApp", new Version(version.Major, version.Minor))
                                            : NetStandardFramework;
        }
    }
}