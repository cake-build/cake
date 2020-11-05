// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
#if NETCORE
using System.Linq;
using System.Reflection;
using System.Runtime.InteropServices;
#endif
using System.Runtime.Versioning;

namespace Cake.Core.Polyfill
{
    internal static class EnvironmentHelper
    {
#if !NETCORE
        private static bool? _isRunningOnMac;
#else
        private static readonly FrameworkName NetStandardFramework = new FrameworkName(".NETStandard,Version=v2.0");
        private static bool? _isCoreClr;
        private static FrameworkName netCoreAppFramwork;
#endif

        public static bool Is64BitOperativeSystem()
        {
#if NETCORE
            return RuntimeInformation.OSArchitecture == Architecture.X64
                   || RuntimeInformation.OSArchitecture == Architecture.Arm64;
#else
            return Environment.Is64BitOperatingSystem;
#endif
        }

        public static PlatformFamily GetPlatformFamily()
        {
#if NETCORE
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
#else
            var platform = (int)Environment.OSVersion.Platform;
            if (platform <= 3 || platform == 5)
            {
                return PlatformFamily.Windows;
            }
            if (!_isRunningOnMac.HasValue)
            {
                _isRunningOnMac = Native.MacOSX.IsRunningOnMac();
            }
            if (_isRunningOnMac ?? false || platform == (int)PlatformID.MacOSX)
            {
                return PlatformFamily.OSX;
            }
            if (platform == 4 || platform == 6 || platform == 128)
            {
                return PlatformFamily.Linux;
            }
#endif
            return PlatformFamily.Unknown;
        }

        public static bool IsCoreClr()
        {
#if NETCORE
            if (_isCoreClr == null)
            {
                _isCoreClr = Environment.Version.Major >= 5
                             || RuntimeInformation.FrameworkDescription.StartsWith(".NET Core");
            }
            return _isCoreClr.Value;
#else
            return false;
#endif
        }

        public static bool IsUnix()
        {
            return IsUnix(GetPlatformFamily());
        }

        public static bool IsUnix(PlatformFamily family)
        {
            return family == PlatformFamily.Linux
                   || family == PlatformFamily.OSX;
        }

        public static bool IsOSX(PlatformFamily family)
        {
            return family == PlatformFamily.OSX;
        }

        public static bool IsLinux(PlatformFamily family)
        {
            return family == PlatformFamily.Linux;
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
#if NETCORE
            if (netCoreAppFramwork != null)
            {
                return netCoreAppFramwork;
            }

            var assemblyPath = typeof(System.Runtime.GCSettings)?.GetTypeInfo()
                ?.Assembly
#if NET5_0
                ?.Location;
#else
#pragma warning disable 0618
                ?.CodeBase;
#pragma warning restore 0618
#endif
            if (string.IsNullOrEmpty(assemblyPath))
            {
                return NetStandardFramework;
            }

            const string microsoftNetCoreApp = "Microsoft.NETCore.App";
            var runtimeBasePathLength = assemblyPath.IndexOf(microsoftNetCoreApp) + microsoftNetCoreApp.Length + 1;
            var netCoreAppVersion = string.Concat(assemblyPath.Skip(runtimeBasePathLength).Take(3));
            if (string.IsNullOrEmpty(netCoreAppVersion))
            {
                return NetStandardFramework;
            }

            return netCoreAppFramwork = Version.TryParse(netCoreAppVersion, out var version)
                                            ? new FrameworkName(".NETCoreApp", version)
                                            : NetStandardFramework;
#else
            return new FrameworkName(".NETFramework,Version=v4.6.1");
#endif
        }
    }
}