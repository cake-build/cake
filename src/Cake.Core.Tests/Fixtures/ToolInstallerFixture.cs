// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using System.Linq;
using Cake.Core.Configuration;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Tooling;
using Cake.Testing;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class ToolInstallerFixture
    {
        public ICakeEnvironment Environment { get; set; }
        public IToolLocator Locator { get; set; }
        public ICakeConfiguration Configuration { get; set; }
        public ICakeLog Log { get; set; }
        public IPackageInstaller Installer { get; set; }
        public FakeFileSystem FileSystem { get; set; }
        public PackageReference Tool { get; set; }

        public ToolInstallerFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Locator = Substitute.For<IToolLocator>();
            Configuration = Substitute.For<ICakeConfiguration>();
            Log = Substitute.For<ICakeLog>();
            Installer = Substitute.For<IPackageInstaller>();
            Installer.CanInstall(Arg.Any<PackageReference>(), Arg.Any<PackageType>()).Returns(true);
            Tool = new PackageReference("custom:?package=tool");
        }

        public void GivenFilesWillBeInstalled()
        {
            Installer
                .Install(Arg.Any<PackageReference>(), Arg.Any<PackageType>(), Arg.Any<DirectoryPath>())
                .Returns(_ => new[] { FileSystem.CreateFile("/Working/tools/tool.exe") });
        }

        public void GivenNoInstallerCouldBeResolved()
        {
            Installer = null;
        }

        public ToolInstaller CreateInstaller()
        {
            var installers = new List<IPackageInstaller>();
            if (Installer != null)
            {
                installers.Add(Installer);
            }

            return new ToolInstaller(Environment, Locator, Configuration, Log, installers);
        }

        public FilePath[] Install()
        {
            return CreateInstaller().Install(Tool).ToArray();
        }
    }
}
