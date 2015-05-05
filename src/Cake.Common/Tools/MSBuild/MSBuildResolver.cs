using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.Common.Tools.MSBuild
{
    internal static class MSBuildResolver
    {
        public static FilePath GetMSBuildPath(IFileSystem fileSystem, ICakeEnvironment environment, MSBuildToolVersion version, PlatformTarget target)
        {
            var binPath = (version == MSBuildToolVersion.Default)
                ? GetHighestAvailableMSBuildVersion(fileSystem, environment, target)
                : GetMSBuildPath(environment, (MSBuildVersion)version, target);

            if (binPath == null)
            {
                throw new CakeException("Could not resolve MSBuild.");
            }

            // Get the MSBuild path.
            return binPath.CombineWithFilePath("MSBuild.exe");
        }

        private static DirectoryPath GetHighestAvailableMSBuildVersion(IFileSystem fileSystem, ICakeEnvironment environment, PlatformTarget target)
        {
            var versions = new[] 
            {
                MSBuildVersion.MSBuild14,
                MSBuildVersion.MSBuild12, 
                MSBuildVersion.MSBuild4,
                MSBuildVersion.MSBuild35,
                MSBuildVersion.MSBuild20
            };

            foreach (var version in versions)
            {
                var path = GetMSBuildPath(environment, version, target);
                if (fileSystem.Exist(path))
                {
                    return path;
                }
            }
            return null;
        }

        private static DirectoryPath GetMSBuildPath(ICakeEnvironment environment, MSBuildVersion version, PlatformTarget target)
        {
            switch (version)
            {
                case MSBuildVersion.MSBuild14:
                    return GetVisualStudioPath(environment, target, "14.0");
                case MSBuildVersion.MSBuild12:
                    return GetVisualStudioPath(environment, target, "12.0");
                case MSBuildVersion.MSBuild4:
                    return GetFrameworkPath(environment, target, "v4.0.30319");
                case MSBuildVersion.MSBuild35:
                    return GetFrameworkPath(environment, target, "v3.5");
                case MSBuildVersion.MSBuild20:
                    return GetFrameworkPath(environment, target, "v2.0.50727");
                default:
                    return null;
            }
        }

        private static DirectoryPath GetVisualStudioPath(ICakeEnvironment environment, PlatformTarget target, string version)
        {
            // Get the bin path.
            var programFilesPath = environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
            var binPath = programFilesPath.Combine(string.Concat("MSBuild/", version, "/Bin"));
            if (target == PlatformTarget.MSIL)
            {
                if (environment.Is64BitOperativeSystem())
                {
                    binPath = binPath.Combine("amd64");
                }
            }
            if (target == PlatformTarget.x64)
            {
                binPath = binPath.Combine("amd64");
            }
            return binPath;
        }

        private static DirectoryPath GetFrameworkPath(ICakeEnvironment environment, PlatformTarget target, string version)
        {
            // Get the Microsoft .NET folder.
            var windowsFolder = environment.GetSpecialPath(SpecialPath.Windows);
            var netFolder = windowsFolder.Combine("Microsoft.NET");

            if (target == PlatformTarget.MSIL)
            {
                // Get the framework folder.
                var is64Bit = environment.Is64BitOperativeSystem();
                var frameWorkFolder = is64Bit ? netFolder.Combine("Framework64") : netFolder.Combine("Framework");
                return frameWorkFolder.Combine(version);
            }

            if (target == PlatformTarget.x86)
            {
                return netFolder.Combine("Framework").Combine(version);
            }

            if (target == PlatformTarget.x64)
            {
                return netFolder.Combine("Framework64").Combine(version);
            }

            throw new NotSupportedException();
        }
    }
}
