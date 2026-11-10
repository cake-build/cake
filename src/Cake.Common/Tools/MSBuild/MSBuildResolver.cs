// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSBuild;

internal static class MSBuildResolver
{
    private const string CurrentBinPath = "MSBuild/Current/Bin";
    private const string VS2017BinPath = "MSBuild/15.0/Bin";

    public static FilePath GetMSBuildPath(IFileSystem fileSystem, ICakeEnvironment environment, MSBuildPlatform buildPlatform, MSBuildSettings settings, IMSBuildInstallationLocator installationLocator = null)
    {
        if (environment.Platform.Family == PlatformFamily.OSX)
        {
            var macMSBuildPath = new FilePath("/Library/Frameworks/Mono.framework/Versions/Current/Commands/msbuild");

            if (fileSystem.Exist(macMSBuildPath))
            {
                return macMSBuildPath;
            }

            var brewMSBuildPath = new FilePath("/usr/local/bin/msbuild");

            if (fileSystem.Exist(brewMSBuildPath))
            {
                return brewMSBuildPath;
            }

            throw new CakeException("Could not resolve MSBuild.");
        }
        else if (environment.Platform.Family == PlatformFamily.Linux)
        {
            var linuxMSBuildPath = new FilePath("/usr/bin/msbuild");

            if (fileSystem.Exist(linuxMSBuildPath))
            {
                return linuxMSBuildPath;
            }

            throw new CakeException("Could not resolve MSBuild.");
        }
        else if (environment.Platform.Family == PlatformFamily.FreeBSD)
        {
            var freebsdMSBuildPath = new FilePath("/usr/local/bin/msbuild");

            if (fileSystem.Exist(freebsdMSBuildPath))
            {
                return freebsdMSBuildPath;
            }

            throw new CakeException("Could not resolve MSBuild.");
        }

        var binPath = settings.ToolVersion == MSBuildToolVersion.Default
            ? GetHighestAvailableMSBuildVersion(fileSystem, environment, buildPlatform, settings.AllowPreviewVersion, installationLocator)
            : GetMSBuildPath(fileSystem, environment, (MSBuildVersion)settings.ToolVersion, buildPlatform, settings.CustomVersion, settings.AllowPreviewVersion, installationLocator);

        if (binPath == null)
        {
            throw new CakeException("Could not resolve MSBuild.");
        }

        // Get the MSBuild path.
        return binPath.CombineWithFilePath("MSBuild.exe");
    }

    private static DirectoryPath GetHighestAvailableMSBuildVersion(IFileSystem fileSystem, ICakeEnvironment environment, MSBuildPlatform buildPlatform, bool allowPreview, IMSBuildInstallationLocator installationLocator)
    {
        var visualStudioVersions = new[]
        {
            MSBuildVersion.MSBuild18,
            MSBuildVersion.MSBuild17,
            MSBuildVersion.MSBuild16,
            MSBuildVersion.MSBuild15,
        };

        var legacyVersions = new[]
        {
            MSBuildVersion.MSBuild14,
            MSBuildVersion.MSBuild12,
            MSBuildVersion.MSBuild4,
            MSBuildVersion.MSBuild35,
            MSBuildVersion.MSBuild20,
        };

        // The installation locator is not passed here, so it's only queried once below instead of once per version.
        foreach (var version in visualStudioVersions)
        {
            var path = GetMSBuildPath(fileSystem, environment, version, buildPlatform, null, allowPreview, null);
            if (fileSystem.Exist(path))
            {
                return path;
            }
        }

        // Ask the installation locator before falling back to legacy versions,
        // as the .NET Framework MSBuild is present on almost every Windows machine.
        var locatedPath = FindInstalledMSBuild(fileSystem, environment, buildPlatform, allowPreview, installationLocator, null, CurrentBinPath, VS2017BinPath);
        if (locatedPath != null)
        {
            return locatedPath;
        }

        foreach (var version in legacyVersions)
        {
            var path = GetMSBuildPath(fileSystem, environment, version, buildPlatform, null, allowPreview, null);
            if (fileSystem.Exist(path))
            {
                return path;
            }
        }

        return null;
    }

