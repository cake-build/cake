// Licensed to the .NET Foundation under one or more agreements.
// The .NET Foundation licenses this file to you under the MIT license.
// See the LICENSE file in the project root for more information.

using Cake.Common.Tests.Fixtures.Tools.DotNetCore.Tool;
using Cake.Common.Tools.DotNetCore;
using Cake.Common.Tools.DotNetCore.Tool;
using Cake.Testing;
using Xunit;

namespace Cake.Common.Tests.Unit.Tools.DotNetCore.Tool
{
    public sealed class DotNetCoreToolTests
    {
        public sealed class TheToolMethod
        {
            [Fact]
            public void Should_Throw_If_ProjectPath_IsNull()
            {
                // Given
                var fixture = new DotNetCoreToolFixture();
                fixture.ProjectPath = null;
                fixture.GivenDefaultToolDoNotExist();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsArgumentNullException(result, "projectPath");
            }

            [Theory]
            [InlineData("")]
            [InlineData(null)]
            public void Should_Throw_If_Command_IsNull(string command)
            {
                // Given
                var fixture = new DotNetCoreToolFixture();
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
                var fixture = new DotNetCoreToolFixture();
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
                var fixture = new DotNetCoreToolFixture();
                fixture.ProjectPath = "./tests/Cake.Common.Tests/";
                fixture.Command = "xunit";
                fixture.Settings = new DotNetCoreToolSettings();
                fixture.GivenProcessCannotStart();

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process was not started.");
            }

            [Fact]
            public void Should_Throw_If_Process_Has_A_Non_Zero_Exit_Code()
            {
                // Given
                var fixture = new DotNetCoreToolFixture();
                fixture.ProjectPath = "./tests/Cake.Common.Tests/";
                fixture.Command = "xunit";
                fixture.Settings = new DotNetCoreToolSettings();
                fixture.GivenProcessExitsWithCode(1);

                // When
                var result = Record.Exception(() => fixture.Run());

                // Then
                AssertEx.IsCakeException(result, ".NET Core CLI: Process returned an error (exit code 1).");
            }
        }
    }
}