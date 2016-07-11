// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Packaging;
using Cake.Testing;
using NSubstitute;

namespace Cake.NuGet.Tests.Fixtures
{
    internal sealed class NuGetPackageInstallerFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IFileSystem FileSystem { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public INuGetToolResolver ToolResolver { get; set; }
        public INuGetPackageContentResolver ContentResolver { get; set; }
        public ICakeLog Log { get; set; }

        public PackageReference Package { get; set; }
        public PackageType PackageType { get; set; }
        public DirectoryPath InstallPath { get; set; }

        public NuGetPackageInstallerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            ProcessRunner = Substitute.For<IProcessRunner>();
            ContentResolver = Substitute.For<INuGetPackageContentResolver>();
            Log = Substitute.For<ICakeLog>();

            ToolResolver = Substitute.For<INuGetToolResolver>();
            ToolResolver.ResolvePath().Returns(new FilePath("/Working/tools/nuget.exe"));

            Package = new PackageReference("nuget:https://myget.org/temp/?package=Cake.Foo&prerelease&version=1.2.3");
            PackageType = PackageType.Addin;
            InstallPath = new DirectoryPath("./nuget");
        }

        public NuGetPackageInstaller CreateInstaller()
        {
            return new NuGetPackageInstaller(FileSystem, Environment, ProcessRunner, ToolResolver , ContentResolver, Log);
        }

        public IReadOnlyCollection<IFile> Install()
        {
            var installer = CreateInstaller();
            return installer.Install(Package, PackageType, InstallPath);
        }

        public bool CanInstall()
        {
            var installer = CreateInstaller();
            return installer.CanInstall(Package, PackageType);
        }
    }
}
