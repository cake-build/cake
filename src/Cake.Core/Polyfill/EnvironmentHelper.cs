// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Cake.Core.Polyfill;

internal static class EnvironmentHelper
{
    private static FrameworkName netCoreAppFramework;

    public static bool Is64BitOperativeSystem()
    {
        return RuntimeInformation.OSArchitecture == Architecture.X64
               || RuntimeInformation.OSArchitecture == Architecture.Arm64;
    }

    public static PlatformFamily GetPlatformFamily()
    {
        if (OperatingSystem.IsMacOS())
        {
            return PlatformFamily.OSX;
        }

        if (OperatingSystem.IsLinux())
        {
            return PlatformFamily.Linux;
        }

        if (OperatingSystem.IsWindows())
        {
            return PlatformFamily.Windows;
        }

        if (OperatingSystem.IsFreeBSD())
        {
            return PlatformFamily.FreeBSD;
        }

        return PlatformFamily.Unknown;
    }

    public static bool IsCoreClr()
    {
        return true;
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
        return Runtime.CoreClr;
    }

    public static FrameworkName GetBuiltFramework()
    {
        if (netCoreAppFramework != null)
        {
            return netCoreAppFramework;
        }

        var version = Environment.Version;
        return netCoreAppFramework = new FrameworkName(".NETCoreApp", new Version(version.Major, version.Minor));
    }
}
