// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System.Collections.Generic;
using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.NuGet.Tests.Fixtures;
using Cake.Testing;
using NSubstitute;
using Xunit;
using LogLevel = Cake.Core.Diagnostics.LogLevel;
using Verbosity = Cake.Core.Diagnostics.Verbosity;

namespace Cake.NuGet.Tests.Unit
{
    public sealed class OutProcNuGetPackageInstallerTests
    {
        public sealed class TheCanInstallMethod
        {
            private string NUGET_CONFIGKEY = "NuGet_Source";

            [Fact]
            public void Should_Throw_If_URI_Is_Null()
            {
                // Given
                var fixture = new OutOfProcessFixture();
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
                var fixture = new OutOfProcessFixture();
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
                var fixture = new OutOfProcessFixture();
                fixture.Package = new PackageReference("homebrew:?package=Cake.Core");

                // When
                var result = fixture.CanInstall();

                // Then
                Assert.False(result);
            }

            [Fact]
            public void Should_Ignore_Custom_Source_If_AbsoluteUri_Is_Used()
            {
                var fixture = new OutOfProcessFixture();
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
                var fixture = new OutOfProcessFixture();
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
                var fixture = new OutOfProcessFixture();
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
                var fixture = new OutOfProcessFixture();
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
                var fixture = new OutOfProcessFixture();
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
                var fixture = new OutOfProcessFixture();
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
                var fixture = new OutOfProcessFixture();

                // When
                fixture.Install();

                // Then
                Assert.True(fixture.FileSystem.GetDirectory("/Working/nuget").Exists);
            }

            [Fact]
            public void Should_Install_Resource()
            {
                // Given
                var fixture = new OutOfProcessFixture();

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(path => path.FullPath == "/Working/tools/nuget.exe"),
                    Arg.Is<ProcessSettings>(settings =>
                        settings.Arguments.Render() == "install \"Cake.Foo\" " +
                            "-OutputDirectory \"/Working/nuget/cake.foo.1.2.3\" " +
                            "-Source \"https://myget.org/temp/\" " +
                            "-Version \"1.2.3\" " +
                            "-Prerelease " +
                            "-ExcludeVersion " +
                            "-NonInteractive"));
            }

            [Fact]
            public void Should_Create_Directory_For_Installed_Package()
            {
                // Given
                var fixture = new OutOfProcessFixture();

                // When
                fixture.Install();

                // Then
                Assert.True(fixture.FileSystem.GetDirectory("/Working/nuget/cake.foo.1.2.3").Exists);
            }

            [Fact]
            public void Should_Install_Resource_Without_Version_Number_If_None_Is_Provided()
            {
                // Given
                var fixture = new OutOfProcessFixture();
                fixture.Package = new PackageReference("nuget:https://myget.org/temp/?package=Cake.Foo&prerelease");

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(path => path.FullPath == "/Working/tools/nuget.exe"),
                    Arg.Is<ProcessSettings>(settings =>
                        settings.Arguments.Render() == "install \"Cake.Foo\" " +
                        "-OutputDirectory \"/Working/nuget/cake.foo\" " +
                        "-Source \"https://myget.org/temp/\" " +
                        "-Prerelease " +
                        "-ExcludeVersion " +
                        "-NonInteractive"));
            }

            [Theory]
            [InlineData("nuget:?package=Cake.Foo", "/Working/nuget/cake.foo/caKe.fOO")]
            [InlineData("nuget:?package=Cake.Foo&version=1.2.3", "/Working/nuget/cake.foo.1.2.3/caKe.fOO")]
            public void Should_Look_For_Files_In_The_Correct_Directory(string uri, string path)
            {
                // Given
                var fixture = new OutOfProcessFixture();
                fixture.Package = new PackageReference(uri);

                fixture.InstallPackageAtSpecifiedPath(path);

                // When
                fixture.Install();

                // Then
                fixture.ContentResolver.Received(1).GetFiles(
                    Arg.Is<DirectoryPath>(p => p.FullPath == path),
                    Arg.Any<PackageReference>(), Arg.Any<PackageType>());
            }

            [Fact]
            public void Should_Not_Install_If_Resource_Already_Is_Installed()
            {
                // Given
                var fixture = new OutOfProcessFixture();
                fixture.FileSystem.CreateDirectory("/Working/nuget/cake.foo.1.2.3/Cake.Foo");
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

            [Fact]
            public void Should_Return_Correct_Files_When_Resource_Has_Dependencies()
            {
                // Given
                var fixture = new OutOfProcessFixture();
                fixture.FileSystem.CreateFile("/Working/nuget/cake.foo.1.2.3/Cake.Abc/Cake.Abc.dll");
                fixture.FileSystem.CreateFile("/Working/nuget/cake.foo.1.2.3/Cake.Foo/Cake.Foo.dll");
                fixture.FileSystem.CreateFile("/Working/nuget/cake.foo.1.2.3/Cake.Xyz/Cake.Xyz.dll");
                fixture.ContentResolver.GetFiles(
                        Arg.Any<DirectoryPath>(), Arg.Any<PackageReference>(), Arg.Any<PackageType>())
                    .Returns(new List<IFile> { Substitute.For<IFile>() });

                // When
                var result = fixture.Install();

                // Then
                fixture.ContentResolver.Received(1).GetFiles(
                    Arg.Is<DirectoryPath>(x => x.FullPath.Equals("/Working/nuget/cake.foo.1.2.3/Cake.Foo")),
                    fixture.Package,
                    PackageType.Addin);
            }

            [Fact]
            public void Should_Delete_Installation_Directory_If_Installation_Failed()
            {
                // Given
                var fixture = new OutOfProcessFixture();

                var process = Substitute.For<IProcess>();
                process.GetExitCode().Returns(128);
                fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(process);

                // When
                fixture.Install();

                // Then
                Assert.False(fixture.FileSystem.GetDirectory("/Working/nuget/cake.foo.1.2.3").Exists);
            }

            [Fact]
            public void Should_Use_Explicit_NuGet_Config_If_Set()
            {
                // Given
                var fixture = new OutOfProcessFixture();
                fixture.Config.GetValue(Constants.NuGet.ConfigFile).Returns("/Working/nuget.config");

                // When
                fixture.Install();

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(path => path.FullPath == "/Working/tools/nuget.exe"),
                    Arg.Is<ProcessSettings>(settings =>
                        settings.Arguments.Render() ==
                            "install \"Cake.Foo\" " +
                            "-OutputDirectory \"/Working/nuget/cake.foo.1.2.3\" " +
                            "-Source \"https://myget.org/temp/\" " +
                            "-ConfigFile \"/Working/nuget.config\" " +
                            "-Version \"1.2.3\" " +
                            "-Prerelease " +
                            "-ExcludeVersion " +
                            "-NonInteractive"));
            }
        }
    }
}
