using System;
using System.Diagnostics;
using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class ProcessExtensionsTests
    {
        public sealed class TheStartProcessMethod
        {
            public sealed class WithoutSettings
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given
                    const string fileName = "git";

                    // When
                    var result = Record.Exception(() =>
                        ProcessExtensions.StartProcess(null, fileName));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("context", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Filename_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        ProcessExtensions.StartProcess(context, null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("fileName", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Use_Environments_Working_Directory()
                {
                    // Given
                    var fixture = new ProcessFixture();

                    // When
                    fixture.Start("hello.exe");

                    // Then
                    fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(info =>
                        info.WorkingDirectory == "/Working"));
                }

                [Fact]
                public void Should_Throw_If_No_Process_Was_Returned_From_Process_Runner()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>())
                        .Returns((IProcess)null);

                    // When
                    var result = Record.Exception(() => fixture.Start("hello.exe"));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("Could not start process.", result.Message);
                }

                [Fact]
                public void Should_Return_Exit_Code()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    fixture.Process.GetExitCode().Returns(12);

                    // When
                    var result = fixture.Start("hello.exe");

                    // Then
                    Assert.Equal(12, result);
                }
            }

            public sealed class WithSettings
            {
                [Fact]
                public void Should_Throw_If_Context_Is_Null()
                {
                    // Given
                    const string fileName = "git";
                    var settings = new ProcessSettings();

                    // When
                    var result = Record.Exception(() =>
                        ProcessExtensions.StartProcess(null, fileName, settings));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("context", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Filename_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();
                    var settings = new ProcessSettings();

                    // When
                    var result = Record.Exception(() =>
                        ProcessExtensions.StartProcess(context, null, settings));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("fileName", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Throw_If_Settings_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();
                    const string fileName = "git";

                    // When
                    var result = Record.Exception(() =>
                        ProcessExtensions.StartProcess(context, fileName, null));

                    // Then
                    Assert.IsType<ArgumentNullException>(result);
                    Assert.Equal("settings", ((ArgumentNullException)result).ParamName);
                }

                [Fact]
                public void Should_Use_Environments_Working_Directory_If_Not_Working_Directory_Is_Set()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";
                    var settings = new ProcessSettings();

                    // When
                    fixture.Start(fileName, settings);

                    // Then
                    fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(info =>
                        info.WorkingDirectory == "/Working"));
                }

                [Fact]
                public void Should_Use_Provided_Working_Directory_If_Set()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";
                    var settings = new ProcessSettings();
                    settings.WorkingDirectory = "/OtherWorking";

                    // When
                    fixture.Start(fileName, settings);

                    // Then
                    fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(info =>
                        info.WorkingDirectory == "/OtherWorking"));
                }

                [Fact]
                public void Should_Make_Working_Directory_Absolute_If_Set()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";
                    var settings = new ProcessSettings();
                    settings.WorkingDirectory = "OtherWorking";

                    // When
                    fixture.Start(fileName, settings);

                    // Then
                    fixture.ProcessRunner.Received(1).Start(Arg.Is<ProcessStartInfo>(info =>
                        info.WorkingDirectory == "/Working/OtherWorking"));
                }

                [Fact]
                public void Should_Throw_If_No_Process_Was_Returned_From_Process_Runner()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";
                    var settings = new ProcessSettings();

                    fixture.ProcessRunner.Start(Arg.Any<ProcessStartInfo>())
                        .Returns((IProcess)null);

                    // When
                    var result = Record.Exception(() => fixture.Start(fileName, settings));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("Could not start process.", result.Message);
                }

                [Fact]
                public void Should_Return_Exit_Code()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";
                    var settings = new ProcessSettings();

                    fixture.Process.GetExitCode().Returns(12);

                    // When
                    var result = fixture.Start(fileName, settings);

                    // Then
                    Assert.Equal(12, result);
                }
            }
        }
    }
}
