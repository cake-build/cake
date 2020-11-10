// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.IO.NuGet;
using Cake.Core.Packaging;
using Cake.Testing;
using NSubstitute;

namespace Cake.NuGet.Tests.Fixtures
{
    internal sealed class OutOfProcessFixture : NuGetPackageInstallerFixture
    {
        public OutOfProcessFixture()
            : base()
        {
            Config?.GetValue(Constants.NuGet.UseInProcessClient).Returns(bool.FalseString);
        }
    }

    internal abstract class NuGetPackageInstallerFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public FakeFileSystem FileSystem { get; set; }
        public IProcessRunner ProcessRunner { get; set; }
        public INuGetToolResolver ToolResolver { get; set; }
        public INuGetContentResolver ContentResolver { get; set; }
        public ICakeLog Log { get; set; }

        public PackageReference Package { get; set; }
        public PackageType PackageType { get; set; }
        public DirectoryPath InstallPath { get; set; }

        public ICakeConfiguration Config { get; set; }

        public InProcessInstaller InProc { get; set; }
        public OutOfProcessInstaller OutProc { get; set; }

        public NuGetPackageInstallerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            ContentResolver = Substitute.For<INuGetContentResolver>();
            Log = Substitute.For<ICakeLog>();
            Config = Substitute.For<ICakeConfiguration>();

            ToolResolver = Substitute.For<INuGetToolResolver>();
            ToolResolver.ResolvePath().Returns(new FilePath("/Working/tools/nuget.exe"));

            Package = new PackageReference("nuget:https://myget.org/temp/?package=Cake.Foo&prerelease&version=1.2.3");
            PackageType = PackageType.Addin;
            InstallPath = new DirectoryPath("./nuget");

            ProcessRunner = Substitute.For<IProcessRunner>();
            ProcessRunner.When(p => p.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()))
                .Do(info => FileSystem.CreateDirectory(InstallPath.Combine(Package.Package.ToLowerInvariant()).Combine(Package.Package)));

            InProc = new InProcessInstaller(FileSystem, Environment, ContentResolver, Log, Config);
            OutProc = new OutOfProcessInstaller(FileSystem, Environment, ProcessRunner, ToolResolver, ContentResolver, Log, Config);
        }

        public void InstallPackageAtSpecifiedPath(DirectoryPath path)
        {
            ProcessRunner.When(p => p.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()))
                .Do(info => FileSystem.CreateDirectory(path));
        }

        public NuGetPackageInstaller CreateInstaller()
        {
            return new NuGetPackageInstaller(Config, InProc, OutProc);
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