// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Tools.SignTool;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;

namespace Cake.Common.Tests.Fixtures.Tools
{
    internal sealed class SignToolResolverFixture
    {
        private readonly bool _is64Bit;

        public IFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public IRegistry Registry { get; set; }


        public SignToolResolverFixture(bool is64Bit = true)
        {
            _is64Bit = is64Bit;

            FileSystem = Substitute.For<IFileSystem>();
            Environment = Substitute.For<ICakeEnvironment>();
            Registry = Substitute.For<IRegistry>();

            Environment.Is64BitOperativeSystem().Returns(_is64Bit);
            Environment.GetSpecialPath(SpecialPath.ProgramFiles).Returns("/ProgramFiles");
            Environment.GetSpecialPath(SpecialPath.ProgramFilesX86).Returns("/ProgramFilesX86");
        }

        public void GivenThatToolExistInKnownPath()
        {
            if (_is64Bit)
            {
                FileSystem.Exist(Arg.Is<FilePath>(p => p.FullPath == "/ProgramFilesX86/Windows Kits/8.1/bin/x64/signtool.exe")).Returns(true);
            }
            else
            {
                FileSystem.Exist(Arg.Is<FilePath>(p => p.FullPath == "/ProgramFiles/Windows Kits/8.1/bin/x86/signtool.exe")).Returns(true);
            }
        }

        public void GivenThatToolHasRegistryKey()
        {
            var signToolKey = Substitute.For<IRegistryKey>();
            signToolKey.GetValue("InstallationFolder").Returns("/SignTool");

            var windowsKey = Substitute.For<IRegistryKey>();
            windowsKey.GetSubKeyNames().Returns(new[] { "v8.1A" });
            windowsKey.OpenKey("v8.1A").Returns(signToolKey);

            var localMachine = Substitute.For<IRegistryKey>();
            localMachine.OpenKey("Software\\Microsoft\\Microsoft SDKs\\Windows").Returns(windowsKey);

            FileSystem.Exist(Arg.Is<FilePath>(p => p.FullPath == "/SignTool/bin/signtool.exe")).Returns(true);
            Registry.LocalMachine.Returns(localMachine);
        }

        public void GivenThatNoSdkRegistryKeyExist()
        {
            var localMachine = Substitute.For<IRegistryKey>();
            localMachine.OpenKey("Software\\Microsoft\\Microsoft SDKs\\Windows").Returns((IRegistryKey)null);
            Registry.LocalMachine.Returns(localMachine);
        }

        public FilePath Resolve()
        {
            var resolver = new SignToolResolver(FileSystem, Environment, Registry);
            return resolver.GetPath();
        }
    }
}
