﻿// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNet.Tool;
using Cake.Common.Tools.DotNet;
using Cake.Common.Tools.DotNet.Tool;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNet.Tool
{
    public sealed class DotNetToolTests
    {
        public sealed class TheToolMethod
        {
            [Fact]
            public void Should_Not_Throw_If_ProjectPath_IsNull()
            {
                // Given
                var fixture = new DotNetToolFixture();
                fixture.Command = "cake";
                fixture.ProjectPath = null;

                // When
                fixture.Run();
            }

            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public void Should_Throw_If_Command_IsNull(string command)
            {
                // Given
                var fixture = new DotNetToolFixture();
                fixture.ProjectPath = "./tests/Cake.Common.Tests/";
                fixture.Command = command;

                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "command");
            }

            [Fact]
            public void Should_Throw_If_Settings_Are_Null()
            {
                // Given
                var fixture = new DotNetToolFixture();
                fixture.ProjectPath = "./tests/Cake.Common.Tests/";
                fixture.Command = "xunit";
                fixture.Settings = null;

                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "settings");
            }

            [Fact]
            public void Should_Throw_If_Process_Was_Not_Started()
            {
                // Given
                var fixture = new DotNetToolFixture();
                fixture.ProjectPath = "./tests/Cake.Common.Tests/";
                fixture.Command = "xunit";
                fixture.Settings = new DotNetToolSettings();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetToolFixture();
                fixture.ProjectPath = "./tests/Cake.Common.Tests/";
                fixture.Command = "xunit";
                fixture.Settings = new DotNetToolSettings();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET CLI: Process returned an error (exit code 1).");
            }

            [Fact]
            public void Should_Wrap_Command_In_Quotes()
            {
                // Given
                var fixture = new DotNetToolFixture();
                fixture.ProjectPath = "./tests/Cake.Common.Tests/";
                fixture.Command = "C:\\example\\path with\\spaces";
                fixture.Settings = new DotNetToolSettings();

                // When
                var result = fixture.Run();

                // Then
                Assert.Equal("\"C:\\example\\path with\\spaces\"", result.Args);
            }
        }
    }
}