// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

#if !NETCORE
using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.NuGet.Tests.Fixtures;
using NSubstitute;
using Xunit;
using LogLevel = Cake.Core.Diagnostics.LogLevel;
using Verbosity = Cake.Core.Diagnostics.Verbosity;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class NuGetPackageInstallerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Process_Runner_Is_Null()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.ProcessRunner = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "processRunner");
            }

            [Fact]
            public void Should_Throw_If_Tool_Resolver_Is_Null()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.ToolResolver = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "toolResolver");
            }

            [Fact]
            public void Should_Throw_If_Content_Resolver_Is_Null()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.ContentResolver = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "contentResolver");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheCanInstallMethod
        {
            private string NUGET_CONFIGKEY = "NuGet_Source";

            [Fact]
            public void Should_Throw_If_URI_Is_Null()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Package = null;

                // When
                var result = Record.Exception(() => fixture.CanInstall());

                // Then
                AssertEx.IsArgumentNullException(result, "package");
            }

            [Fact]
            public void Should_Be_Able_To_Install_If_Scheme_Is_Correct()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Package = new PackageReference("nuget:?package=Cake.Core");

                // When
                var result = fixture.CanInstall();

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Not_Be_Able_To_Install_If_Scheme_Is_Incorrect()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Package = new PackageReference("homebrew:?package=Cake.Core");

                // When
                var result = fixture.CanInstall();

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Ignore_Custom_Source_If_AbsoluteUri_Is_Used()
            {
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Package = new PackageReference("nuget:http://absolute/?package=Cake.Core");

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.Null(result);
                fixture.Config.DidNotReceive().GetValue(NUGET_CONFIGKEY);
            }

            [Fact]
            public void Should_Use_Custom_Source_If_RelativeUri_Is_Used()
            {
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Package = new PackageReference("nuget:?package=Cake.Core");

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.Null(result);
                fixture.Config.Received().GetValue(NUGET_CONFIGKEY);
            }

            [Fact]
            public void Should_Use_NoCache_Flag_If_Parameter_Is_Used()
            {
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Package = new PackageReference("nuget:?package=Cake.Core&nocache");

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.Null(result);
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(settings =>
                        settings.Arguments.Render().Contains("-NoCache")));
            }

            [Fact]
            public void Should_Not_Use_NoCache_Flag_If_Parameter_Is_Not_Used()
            {
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Package = new PackageReference("nuget:?package=Cake.Core");

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                Assert.Null(result);
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Any<FilePath>(),
                    Arg.Is<ProcessSettings>(settings =>
                        !settings.Arguments.Render().Contains("-NoCache")));
            }
        }

        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Uri_Is_Null()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.Package = null;

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                AssertEx.IsArgumentNullException(result, "package");
            }

            [Fact]
            public void Should_Throw_If_Install_Path_Is_Null()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.InstallPath = null;

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Create_Install_Directory_If_It_Do_Not_Exist()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();

                // When
                fixture.Install();

                // Then
                Assert.True(fixture.FileSystem.GetDirectory("/Working/nuget").Exists);
            }

            [Fact]
            public void Should_Install_Resource()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(path => path.FullPath == "/Working/tools/nuget.exe"),
                    Arg.Is<ProcessSettings>(settings =>
                        settings.Arguments.Render() == "install \"Cake.Foo\" " +
                            "-OutputDirectory \"/Working/nuget\" " +
                            "-Source \"https://myget.org/temp/\" " +
                            "-Version \"1.2.3\" " +
                            "-Prerelease -ExcludeVersion " +
                            "-NonInteractive"));
            }

            [Fact]
            public void Should_Not_Install_If_Resource_Already_Is_Installed()
            {
                // Given
                var fixture = new NuGetPackageInstallerFixture();
                fixture.ContentResolver.GetFiles(
                    Arg.Any<DirectoryPath>(), Arg.Any<PackageReference>(), Arg.Any<PackageType>())
                    .Returns(new List<IFile> { Substitute.For<IFile>() });

                // When
                fixture.Install();

                // Then
                fixture.Log.Received(1).Write(
                    Verbosity.Diagnostic, LogLevel.Debug,
                    "Package {0} has already been installed.",
                    "Cake.Foo");
            }
        }
    }
}
#endif
