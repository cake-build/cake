// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using System.Runtime;
using System.Runtime.InteropServices;
using System.Runtime.Versioning;

namespace Cake.Core.Polyfill;

internal static class EnvironmentHelper
{
    private static readonly FrameworkName NetStandardFramework = new FrameworkName(".NETStandard,Version=v2.0");
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
        if (netCoreAppFramwork != null)
        {
            return netCoreAppFramwork;
        }

        if (TryCreateNetCoreAppFramework(Environment.Version) is { } fromEnvironment)
        {
            return netCoreAppFramwork = fromEnvironment;
        }

        if (TryParseFrameworkDescription(RuntimeInformation.FrameworkDescription) is { } fromDescription)
        {
            return netCoreAppFramwork = fromDescription;
        }

        var assembly = typeof(GCSettings).Assembly;
        var assemblyPath = AssemblyPathResolver.GetAssemblyFilePath(assembly);
        if (TryParseNetCoreAppFromPath(assemblyPath) is { } fromPath)
        {
            return netCoreAppFramwork = fromPath;
        }

        if (assembly?.GetName().Version is { } assemblyVersion)
        {
            return netCoreAppFramwork = new FrameworkName(".NETCoreApp", new Version(assemblyVersion.Major, assemblyVersion.Minor));
        }

        return netCoreAppFramwork = NetStandardFramework;
    }

    private static FrameworkName TryCreateNetCoreAppFramework(Version version)
    {
        if (version is null || version.Major < 5)
        {
            return null;
        }

        return new FrameworkName(".NETCoreApp", new Version(version.Major, version.Minor));
    }

    private static FrameworkName TryParseFrameworkDescription(string description)
    {
        if (string.IsNullOrEmpty(description) ||
            description.StartsWith(".NET Framework", StringComparison.OrdinalIgnoreCase))
        {
            return null;
        }

        const string netCorePrefix = ".NET Core ";
        const string netPrefix = ".NET ";

        string versionPart;
        if (description.StartsWith(netCorePrefix, StringComparison.OrdinalIgnoreCase))
        {
            versionPart = description[netCorePrefix.Length..];
        }
        else if (description.StartsWith(netPrefix, StringComparison.OrdinalIgnoreCase))
        {
            versionPart = description[netPrefix.Length..];
        }
        else
        {
            return null;
        }

        var separator = versionPart.IndexOfAny([' ', '-']);
        if (separator >= 0)
        {
            versionPart = versionPart[..separator];
        }

        return Version.TryParse(versionPart, out var version)
            ? new FrameworkName(".NETCoreApp", new Version(version.Major, version.Minor))
            : null;
    }

    private static FrameworkName TryParseNetCoreAppFromPath(string assemblyPath)
    {
        if (string.IsNullOrEmpty(assemblyPath))
        {
            return null;
        }

        const string microsoftNetCoreApp = "Microsoft.NETCore.App";
        const int microsoftNetCoreAppLengthPlusOne = 22;
        var versionStart = Math.Max(0, assemblyPath.IndexOf(microsoftNetCoreApp) + microsoftNetCoreAppLengthPlusOne);
        var versionEnd = versionStart + Math.Max(3, assemblyPath.IndexOfAny(['-', '/', '\\'], versionStart) - versionStart);
        var netCoreAppVersion = assemblyPath[versionStart..versionEnd];

        if (string.IsNullOrEmpty(netCoreAppVersion))
        {
            return null;
        }

        return Version.TryParse(netCoreAppVersion, out var version)
            ? new FrameworkName(".NETCoreApp", new Version(version.Major, version.Minor))
            : null;
    }
}