    private static DirectoryPath GetMSBuildPath(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        MSBuildVersion version,
        MSBuildPlatform buildPlatform,
        string customVersion,
        bool allowPreview,
        IMSBuildInstallationLocator installationLocator)
    {
        switch (version)
        {
            case MSBuildVersion.MSBuild18:
                return GetVisualStudioYearPath(fileSystem, environment, "18", CurrentBinPath, "[18.0,19.0)", buildPlatform, allowPreview, installationLocator);
            case MSBuildVersion.MSBuild17:
                return GetVisualStudioYearPath(fileSystem, environment, "2022", CurrentBinPath, "[17.0,18.0)", buildPlatform, allowPreview, installationLocator);
            case MSBuildVersion.MSBuild16:
                return GetVisualStudioYearPath(fileSystem, environment, "2019", CurrentBinPath, "[16.0,17.0)", buildPlatform, allowPreview, installationLocator);
            case MSBuildVersion.MSBuild15:
                return GetVisualStudioYearPath(fileSystem, environment, "2017", VS2017BinPath, "[15.0,16.0)", buildPlatform, allowPreview, installationLocator);
            case MSBuildVersion.MSBuild14:
                return GetVisualStudioPath(environment, buildPlatform, "14.0");
            case MSBuildVersion.MSBuild12:
                return GetVisualStudioPath(environment, buildPlatform, "12.0");
            case MSBuildVersion.MSBuildCustomVS:
                return GetVisualStudioPath(environment, buildPlatform, customVersion);
            case MSBuildVersion.MSBuild4:
                return GetFrameworkPath(environment, buildPlatform, "v4.0.30319");
            case MSBuildVersion.MSBuild35:
                return GetFrameworkPath(environment, buildPlatform, "v3.5");
            case MSBuildVersion.MSBuild20:
                return GetFrameworkPath(environment, buildPlatform, "v2.0.50727");
            case MSBuildVersion.MSBuildNETCustom:
                if (!customVersion.Contains("v"))
                {
                    customVersion = "v" + customVersion;
                }
                return GetFrameworkPath(environment, buildPlatform, customVersion);
            default:
                return null;
        }
    }

    private static DirectoryPath GetVisualStudioPath(ICakeEnvironment environment, MSBuildPlatform buildPlatform, string version)
    {
        // Get the bin path.
        var programFilesPath = environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
        var binPath = programFilesPath.Combine(string.Concat("MSBuild/", version, "/Bin"));
        return ApplyPlatform(environment, buildPlatform, binPath);
    }

    private static DirectoryPath GetVisualStudioYearPath(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        string year,
        string relativeBinPath,
        string versionRange,
        MSBuildPlatform buildPlatform,
        bool allowPreviewVersion,
        IMSBuildInstallationLocator installationLocator)
    {
        foreach (var edition in allowPreviewVersion
                     ? VisualStudio.Editions.All
                     : VisualStudio.Editions.Stable)
        {
            foreach (var rootPath in VisualStudio.GetYearAndEditionRootPaths(environment, year, edition))
            {
                // Get the bin path, and only accept it if MSBuild.exe exists for the requested platform,
                // so a leftover bin directory doesn't hide a complete installation in another root.
                var binPath = ApplyPlatform(environment, buildPlatform, rootPath.Combine(relativeBinPath));
                if (fileSystem.Exist(binPath.CombineWithFilePath("MSBuild.exe")))
                {
                    return binPath;
                }
            }
        }

        return FindInstalledMSBuild(fileSystem, environment, buildPlatform, allowPreviewVersion, installationLocator, versionRange, relativeBinPath)
            ?? VisualStudio.GetYearAndEditionRootPath(environment, year, "Professional").Combine(relativeBinPath);
    }

    private static DirectoryPath FindInstalledMSBuild(
        IFileSystem fileSystem,
        ICakeEnvironment environment,
        MSBuildPlatform buildPlatform,
        bool allowPreviewVersion,
        IMSBuildInstallationLocator installationLocator,
        string versionRange,
        params string[] relativeBinPaths)
    {
        var installationPath = installationLocator?.FindInstallation(versionRange, allowPreviewVersion);
        if (installationPath == null)
        {
            return null;
        }

        foreach (var relativeBinPath in relativeBinPaths)
        {
            var binPath = installationPath.Combine(relativeBinPath);
            var platformBinPath = ApplyPlatform(environment, buildPlatform, binPath);
            if (fileSystem.Exist(platformBinPath.CombineWithFilePath("MSBuild.exe")))
            {
                return platformBinPath;
            }

            // The located installation might not contain the amd64 MSBuild,
            // so prefer its MSBuild over falling back to the .NET Framework one.
            if (fileSystem.Exist(binPath.CombineWithFilePath("MSBuild.exe")))
            {
                return binPath;
            }
        }

        return null;
    }

    private static DirectoryPath ApplyPlatform(ICakeEnvironment environment, MSBuildPlatform buildPlatform, DirectoryPath binPath)
    {
        if (buildPlatform == MSBuildPlatform.x64
            || (buildPlatform == MSBuildPlatform.Automatic && environment.Platform.Is64Bit))
        {
            return binPath.Combine("amd64");
        }

        return binPath;
    }

    private static DirectoryPath GetFrameworkPath(ICakeEnvironment environment, MSBuildPlatform buildPlatform, string version)
    {
        // Get the Microsoft .NET folder.
        var windowsFolder = environment.GetSpecialPath(SpecialPath.Windows);
        var netFolder = windowsFolder.Combine("Microsoft.NET");

        if (buildPlatform == MSBuildPlatform.Automatic)
        {
            // Get the framework folder.
            var is64Bit = environment.Platform.Is64Bit;
            var frameWorkFolder = is64Bit ? netFolder.Combine("Framework64") : netFolder.Combine("Framework");
            return frameWorkFolder.Combine(version);
        }

        if (buildPlatform == MSBuildPlatform.x86)
        {
            return netFolder.Combine("Framework").Combine(version);
        }

        if (buildPlatform == MSBuildPlatform.x64)
        {
            return netFolder.Combine("Framework64").Combine(version);
        }

        throw new NotSupportedException();
    }
}
