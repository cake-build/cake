// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Core.IO;
using Cake.Core.Tests.Fixtures;
using Cake.Core.Tooling;
using Xunit;

namespace Cake.Core.Tests.Unit.Tooling
{
    public sealed class ToolSettingsExtensionsTests
    {
        [Fact]
        public void Should_Call_WithToolSettings()
        {
            // Given
            var fixture = new DummyToolFixture();
            FilePath expect = "/temp/WithToolSettings.exe";

            // When
            fixture.Settings.WithToolSettings(settings => settings.ToolPath = expect);

            // Then
            Assert.Equal(expect.FullPath, fixture.Settings.ToolPath.FullPath);
        }

        [Fact]
        public void Should_Set_ToolPath()
        {
            // Given
            var fixture = new DummyToolFixture();
            FilePath expect = "/temp/WithToolSettings.exe";

            // When
            fixture.Settings.WithToolPath(expect);

            // Then
            Assert.Equal(expect.FullPath, fixture.Settings.ToolPath.FullPath);
        }

        [Fact]
        public void Should_Set_Timeout()
        {
            // Given
            var fixture = new DummyToolFixture();
            var expect = TimeSpan.FromSeconds(42);

            // When
            fixture.Settings.WithToolTimeout(expect);

            // Then
            Assert.Equal(expect.TotalSeconds, fixture.Settings.ToolTimeout?.TotalSeconds);
        }

        [Fact]
        public void Should_Set_WorkingDirectory()
        {
            // Given
            var fixture = new DummyToolFixture();
            DirectoryPath expect = "/temp/dir";

            // When
            fixture.Settings.WithWorkingDirectory(expect);

            // Then
            Assert.Equal(expect.FullPath, fixture.Settings.WorkingDirectory.FullPath);
        }

        [Fact]
        public void Should_Set_NoWorkingDirectory()
        {
            // Given
            var fixture = new DummyToolFixture();

            // When
            fixture.Settings.WithNoWorkingDirectory();

            // Then
            Assert.True(fixture.Settings.NoWorkingDirectory);
        }

        [Theory]
        [InlineData(true)]
        [InlineData(false)]
        public void Should_Set_NoWorkingDirectory_Arg(bool noWorkingDirectory)
        {
            // Given
            var fixture = new DummyToolFixture();

            // When
            fixture.Settings.WithNoWorkingDirectory(noWorkingDirectory);

            // Then
            Assert.Equal(noWorkingDirectory, fixture.Settings.NoWorkingDirectory);
        }

        [Fact]
        public void Should_Set_ArgumentCustomization()
        {
            // Given
            var fixture = new DummyToolFixture();
            Func<ProcessArgumentBuilder, ProcessArgumentBuilder> expect = _ => _;

            // When
            fixture.Settings.WithArgumentCustomization(expect);

            // Then
            Assert.Same(expect, fixture.Settings.ArgumentCustomization);
        }

        [Fact]
        public void Should_Set_EnvironmentVariable()
        {
            // Given
            var fixture = new DummyToolFixture();
            const string key = nameof(key);
            const string value = nameof(value);

            // When
            fixture.Settings.WithEnvironmentVariable(key, value);

            // Then
            Assert.Equal(value, fixture.Settings.EnvironmentVariables[key]);
        }

        [Fact]
        public void Should_Set_HandleExitCode()
        {
            // Given
            var fixture = new DummyToolFixture();
            Func<int, bool> expect = _ => false;

            // When
            fixture.Settings.WithHandleExitCode(expect);

            // Then
            Assert.Same(expect, fixture.Settings.HandleExitCode);
        }

        [Fact]
        public void Should_Set_PostAction()
        {
            // Given
            var fixture = new DummyToolFixture();
            Action<IProcess> expect = _ => { };

            // When
            fixture.Settings.WithPostAction(expect);

            // Then
            Assert.Same(expect, fixture.Settings.PostAction);
        }

        [Fact]
        public void Should_Set_SetupProcessSettings()
        {
            // Given
            var fixture = new DummyToolFixture();
            Action<ProcessSettings> expect = _ => { };

            // When
            fixture.Settings.WithSetupProcessSettings(expect);

            // Then
            Assert.Same(expect, fixture.Settings.SetupProcessSettings);
        }

        [Theory]
        [InlineData(0)]
        [InlineData(1)]
        [InlineData(2)]
        public void Should_Set_ReturnTrueForExpectedExitCode(int expectedExitCode)
        {
            // Given
            var fixture = new DummyToolFixture();
            fixture.Settings.WithExpectedExitCode(expectedExitCode);

            // When
            var result = fixture.Settings.HandleExitCode(expectedExitCode);

            // Then
            Assert.True(result);
        }
    }
}