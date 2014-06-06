using System;
using Cake.Core;
using Cake.Core.IO;

namespace Cake.MSBuild
{
    internal sealed class MSBuildResolver
    {
        public static FilePath GetMSBuildPath(ICakeContext context, MSBuildToolVersion version, PlatformTarget target)
        {
            // Get the bin path for MSBuild.
            var binPath = GetMSBuildPath(context, (MSBuildVersion)version, target);
            if (binPath == null)
            {
                throw new CakeException("Could not resolve MSBuild.");
            }

            // Get the MSBuild path.
            return binPath.GetFilePath("MSBuild.exe");
        }

        private static DirectoryPath GetMSBuildPath(ICakeContext context, MSBuildVersion version, PlatformTarget target)
        {
            switch (version)
            {
                case MSBuildVersion.MSBuild12:
                    return GetVisualStudioPath(context, target);
                case MSBuildVersion.MSBuild4:
                    return GetFrameworkPath(context, target, "v4.0.30319");
                case MSBuildVersion.MSBuild35:
                    return GetFrameworkPath(context, target, "v3.5");
                case MSBuildVersion.MSBuild20:
                    return GetFrameworkPath(context, target, "v2.0.50727");
                default:
                    return null;
            }
        }

        private static DirectoryPath GetVisualStudioPath(ICakeContext context, PlatformTarget target)
        {
            // Get the bin path.
            var programFilesPath = context.Environment.GetSpecialPath(SpecialPath.ProgramFilesX86);
            var binPath = programFilesPath.Combine("MSBuild/12.0/Bin");
            if (target == PlatformTarget.MSIL)
            {
                if (context.Environment.Is64BitOperativeSystem())
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

        private static DirectoryPath GetFrameworkPath(ICakeContext context, PlatformTarget target, string version)
        {
            // Get the Microsoft .NET folder.
            var windowsFolder = context.Environment.GetSpecialPath(SpecialPath.Windows);
            var netFolder = windowsFolder.Combine("Microsoft.NET");

            if (target == PlatformTarget.MSIL)
            {
                // Get the framework folder.
                var is64bit = context.Environment.Is64BitOperativeSystem();
                var frameWorkFolder = is64bit ? netFolder.Combine("Framework64") : netFolder.Combine("Framework");
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
