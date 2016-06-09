// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.
using Cake.Common.Build.AppVeyor;
using Cake.Common.Tests.Fixtures.Build;
using Cake.Core;
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
                var processRunner = Substitute.For<IProcessRunner>();
                var result = Record.Exception(() => new AppVeyorProvider(null, processRunner));

                // Then
                Assert.IsArgumentNullException(result, "environment");
            }

            [Fact]
            public void Should_Throw_If_Process_Runner_Is_Null()
            {
                // Given, When
                var environment = Substitute.For<ICakeEnvironment>();
                var result = Record.Exception(() => new AppVeyorProvider(environment, null));

                // Then
                Assert.IsArgumentNullException(result, "processRunner");
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
                Assert.IsArgumentNullException(result, "path");
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
                Assert.IsExceptionWithMessage<CakeException>(result,
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
                        == "PushArtifact -Path \"/Working/file.zip\" -FileName \"file.zip\""));
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
                Assert.IsArgumentNullException(result, "version");
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
                Assert.IsExceptionWithMessage<CakeException>(result,
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
                Assert.IsExceptionWithMessage<CakeException>(result,
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
                Assert.IsArgumentNullException(result, "path");
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
                Assert.IsExceptionWithMessage<CakeException>(result,
                    "The current build is not running on AppVeyor.");
            }
        }
    }
}
