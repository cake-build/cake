// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

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

        public FilePath InstalledToolPath { get; private set; }

        public InnoSetupFixture()
            : base("iscc.exe")
        {
            ScriptPath = new FilePath("./Test.iss");
            Registry = Substitute.For<IRegistry>();
        }

        public void GivenToolIsInstalledX86()
        {
            ConfigureInstalledLocation(false);
        }

        public void GivenToolIsInstalledX64()
        {
            ConfigureInstalledLocation(true);
        }

        private void ConfigureInstalledLocation(bool is64Bit)
        {
            string registryKeyPath = is64Bit
                ? @"SOFTWARE\Wow6432Node\Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 5_is1"
                : @"SOFTWARE\Microsoft\Windows\CurrentVersion\Uninstall\Inno Setup 5_is1";

            DirectoryPath installLocation = is64Bit
                ? "/programfiles(x86)/innosetup"
                : "/programfiles/innosetup";

            InstalledToolPath = installLocation.CombineWithFilePath("iscc.exe");

            var innoSetupKey = Substitute.For<IRegistryKey>();
            innoSetupKey.GetValue("InstallLocation").Returns(installLocation.FullPath);

            var hklm = Substitute.For<IRegistryKey>();
            hklm.OpenKey(registryKeyPath).Returns(innoSetupKey);

            Registry.LocalMachine.Returns(hklm);

            FileSystem.CreateDirectory(installLocation);
            FileSystem.CreateFile(InstalledToolPath);

            Environment.Platform.Is64Bit = is64Bit;
        }

        protected override void RunTool()
        {
            var tool = new InnoSetupRunner(FileSystem, Registry, Environment, ProcessRunner, Tools);
            tool.Run(ScriptPath, Settings);
        }
    }
}