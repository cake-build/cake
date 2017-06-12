// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Build.AppVeyor;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
using Cake.Core.Diagnostics;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit.Build.AppVeyor
{
    public sealed class AppVeyorProviderTests
    {
        public sealed class TheConstructor
        {
            [Fact]
            public void Should_Throw_If_Environment_Is_Null()
            {
                // Given, When
                var cakeLog = Substitute.For<ICakeLog>();
                var processRunner = Substitute.For<IProcessRunner>();
                var result = Record.Exception(() => new AppVeyorProvider(null, processRunner, cakeLog));

                // Then
                AssertEx.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Process_Runner_Is_Null()
            {
                // Given, When
                var cakeLog = Substitute.For<ICakeLog>();
                var environment = Substitute.For<ICakeEnvironment>();
                var result = Record.Exception(() => new AppVeyorProvider(environment, null, cakeLog));

                // Then
                AssertEx.IsArgumentNullException(result, "processRunner");
            }

            [Fact]
            public void Should_Throw_If_Log_Is_Null()
            {
                // Given, When
                var processRunner = Substitute.For<IProcessRunner>();
                var environment = Substitute.For<ICakeEnvironment>();
                var result = Record.Exception(() => new AppVeyorProvider(environment, processRunner, null));

                // Then
                AssertEx.IsArgumentNullException(result, "cakeLog");
            }
        }

        public sealed class TheIsRunningOnAppVeyorProperty
        {
            [Fact]
            public void Should_Return_True_If_Running_On_AppVeyor()
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = appVeyor.IsRunningOnAppVeyor;

                // Then
                Assert.True(result);
            }

            [Fact]
            public void Should_Return_False_If_Not_Running_On_AppVeyor()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = appVeyor.IsRunningOnAppVeyor;

                // Then
                Assert.False(result);
            }
        }

        public sealed class TheEnvironmentProperty
        {
            [Fact]
            public void Should_Return_Non_Null_Reference()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = appVeyor.Environment;

                // Then
                Assert.NotNull(result);
            }
        }

        public sealed class TheUploadArtifactMethod
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.UploadArtifact(null));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_Path_Is_Null_WithSettings()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.UploadArtifact(null, settings => settings.SetArtifactType(AppVeyorUploadArtifactType.Auto)));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_Not_Running_On_AppVeyor()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.UploadArtifact("./file.zip"));

                // Then
                AssertEx.IsExceptionWithMessage<CakeException>(result,
                    "The current build is not running on AppVeyor.");
            }

            [Fact]
            public void Should_Upload_Artifact()
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                appVeyor.UploadArtifact("./file.zip");

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "appveyor"),
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render()
                        == "PushArtifact \"/Working/file.zip\" -Type Auto"));
            }

            [Theory]
            [InlineData(AppVeyorUploadArtifactType.Auto, "Auto")]
            [InlineData(AppVeyorUploadArtifactType.WebDeployPackage, "WebDeployPackage")]
            [InlineData(AppVeyorUploadArtifactType.NuGetPackage, "NuGetPackage")]
            public void Should_Upload_Artifact_For_ArtifactType(AppVeyorUploadArtifactType type, string arg)
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                appVeyor.UploadArtifact("./file.zip", settings => settings.SetArtifactType(type));

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "appveyor"),
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render()
                        == $"PushArtifact \"/Working/file.zip\" -Type {arg}"));
            }

            [Fact]
            public void Should_Upload_Artifact_For_DeploymentName()
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                appVeyor.UploadArtifact("./file.zip", settings => settings.SetDeploymentName("MyApp.Web"));

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "appveyor"),
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render()
                        == "PushArtifact \"/Working/file.zip\" -Type Auto -DeploymentName \"MyApp.Web\""));
            }

            [Fact]
            public void Should_Upload_Artifact_Throw_Exception_For_DeploymentName_Containing_Spaces()
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.UploadArtifact("./file.zip", settings => settings.SetDeploymentName("MyApp Web")));

                // Then
                AssertEx.IsCakeException(result, "The deployment name can not contain spaces");
            }
        }

        public sealed class TheUpdateBuildVersionMethod
        {
            [Fact]
            public void Should_Throw_If_Build_Version_Is_Null()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.UpdateBuildVersion(null));

                // Then
                AssertEx.IsArgumentNullException(result, "version");
            }

            [Theory]
            [InlineData("")]
            [InlineData("\t")]
            public void Should_Throw_If_Build_Version_Is_Empty(string version)
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.UpdateBuildVersion(version));

                // Then
                AssertEx.IsExceptionWithMessage<CakeException>(result,
                    "The build version cannot be empty.");
            }

            [Fact]
            public void Should_Throw_If_Not_Running_On_AppVeyor()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.UpdateBuildVersion("build-123"));

                // Then
                AssertEx.IsExceptionWithMessage<CakeException>(result,
                    "The current build is not running on AppVeyor.");
            }

            [Fact]
            public void Should_Update_Build_Version()
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                appVeyor.UpdateBuildVersion("build-123");

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "appveyor"),
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render() == "UpdateBuild -Version \"build-123\""));
            }
        }

        public sealed class TheUploadTestResultsMethod
        {
            [Fact]
            public void Should_Throw_If_Path_Is_Null()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.UploadTestResults(null, AppVeyorTestResultsType.XUnit));

                // Then
                AssertEx.IsArgumentNullException(result, "path");
            }

            [Fact]
            public void Should_Throw_If_Not_Running_On_AppVeyor()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.UploadTestResults("./file.xml", AppVeyorTestResultsType.XUnit));

                // Then
                AssertEx.IsExceptionWithMessage<CakeException>(result,
                    "The current build is not running on AppVeyor.");
            }
        }

        public sealed class TheAddMessageMethod
        {
            [Fact]
            public void Should_Throw_If_Message_Is_Null()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.AddMessage(null));

                // Then
                AssertEx.IsArgumentNullException(result, "message");
            }

            [Fact]
            public void Should_Throw_If_Message_Is_Empty()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.AddMessage(""));

                // Then
                AssertEx.IsCakeException(result, "The message cannot be empty.");
            }

            [Fact]
            public void Should_Throw_If_Not_Running_On_AppVeyor()
            {
                // Given
                var fixture = new AppVeyorFixture();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                var result = Record.Exception(() => appVeyor.AddMessage("Hello world"));

                // Then
                AssertEx.IsExceptionWithMessage<CakeException>(result,
                    "The current build is not running on AppVeyor.");
            }

            [Theory]
            [InlineData("Hello world", AppVeyorMessageCategoryType.Information, null, "\"Hello world\" -Category \"Information\"")]
            [InlineData("Hello world", AppVeyorMessageCategoryType.Warning, null, "\"Hello world\" -Category \"Warning\"")]
            [InlineData("Hello world", AppVeyorMessageCategoryType.Error, null, "\"Hello world\" -Category \"Error\"")]
            [InlineData("Hello world", AppVeyorMessageCategoryType.Error, "Details of message", "\"Hello world\" -Category \"Error\" -Details \"Details of message\"")]
            public void Should_Add_Message(string message, AppVeyorMessageCategoryType category, string details, string args)
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();

                // When
                appVeyor.AddMessage(message, category, details);

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "appveyor"),
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render() == $"AddMessage {args}"));
            }

            [Fact]
            public void Should_Add_InformationalMessage()
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();
                const string message = "Hello world";

                // When
                appVeyor.AddInformationalMessage(message);

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "appveyor"),
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render() ==
                                                 $"AddMessage \"{message}\" -Category \"Information\""));
            }

            [Fact]
            public void Should_Add_WarningMessage()
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();
                const string message = "Hello world";

                // When
                appVeyor.AddWarningMessage(message);

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "appveyor"),
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render() ==
                                                 $"AddMessage \"{message}\" -Category \"Warning\""));
            }

            [Fact]
            public void Should_Add_ErrorMessage()
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();
                const string message = "Hello world";

                // When
                appVeyor.AddErrorMessage(message);

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "appveyor"),
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render() == $"AddMessage \"{message}\" -Category \"Error\""));
            }

            [Fact]
            public void Should_Add_ErrorMessageWithException()
            {
                // Given
                var fixture = new AppVeyorFixture();
                fixture.IsRunningOnAppVeyor();
                var appVeyor = fixture.CreateAppVeyorService();
                const string message = "Hello world";
                var exception = new CakeException("This is an exception", new ArgumentException());

                // When
                appVeyor.AddErrorMessage(message, exception);

                // Then
                fixture.ProcessRunner.Received(1).Start(
                    Arg.Is<FilePath>(p => p.FullPath == "appveyor"),
                    Arg.Is<ProcessSettings>(p => p.Arguments.Render() ==
                                                 $"AddMessage \"{message}\" -Category \"Error\" -Details \"{exception.ToString()}\""));
            }
        }
    }
}