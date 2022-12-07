﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using System;
using Cake.Common.Tests.Fixtures.Tools.Command;
using Cake.Core.Tooling;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.Command
{
    public sealed class CommandRunnerTests
    {
        public sealed class TheRunCommandMethod
        {
            [Fact]
            public void Should_Throw_If_Arguments_Was_Null()
            {
                // Given
                var fixture = new CommandRunnerFixture
                {
                    Arguments = null
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "arguments");
            }

            [Fact]
            public void Should_Throw_If_Settings_Was_Null()
            {
                // Given
                var fixture = new CommandRunnerFixture
                {
                    Settings = null
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_ToolName_Was_Null()
            {
                // Given
                var fixture = new CommandRunnerFixture
                {
                    ToolName = null
                };

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "ToolName");
            }
        }

        [Fact]
        public void Should_Throw_If_ToolExecutableNames_Was_Null()
        {
            // Given
            var fixture = new CommandRunnerFixture
            {
                ToolExecutableNames = null
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "ToolExecutableNames");
        }

        [Fact]
        public void Should_Throw_If_ToolExecutableNames_Was_Empty()
        {
            // Given
            var fixture = new CommandRunnerFixture
            {
                ToolExecutableNames = Array.Empty<string>()
            };

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsArgumentNullException(result, "ToolExecutableNames");
        }

        [Fact]
        public void Should_Call_Settings_PostAction()
        {
            // Given
            var called = false;
            var fixture = new CommandRunnerFixture
            {
                Settings = { PostAction = _ => called = true }
            };

            // When
            var result = fixture.Run();

            // Then
            Assert.True(called, "Settings PostAction not called");
        }

        [Fact]
        public void Should_Return_StandardOutput()
        {
            // Given
            const string expectedStandardOutput = "LINE1";
            const int expectedExitCode = 0;

            var fixture = new CommandRunnerStandardOutputFixture()
                            .GivenStandardOutput(expectedStandardOutput);

            // When
            fixture.Run();

            // Then
            Assert.Equal(expectedStandardOutput, fixture.StandardOutput);
            Assert.Equal(expectedExitCode, fixture.ExitCode);
        }

        [Fact]
        public void Should_Return_StandardOutput_ExitCode()
        {
            // Given
            const string expectedStandardOutput = "LINE1";
            const int expectedExitCode = 1337;

            var fixture = new CommandRunnerStandardOutputFixture
            {
                Settings =
                {
                    HandleExitCode = exitCode => exitCode == expectedExitCode
                }
            }
                            .GivenStandardOutput(expectedStandardOutput);

            fixture.ProcessRunner.Process.SetExitCode(expectedExitCode);

            // When
            fixture.Run();

            // Then
            Assert.Equal(expectedStandardOutput, fixture.StandardOutput);
            Assert.Equal(expectedExitCode, fixture.ExitCode);
        }

        [Fact]
        public void Should_Return_StandardError()
        {
            // Given
            const string expectedStandardOutput = "LINE1";
            const string expectedStandardError = "ERRORLINE1";
            const int expectedExitCode = 0;

            var fixture = new CommandRunnerStandardErrorFixture()
                            .GivenStandardError(expectedStandardError)
                            .GivenStandardOutput(expectedStandardOutput);

            // When
            fixture.Run();

            // Then
            Assert.Equal(expectedStandardOutput, fixture.StandardOutput);
            Assert.Equal(expectedStandardError, fixture.StandardError);
            Assert.Equal(expectedExitCode, fixture.ExitCode);
        }

        [Fact]
        public void Should_Return_StandardError_ExitCode()
        {
            // Given
            const string expectedStandardOutput = "LINE1";
            const string expectedStandardError = "ERRORLINE1";
            const int expectedExitCode = 1337;

            var fixture = new CommandRunnerStandardErrorFixture
            {
                Settings =
                {
                    HandleExitCode = exitCode => exitCode == expectedExitCode
                }
            }
                            .GivenStandardError(expectedStandardError)
                            .GivenStandardOutput(expectedStandardOutput);

            fixture.ProcessRunner.Process.SetExitCode(expectedExitCode);

            // When
            fixture.Run();

            // Then
            Assert.Equal(expectedStandardOutput, fixture.StandardOutput);
            Assert.Equal(expectedStandardError, fixture.StandardError);
            Assert.Equal(expectedExitCode, fixture.ExitCode);
        }

        [Fact]
        // This unit test demonstrates the problem reported in GH-4013.
        // The command's exit code is zero and the expected exit code is any non-zero value (I used '1234' for demonstration purposes).
        // In this scenario, I expect Cake to throw an exception because the exit code does not match the expected value.
        // Currently this unit test fails because Cake does not throw the expected exception.
        // The reason why Cake currently does not throw an exception is because zero is hard-coded on line 115 in Cake.Core/Tooling.Tol.cs
        // Zero is hard coded to mean that the command was successful, and the expected exit code is completely disregarded.
        public void Exception_Thrown_when_command_exit_code_is_zero_and_expected_value_is_non_zero()
        {
            // Given
            const int actualExitCode = 0;
            const int expectedExitCode = 1234;

            var fixture = new CommandRunnerFixture();

            fixture.Settings.WithExpectedExitCode(expectedExitCode);
            fixture.ProcessRunner.Process.SetExitCode(actualExitCode);

            // When
            var result = Record.Exception(() => fixture.Run());

            // Then
            AssertEx.IsCakeException(result, "dotnet: Process returned an error (exit code 0).");
        }
    }
}
