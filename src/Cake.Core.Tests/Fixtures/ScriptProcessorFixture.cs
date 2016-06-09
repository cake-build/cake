// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using System.Collections.Generic;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Scripting;
using Cake.Core.Scripting.Analysis;
using Cake.Core.Tooling;
using Cake.Testing;
using NSubstitute;

namespace Cake.Core.Tests.Fixtures
{
    public sealed class ScriptProcessorFixture
    {
        public FakeFileSystem FileSystem { get; set; }
        public ICakeEnvironment Environment { get; set; }
        public ICakeLog Log { get; set; }
        public IToolLocator Tools { get; set; }
        public IPackageInstaller Installer { get; set; }

        public ScriptAnalyzerResult Result { get; set; }
        public DirectoryPath InstallPath { get; set; }

        public ScriptProcessorFixture()
        {
            Environment = FakeEnvironment.CreateUnixEnvironment();
            FileSystem = new FakeFileSystem(Environment);
            Log = Substitute.For<ICakeLog>();
            Tools = Substitute.For<IToolLocator>();

            Installer = Substitute.For<IPackageInstaller>();
            Installer.CanInstall(Arg.Any<PackageReference>(), Arg.Any<PackageType>()).Returns(true);
            InstallPath = new DirectoryPath("/Working/Bin");

            // Create the script analyzer result.
            var script = new ScriptInformation("/Working/build.cake");
            script.Addins.Add(new PackageReference("custom:?package=addin"));
            script.Tools.Add(new PackageReference("custom:?package=tool"));
            Result = new ScriptAnalyzerResult(script, new List<string>());
        }

        public void GivenFilesWillBeInstalled()
        {
            Installer
                .Install(Arg.Any<PackageReference>(), Arg.Any<PackageType>(), Arg.Any<DirectoryPath>())
                .Returns(info => new[] { FileSystem.CreateFile("/Working/Bin/Temp.dll") });
        }

        public void GivenNoInstallerCouldBeResolved()
        {
            Installer = null;
        }

        public ScriptProcessor CreateProcessor()
        {
            var installers = new List<IPackageInstaller>();
            if (Installer != null)
            {
                installers.Add(Installer);
            }
            return new ScriptProcessor(FileSystem, Environment, Log, Tools, installers);
        }

        public void InstallAddins()
        {
            CreateProcessor().InstallAddins(Result, InstallPath);
        }

        public void InstallTools()
        {
            CreateProcessor().InstallTools(Result, InstallPath);
        }
    }
}
