// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Common.Tools.InnoSetup;
using Cake.Core.IO;
using Cake.Testing;
using Cake.Testing.Fixtures;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class InnoSetupFixture : ToolFixture<InnoSetupSettings>
    {
        public IRegistry Registry { get; set; }

        public FilePath ScriptPath { get; set; }

        public Dictionary<InnoSetupVersion, FilePath> InstalledToolPaths { get; private set; }

        public InnoSetupFixture()
            : base("iscc.exe")
        {
            ScriptPath = new FilePath("./Test.iss");
            InstalledToolPaths = new Dictionary<InnoSetupVersion, FilePath>();
            Registry = Substitute.For<IRegistry>();
            var hklm = Substitute.For<IRegistryKey>();
            Registry.LocalMachine.Returns(hklm);
        }

        public void GivenToolIsInstalled(bool is64BitPlatform, bool is64Bit, InnoSetupVersion version)
        {
            Environment.Platform.Is64Bit = is64BitPlatform;
            ConfigureInstalledLocation(is64BitPlatform, is64Bit, version);
        }

        protected override void RunTool()
        {
            var tool = new InnoSetupRunner(FileSystem, Registry, Environment, ProcessRunner, Tools);
            tool.Run(ScriptPath, Settings);
        }

        private void ConfigureInstalledLocation(bool is64BitPlatform, bool is64Bit, InnoSetupVersion version)
        {
            var useWow6432 = is64BitPlatform && !is64Bit;

            var registryKeyPath = GetRegistryKeyPathForVersion(useWow6432, is64Bit, version);
            var installLocation = GetInstallLocation(useWow6432, version);

            var installedToolPath = installLocation.CombineWithFilePath("iscc.exe");
            InstalledToolPaths.Add(version, installLocation.CombineWithFilePath("iscc.exe"));

            var innoSetupKey = Substitute.For<IRegistryKey>();
            innoSetupKey.GetValue("InstallLocation").Returns(installLocation.FullPath);

            Registry.LocalMachine.OpenKey(registryKeyPath).Returns(innoSetupKey);

            FileSystem.CreateDirectory(installLocation);
            FileSystem.CreateFile(installedToolPath);
        }

        private static string GetRegistryKeyPathForVersion(bool useWow6432, bool is64Bit, InnoSetupVersion version)
        {
            switch (version)
            {
                case InnoSetupVersion.InnoSetup7 when !is64Bit:
                    return GetRegistryKeyPath(useWow6432, "Inno Setup 7 (32-bit)_is1");
                case InnoSetupVersion.InnoSetup7:
                    return GetRegistryKeyPath(useWow6432, "Inno Setup 7_is1");
                case InnoSetupVersion.InnoSetup6:
                    return GetRegistryKeyPath(useWow6432, "Inno Setup 6_is1");
                case InnoSetupVersion.InnoSetup5:
                    return GetRegistryKeyPath(useWow6432, "Inno Setup 5_is1");
                default:
                    return null;
            }
        }

        private static string GetRegistryKeyPath(bool useWow6432, string relativePath)
        {
            // On 64-bit Windows, the registry key for Inno Setup (32-bit) will be accessible under Wow6432Node
            var softwareKeyPath = useWow6432 ? @"SOFTWARE\Wow6432Node\" : @"SOFTWARE\";
            return $@"{softwareKeyPath}Microsoft\Windows\CurrentVersion\Uninstall\{relativePath}";
        }

        private static DirectoryPath GetInstallLocation(bool useWow6432, InnoSetupVersion version)
        {
            var programFiles = useWow6432 ? "/Program Files (x86)/" : "/Program Files/";
            switch (version)
            {
                case InnoSetupVersion.InnoSetup7:
                    return $@"{programFiles}Inno Setup 7";
                case InnoSetupVersion.InnoSetup6:
                    return $@"{programFiles}Inno Setup 6";
                case InnoSetupVersion.InnoSetup5:
                    return $@"{programFiles}Inno Setup 5";
                default:
                    return null;
            }
        }
    }
}