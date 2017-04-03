// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Core.IO;
using Cake.Core.Packaging;
using Cake.Core.Tests.Fixtures;
using NSubstitute;
using Xunit;

namespace Cake.Core.Tests.Unit.Scripting
{
    public sealed class ScriptProcessorTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_File_System_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.FileSystem = null;

                // When
                var result = Record.Exception(() => fixture.CreateProcessor());

                // Then
                AssertEx.IsArgumentNullException(result, "fileSystem");
            }

            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.Environment = null;

                // When
                var result = Record.Exception(() => fixture.CreateProcessor());

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.Log = null;

                // When
                var result = Record.Exception(() => fixture.CreateProcessor());

                // Then
                AssertEx.IsArgumentNullException(result, "log");
            }
        }

        public sealed class TheInstallAddinsMethod
        {
            [Fact]
            public void Should_Throw_If_Analyzer_Result_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.Result = null;

                // When
                var result = Record.Exception(() => fixture.InstallAddins());

                // Then
                AssertEx.IsArgumentNullException(result, "analyzerResult");
            }

            [Fact]
            public void Should_Throw_If_Install_Path_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.InstallPath = null;

                // When
                var result = Record.Exception(() => fixture.InstallAddins());

                // Then
                AssertEx.IsArgumentNullException(result, "installPath");
            }

            [Fact]
            public void Should_Throw_If_Addins_Could_Not_Be_Found()
            {
                // Given
                var fixture = new ScriptProcessorFixture();

                // When
                var result = Record.Exception(() => fixture.InstallAddins());

                // Then
                AssertEx.IsCakeException(result, "Failed to install addin 'addin'.");
            }

            [Fact]
            public void Should_Throw_If_Installer_Could_Not_Be_Resolved()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.GivenNoInstallerCouldBeResolved();

                // When
                var result = Record.Exception(() => fixture.InstallAddins());

                // Then
                AssertEx.IsCakeException(result, "Could not find an installer for the 'custom' scheme.");
            }

            [Fact]
            public void Should_Install_Addins_Referenced_By_Scripts()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.GivenFilesWillBeInstalled();

                // When
                fixture.InstallAddins();

                // Then
                fixture.Installer.Received().Install(
                    Arg.Is<PackageReference>(package => package.OriginalString == "custom:?package=addin"),
                    Arg.Is<PackageType>(type => type == PackageType.Addin),
                    Arg.Is<DirectoryPath>(path => path.FullPath == "/Working/Bin"));
            }
        }

        public sealed class TheInstallToolsMethod
        {
            [Fact]
            public void Should_Throw_If_Analyzer_Result_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.Result = null;

                // When
                var result = Record.Exception(() => fixture.InstallTools());

                // Then
                AssertEx.IsArgumentNullException(result, "analyzerResult");
            }

            [Fact]
            public void Should_Throw_If_Install_Path_Is_Null()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.InstallPath = null;

                // When
                var result = Record.Exception(() => fixture.InstallTools());

                // Then
                AssertEx.IsArgumentNullException(result, "installPath");
            }

            [Fact]
            public void Should_Throw_If_Tools_Could_Not_Be_Found()
            {
                // Given
                var fixture = new ScriptProcessorFixture();

                // When
                var result = Record.Exception(() => fixture.InstallTools());

                // Then
                AssertEx.IsCakeException(result, "Failed to install tool 'tool'.");
            }

            [Fact]
            public void Should_Throw_If_Installer_Could_Not_Be_Resolved()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.GivenNoInstallerCouldBeResolved();

                // When
                var result = Record.Exception(() => fixture.InstallTools());

                // Then
                AssertEx.IsCakeException(result, "Could not find an installer for the 'custom' scheme.");
            }

            [Fact]
            public void Should_Install_Tools_Referenced_By_Scripts()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.GivenFilesWillBeInstalled();

                // When
                fixture.InstallTools();

                // Then
                fixture.Installer.Received(1).Install(
                    Arg.Is<PackageReference>(package => package.OriginalString == "custom:?package=tool"),
                    Arg.Is<PackageType>(type => type == PackageType.Tool),
                    Arg.Is<DirectoryPath>(path => path.FullPath == "/Working/Bin"));
            }

            [Fact]
            public void Should_Register_Installed_Tools_With_The_Tool_Service()
            {
                // Given
                var fixture = new ScriptProcessorFixture();
                fixture.GivenFilesWillBeInstalled();

                // When
                fixture.InstallTools();

                // Then
                fixture.Tools.Received(1).RegisterFile(
                    Arg.Is<FilePath>(path => path.FullPath == "/Working/Bin/Temp.dll"));
            }
        }
    }
}