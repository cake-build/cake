// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.Tooling
{
    public sealed class ToolInstallerTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new ToolInstallerFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Locator_Is_Null()
            {
                // Given
                var fixture = new ToolInstallerFixture();
                fixture.Locator = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "locator");
            }

            [Fact]
            public void Should_Throw_If_Configuration_Is_Null()
            {
                // Given
                var fixture = new ToolInstallerFixture();
                fixture.Configuration = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "configuration");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new ToolInstallerFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.CreateInstaller());

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheInstallMethod
        {
            [Fact]
            public void Should_Throw_If_Tool_Is_Null()
            {
                // Given
                var fixture = new ToolInstallerFixture();
                fixture.Tool = null;

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                AssertEx.IsArgumentNullException(result, "tool");
            }

            [Fact]
            public void Should_Throw_If_Installer_Could_Not_Be_Resolved()
            {
                // Given
                var fixture = new ToolInstallerFixture();
                fixture.GivenNoInstallerCouldBeResolved();

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                AssertEx.IsCakeException(result, "Could not find an installer for the 'custom' scheme.");
            }

            [Fact]
            public void Should_Throw_If_Installer_Returns_No_Files()
            {
                // Given
                var fixture = new ToolInstallerFixture();

                // When
                var result = Record.Exception(() => fixture.Install());

                // Then
                AssertEx.IsCakeException(result, "Failed to install tool 'tool'.");
            }

            [Fact]
            public void Should_Install_Tool_Using_The_Resolved_Installer()
            {
                // Given
                var fixture = new ToolInstallerFixture();
                fixture.GivenFilesWillBeInstalled();

                // When
                fixture.Install();

                // Then
                fixture.Installer.Received(1).Install(
                    Arg.Is<PackageReference>(package => package.OriginalString == "custom:?package=tool"),
                    Arg.Is<PackageType>(type => type == PackageType.Tool),
                    Arg.Is<DirectoryPath>(path => path.FullPath == "/Working/tools"));
            }

            [Fact]
            public void Should_Register_Installed_Tools_With_The_Tool_Locator()
            {
                // Given
                var fixture = new ToolInstallerFixture();
                fixture.GivenFilesWillBeInstalled();

                // When
                fixture.Install();

                // Then
                fixture.Locator.Received(1).RegisterFile(
                    Arg.Is<FilePath>(path => path.FullPath == "/Working/tools/tool.exe"));
            }

            [Fact]
            public void Should_Return_Installed_File_Paths()
            {
                // Given
                var fixture = new ToolInstallerFixture();
                fixture.GivenFilesWillBeInstalled();

                // When
                var result = fixture.Install();

                // Then
                Assert.Single(result);
                Assert.Equal("/Working/tools/tool.exe", result[0].FullPath);
            }
        }
    }
}
