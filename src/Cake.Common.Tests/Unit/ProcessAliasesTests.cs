// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures;
using Cake.Core;
using Cake.Core.IO;
using NSubstitute;
using Xunit;

namespace Cake.Common.Tests.Unit
{
    public sealed class ProcessAliasesTests
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
                        ProcessAliases.StartProcess(null, fileName));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Filename_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        context.StartProcess(null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "fileName");
                }

                [Fact]
                public void Should_Use_Environments_Working_Directory()
                {
                    // Given
                    var fixture = new ProcessFixture();

                    // When
                    fixture.Start("hello.exe");

                    // Then
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(info =>
                            info.WorkingDirectory.FullPath == "/Working"));
                }

                [Fact]
                public void Should_Throw_If_No_Process_Was_Returned_From_Process_Runner()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>())
                        .Returns((IProcess)null);

                    // When
                    var result = Record.Exception(() => fixture.Start("hello.exe"));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("Could not start process.", result?.Message);
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
                        ProcessAliases.StartProcess(null, fileName, settings));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Filename_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();
                    var settings = new ProcessSettings();

                    // When
                    var result = Record.Exception(() =>
                        context.StartProcess(null, settings));

                    // Then
                    AssertEx.IsArgumentNullException(result, "fileName");
                }

                [Fact]
                public void Should_Throw_If_Settings_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();
                    const string fileName = "git";

                    // When
                    var result = Record.Exception(() =>
                        context.StartProcess(fileName, (ProcessSettings)null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "settings");
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
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(),
                        Arg.Is<ProcessSettings>(info =>
                            info.WorkingDirectory.FullPath == "/Working"));
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
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(),
                        Arg.Is<ProcessSettings>(info =>
                            info.WorkingDirectory.FullPath == "/OtherWorking"));
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
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(),
                        Arg.Is<ProcessSettings>(info =>
                            info.WorkingDirectory.FullPath == "/Working/OtherWorking"));
                }

                [Fact]
                public void Should_Throw_If_No_Process_Was_Returned_From_Process_Runner()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";
                    var settings = new ProcessSettings();

                    fixture.ProcessRunner.Start(
                        Arg.Any<FilePath>(),
                        Arg.Any<ProcessSettings>()).Returns((IProcess)null);

                    // When
                    var result = Record.Exception(() => fixture.Start(fileName, settings));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("Could not start process.", result?.Message);
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

            public sealed class WithStringArguments
            {
                [Fact]
                public void Should_Use_Provided_Arguments_In_Process_Settings_If_Set()
                {
                    // Given
                    const string fileName = "git";
                    var fixture = new ProcessFixture();

                    // When
                    fixture.Start(fileName, "pull");

                    // Then
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(),
                        Arg.Is<ProcessSettings>(info =>
                            info.Arguments.Render() == "pull"));
                }

                [Fact]
                public void Should_Not_Use_Arguments_If_Not_Set()
                {
                    // Given
                    const string fileName = "git";
                    var fixture = new ProcessFixture();

                    // When
                    fixture.Start(fileName, string.Empty);

                    // Then
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(),
                        Arg.Is<ProcessSettings>(info =>
                            string.IsNullOrEmpty(info.Arguments.Render())));
                }
            }
        }

        public sealed class TheStartAndReturnProcessMethod
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
                        ProcessAliases.StartAndReturnProcess(null, fileName));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Filename_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();

                    // When
                    var result = Record.Exception(() =>
                        context.StartAndReturnProcess(null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "fileName");
                }

                [Fact]
                public void Should_Use_Environments_Working_Directory()
                {
                    // Given
                    var fixture = new ProcessFixture();

                    // When
                    fixture.StartNewProcess("hello.exe");

                    // Then
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(), Arg.Is<ProcessSettings>(info =>
                            info.WorkingDirectory.FullPath == "/Working"));
                }

                [Fact]
                public void Should_Throw_If_No_Process_Was_Returned_From_Process_Runner()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>())
                        .Returns((IProcess)null);

                    // When
                    var result = Record.Exception(() => fixture.StartNewProcess("hello.exe"));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("Could not start process.", result?.Message);
                }

                [Fact]
                public void Should_Return_Process_Started_By_ProcessRunner()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";

                    var expected = fixture.Process;
                    fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(p => expected);

                    // When
                    var result = fixture.StartNewProcess(fileName);

                    // Then
                    Assert.Same(expected, result);
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
                        ProcessAliases.StartAndReturnProcess(null, fileName, settings));

                    // Then
                    AssertEx.IsArgumentNullException(result, "context");
                }

                [Fact]
                public void Should_Throw_If_Filename_Is_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();
                    var settings = new ProcessSettings();

                    // When
                    var result = Record.Exception(() =>
                        context.StartAndReturnProcess(null, settings));

                    // Then
                    AssertEx.IsArgumentNullException(result, "fileName");
                }

                [Fact]
                public void Should_Throw_If_Settings_Are_Null()
                {
                    // Given
                    var context = Substitute.For<ICakeContext>();
                    const string fileName = "git";

                    // When
                    var result = Record.Exception(() =>
                        context.StartAndReturnProcess(fileName, null));

                    // Then
                    AssertEx.IsArgumentNullException(result, "settings");
                }

                [Fact]
                public void Should_Use_Environments_Working_Directory_If_Not_Working_Directory_Is_Set()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";
                    var settings = new ProcessSettings();

                    // When
                    fixture.StartNewProcess(fileName, settings);

                    // Then
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(),
                        Arg.Is<ProcessSettings>(info =>
                            info.WorkingDirectory.FullPath == "/Working"));
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
                    fixture.StartNewProcess(fileName, settings);

                    // Then
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(),
                        Arg.Is<ProcessSettings>(info =>
                            info.WorkingDirectory.FullPath == "/OtherWorking"));
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
                    fixture.StartNewProcess(fileName, settings);

                    // Then
                    fixture.ProcessRunner.Received(1).Start(
                        Arg.Any<FilePath>(),
                        Arg.Is<ProcessSettings>(info =>
                            info.WorkingDirectory.FullPath == "/Working/OtherWorking"));
                }

                [Fact]
                public void Should_Throw_If_No_Process_Was_Returned_From_Process_Runner()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";
                    var settings = new ProcessSettings();

                    fixture.ProcessRunner.Start(
                        Arg.Any<FilePath>(),
                        Arg.Any<ProcessSettings>()).Returns((IProcess)null);

                    // When
                    var result = Record.Exception(() => fixture.StartNewProcess(fileName, settings));

                    // Then
                    Assert.IsType<CakeException>(result);
                    Assert.Equal("Could not start process.", result?.Message);
                }

                [Fact]
                public void Should_Return_Process_Started_By_ProcessRunner()
                {
                    // Given
                    var fixture = new ProcessFixture();
                    const string fileName = "hello.exe";
                    var settings = new ProcessSettings();

                    var expected = fixture.Process;
                    fixture.ProcessRunner.Start(Arg.Any<FilePath>(), Arg.Any<ProcessSettings>()).Returns(p => expected);

                    // When
                    fixture.StartNewProcess(fileName, settings);

                    // Then
                    Assert.IsAssignableFrom<IProcess>(expected);
                }
            }
        }
    }
}